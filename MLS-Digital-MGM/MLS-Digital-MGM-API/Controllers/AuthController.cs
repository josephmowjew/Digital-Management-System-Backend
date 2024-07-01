using DataStore.Core.DTOs;
using DataStore.Core.DTOs.Authentication;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Core.ViewModels;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using DataStore.Persistence.SQLRepositories;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Cmp;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

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

        /// Authenticates a user with the provided email and password.
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            // Find the user by email
            var user = await _repositoryManager.UserRepository.FindByEmailAsync(loginViewModel.Email);

            // Check if the user exists, is not deleted, and has confirmed their email
            if (user == null || user.DeletedDate != null || !user.EmailConfirmed)
                return BadRequest(GetErrorMessage(user));

            // Check the password for the user or handle Google authentication
            var isGoogleAuth = loginViewModel.AuthProvider?.Equals("Google", StringComparison.CurrentCultureIgnoreCase) == true;
            if (isGoogleAuth || (await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false)).Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                user.LastLogin = DateTime.UtcNow;
                await _repositoryManager.UnitOfWork.CommitAsync();

                var token = await GenerateToken(user);
                return Ok(new { TokenData = token });
            }

            // Return an error message if the credentials are invalid
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
        public async Task<ActionResult> Register([FromBody]RegisterDTO model)
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
                var roleResult = await _repositoryManager.UserRepository.AddUserToRoleAsync(user, model.RoleName);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("Role", "Failed to associate the user with the specified role.");
                    return BadRequest(ModelState);
                }
            }

            // Send login details email
            string passwordBody = $"Your account has been created on Malawi Law Society. Your login details are as follows: <br /> Email: {model.Email} <br /> Password: {model.Password} <br /> Login to Sparc Rides. Your password is {model.Password}";
            var passwordEmailResult = BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(user.Email, "Login Details", passwordBody));
          
            // Send OTP email
            string pinBody = $"An account has been created on Malawi Law Society. Your OTP is {pin} <br /> Enter the OTP to activate your account <br /> You can activate your account by clicking <a href='https://cutt.ly/mentallab'>here</a>";
            var pinEmailResult = BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(user.Email, "Login Details", pinBody));
           

            return Ok(user);
        }
        [AllowAnonymous]
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

            return Ok(new {isSuccess=true, response = "Account confirmed", user = user });

        }
        [HttpGet]
        [Route("ResendPin/{email}")]
        // Resending account activation pin
        public async Task<IActionResult> ResendPin(string email)
        {
            //d6ea9738-d81b-46f9-b6e9-e3260fb75892
            //check if values are not null
            if (email == null)
            {

                return BadRequest("Insufficient parameters, try again");
            }

            // check if user exist
            var user = await _repositoryManager.UserRepository.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("Account doesn't exist, please create one");
            }

            // confirming account and saving 
            int pin = Lambda.RandomNumber();
            user.Pin = pin;

            await _repositoryManager.UnitOfWork.CommitAsync();

            // sending an email
            string PinBody = "Your OTP for Sparc Rides Account is " + pin + " <br /> Enter the OTP, email address and the new password to reset your account";
            var pinEmailResult = BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(user.Email, "Account Reset Details", PinBody));

           

            return Json( new {isSuccess=true, message="Check your email for the pin"});


        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(PasswordResetModel model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the user based on the email provided
            var user = await this._repositoryManager.UserRepository.FindByEmailAsync(model.Email);

            // If the user is not found, return an error
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email not recognized");
                return BadRequest(ModelState);
            }

            // Check if the provided pin matches the user's pin
            if (user.Pin != model.Pin)
            {
                ModelState.AddModelError("Pin", "Pin is invalid");
                return BadRequest(ModelState);
            }

            // Generate a password reset token for the user
            var token = await this._repositoryManager.UserManager.GeneratePasswordResetTokenAsync(user);

            // Reset the user's password using the token and the new password
            var result = await this._repositoryManager.UserManager.ResetPasswordAsync(user, token, model.Password);

            // If the password reset is successful, return a success message
            if (result.Succeeded)
            {
               return Json(new {isSuccess=true, message = "Password reset successfully" });
            }

            // If the password reset fails, return an error
            ModelState.AddModelError("GeneralError", "Failed to reset password");
            return BadRequest(ModelState);
        }


        //user password
        [HttpPost]
        [Route("PasswordReset")]
        public async Task<IActionResult> PasswordReset(PasswordResetModel model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the user based on the email provided
            var user = await this._repositoryManager.UserRepository.FindByEmailAsync(model.Email);

            // If the user is not found, return an error
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email not recognized");
                return BadRequest(ModelState);
            }

            // Check if the provided code is valid
            if (model.Code == null)
            {
                ModelState.AddModelError("Code", "Code is invalid");
                return BadRequest(ModelState);
            }

            // Generate a password reset token for the user
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));

            // Reset the user's password using the token and the new password
            var result = await this._repositoryManager.UserManager.ResetPasswordAsync(user, token, model.Password);

            // If the password reset is successful, return a success message
            if (result.Succeeded)
            {
                 return Json(new { isSuccess=true, message = "Password  has been reset successfully" });
            }
            else{
                // If the password reset fails, return an error
                ModelState.AddModelError("GeneralError", "Failed to reset password");
                return BadRequest(ModelState);
            }

           
        }

        //generate forgot password link
        //user password
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(PasswordResetModel model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the user based on the email provided
            var user = await this._repositoryManager.UserRepository.FindByEmailAsync(model.Email);

            // If the user is not found, return an error
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email not recognized");
                return BadRequest(ModelState);
            }

            // Generate password reset token
            var token = await this._repositoryManager.UserManager.GeneratePasswordResetTokenAsync(user);

            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var callbackUrl = $"{Request.Scheme}://localhost:5002/Home/ResetPassword?code={code}";

            var body = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

            var pinEmailResult = BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(user.Email, "Reset Password", body));

            //return  json with message
            return Json(new { isSuccess=true, message = "Password reset link was sent to your email successfully" });

        }

    }
}

