using DataStore.Core.DTOs;
using DataStore.Core.DTOs.Authentication;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Core.ViewModels;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using DataStore.Persistence.SQLRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService; 
        public IConfiguration Configuration { get; }

        public AuthController(IRepositoryManager repositoryManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IEmailService emailService)
        {
            this._signInManager = signInManager;
            this._repositoryManager = repositoryManager;
            this.Configuration = configuration;
            this._emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var user = await _repositoryManager.UserRepository.FindByEmailAsync(loginViewModel.Email);

            if (user == null || user.DeletedDate != null || !user.EmailConfirmed)
            {
                return BadRequest(GetErrorMessage(user));
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);

            if (signInResult.Succeeded)
            {
                user.LastLogin = DateTime.UtcNow;
                await _repositoryManager.UnitOfWork.CommitAsync();

                var token = await GenerateToken(user);

                return Ok(new { TokenData = token });
            }

            return BadRequest("Invalid login credentials");
        }

        // A function that takes an ApplicationUser object as a parameter and returns an error message based on the user's status.
        private string GetErrorMessage(ApplicationUser user)
        {
            return user == null ? "Account not found" : user.DeletedDate != null ? "Your account is suspended" : "Account not confirmed, please check your email for the activation link";
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO model)
        {
            // Validate the model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Generate a random PIN
            int pin = Lambda.RandomNumber();

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                OtherName = model.OtherName,
                Gender = model.Gender,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IdentityNumber = model.IdentityNumber,
                IdentityExpiryDate = model.IdentityExpiryDate,
                DateOfBirth = model.DateOfBirth,
                DepartmentId = model.DepartmentId,
                IdentityTypeId = model.IdentityTypeId,
                TitleId = model.TitleId,
                CountryId = model.CountryId,
                Pin = pin
            };

            // Check if the email is already registered
            var existingUser = await _repositoryManager.UserRepository.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already in use.");
                return BadRequest(ModelState);
            }

            // Add the new user
            var result = await _repositoryManager.UserRepository.AddAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                return BadRequest(ModelState);
            }

            if (!string.IsNullOrWhiteSpace(model.RoleName))
            {
                // Associate the user with the specified role
                var roleResult = await _repositoryManager.UserRepository.AddAsync(user, model.RoleName);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("Role", "Failed to associate the user with the specified role.");
                    return BadRequest(ModelState);
                }
            }

            // Send login details email
            string passwordBody = $"Your account has been created on Sparc Rides. Your password is {model.Password}";
            var passwordEmailResult = await _emailService.SendMailWithKeyVarReturn(user.Email, "Login Details", passwordBody);
            if (!passwordEmailResult.Key)
            {
                ModelState.AddModelError(nameof(model.Email), $"Failed to send login details email. Error: {passwordEmailResult.Value}");
                return BadRequest(ModelState);
            }

            // Send OTP email
            string pinBody = $"An account has been created on Sparc Rides. Your OTP is {pin} <br /> Enter the OTP to activate your account <br /> You can activate your account by clicking <a href='https://cutt.ly/mentallab'>here</a>";
            var pinEmailResult = await _emailService.SendMailWithKeyVarReturn(user.Email, "Login Details", pinBody);
            if (!pinEmailResult.Key)
            {
                ModelState.AddModelError(nameof(model.Email), $"Failed to send OTP email. Error: {pinEmailResult.Value}");
                return BadRequest(ModelState);
            }

            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost("GenerateToken")]
        private async Task<LoginDTO> GenerateToken(ApplicationUser user)
        {
            //if successful generate the token based on details given. Valid for one day
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("TokenString:TokenKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                            new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Configuration.GetValue<double>("TokenString:expiryMinutes")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);


            var role = _repositoryManager.UserRepository.GetUserRoleByUserId( user.Id);
            var roleData = await _repositoryManager.RoleRepository.GetRoleByIdAsync(role.RoleId);

            // login DTO
            var userData = new LoginDTO()
            {
                Token = tokenString,
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = roleData.Name,
                Email = user.Email,
                TokenType = "bearer",
                DateOfBirth = user.DateOfBirth,
                TokenExpiryMinutes = (DateTime)tokenDescriptor.Expires,

            };
            return userData;

        }

        [HttpGet]
        [Route("ConfirmAccount/{email}/{pin}")]
        // confirming account
        public async Task<IActionResult> ConfirmAccount(string email, int pin)
        {
            //check if values are not null
            if (pin < 0 || email == null)
            {
                ModelState.AddModelError("Pin", "Pin Cannot be empty");

                return BadRequest("pin cannot be empty");
            }

            // check if user exist
            var user = await this._repositoryManager.UserRepository.FindByEmailAsync(email);

            if (user == null)
            {
                //ModelState.AddModelError("Account Error", "Missing Account");
                return BadRequest("account not found");
            }

            // checking is the pin submitted is the same as the pin in db
            if (user.Pin != pin)
            {
                //ModelState.AddModelError("Account Error", "Missing Account");

                return BadRequest("invalid pin");
            }

            // confirming account and saving 
            user = await _repositoryManager.UserRepository.ConfirmAccount(user.Id, pin);

            await _repositoryManager.UnitOfWork.CommitAsync();

            return Ok(new { response = "Account confirmed", user = user });

        }

    }
}
