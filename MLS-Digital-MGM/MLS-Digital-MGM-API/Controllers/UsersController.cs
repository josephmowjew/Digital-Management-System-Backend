using Microsoft.AspNetCore.Mvc;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataStore.Core.DTOs.User;
using DataStore.Helpers;
using System.Linq.Expressions;
using MLS_Digital_MGM.DataStore.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using DataStore.Core.Services;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class UsersController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly SignatureService _signatureService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UsersController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, SignatureService signatureService, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _signatureService = signatureService;
            _configuration = configuration;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetActiveUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
            
                var pagingParameters = new PagingParameters<ApplicationUser>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.EmailConfirmed == true,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated staff users using the UserRepository
                var staffUsers = await _repositoryManager.UserRepository.GetPagedStaffUsersAsync(pagingParameters);

                // Check if users exist
                if (staffUsers == null || !staffUsers.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadUserDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadUserDTO>()); // Return empty list
                }
        
                // Map User entities to ReadUserDTOs
                var mappedUsers = _mapper.Map<IEnumerable<ReadUserDTO>>(staffUsers);

                var usersWithRoles = mappedUsers.Select(user =>
                {
                    var userRole = _repositoryManager.UserRepository.GetUserRoleByUserId(user.Id);
                    string roleName = userRole != null 
                        ? _repositoryManager.UserRepository.GetRoleName(userRole.RoleId) 
                        : string.Empty;
                    user.RoleName = FormatRoleName(roleName);
                    return user;
                }).ToList();

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = usersWithRoles.Count;
                    var totalRecords = await _repositoryManager.UserRepository.CountStaffUsersAsync(pagingParameters);


                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = totalRecords, 
                        recordsTotal = totalRecords, 
                        data = usersWithRoles.ToList() // Materialize the enumerable
                    });
                }

                return Ok(mappedUsers); // Return paginated users
        
            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);
        
                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("suspended")]
        public async Task<IActionResult> GetSuspendedUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
            
                var pagingParameters = new PagingParameters<ApplicationUser>
                {
                    Predicate = u => u.Status == Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetPagedAsync(pagingParameters);

            
               // Check if users exist
                if (users == null || !users.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadUserDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadUserDTO>()); // Return empty list
                }
        
                // Map User entities to ReadUserDTOs
                var mappedUsers = _mapper.Map<IEnumerable<ReadUserDTO>>(users);

                //get the user role of the user
                var usersWithRoles = new List<ReadUserDTO>();

                mappedUsers.ToList().ForEach(user =>
                {
                   var userRole =  this._repositoryManager.UserRepository.GetUserRoleByUserId(user.Id);
                   string roleName = this._repositoryManager.UserRepository.GetRoleName(userRole.RoleId);
                   user.RoleName = FormatRoleName(roleName);
                   usersWithRoles.Add(user);
                
                });

               
                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = usersWithRoles.Count;

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = resultTotalFiltred, 
                        recordsTotal = resultTotalFiltred, 
                        data = usersWithRoles.ToList() // Materialize the enumerable
                    });
                }

                return Ok(mappedUsers); // Return paginated users
        
            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);
        
                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("unconfirmedUsers")]
        public async Task<IActionResult> UnconfirmedUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
            
                var pagingParameters = new PagingParameters<ApplicationUser>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.EmailConfirmed == false,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetPagedAsync(pagingParameters);

            
               // Check if users exist
                if (users == null || !users.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadUserDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadUserDTO>()); // Return empty list
                }
        
                // Map User entities to ReadUserDTOs
                var mappedUsers = _mapper.Map<IEnumerable<ReadUserDTO>>(users);

                //get the user role of the user
                var usersWithRoles = new List<ReadUserDTO>();

                mappedUsers.ToList().ForEach(user =>
                {
                   var userRole =  this._repositoryManager.UserRepository.GetUserRoleByUserId(user.Id);
                   string roleName = this._repositoryManager.UserRepository.GetRoleName(userRole.RoleId);
                   user.RoleName = FormatRoleName(roleName);
                   usersWithRoles.Add(user);
                
                });

               
                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = usersWithRoles.Count;

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = resultTotalFiltred, 
                        recordsTotal = resultTotalFiltred, 
                        data = usersWithRoles.ToList() // Materialize the enumerable
                    });
                }

                return Ok(mappedUsers); // Return paginated users
        
            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getMembersWithMissingMemberRecord")]
        public async Task<IActionResult> GetUserWithMemberRoleAndMissingMemberRecord(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<ApplicationUser>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null

                };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetMembersWithMissingMemberRecordAsync(pagingParameters);


                // Check if users exist
                if (users == null || !users.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadUserDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadUserDTO>()); // Return empty list
                }

                // Map User entities to ReadUserDTOs
                var mappedUsers = _mapper.Map<IEnumerable<ReadUserDTO>>(users);

                //get the user role of the user
                var usersWithRoles = new List<ReadUserDTO>();

                mappedUsers.ToList().ForEach(user =>
                {
                    var userRole = this._repositoryManager.UserRepository.GetUserRoleByUserId(user.Id);
                    string roleName = this._repositoryManager.UserRepository.GetRoleName(userRole.RoleId);
                    user.RoleName = FormatRoleName(roleName);
                    usersWithRoles.Add(user);

                });


                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = usersWithRoles.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltred,
                        recordsTotal = resultTotalFiltred,
                        data = usersWithRoles.ToList() // Materialize the enumerable
                    });
                }

                return Ok(mappedUsers); // Return paginated users

            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("getDeletedUsers")]
        public async Task<IActionResult> DeletedUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                
                // Create PagingParameters object
                var pagingParameters = new PagingParameters<ApplicationUser>{
                    Predicate = u => u.Status == Lambda.Deleted,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                    //SearchTerm = null
                };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetPagedAsync(pagingParameters);

        
                // Check if users exist
                if (users == null || !users.Any())
                {
                    return Ok(Enumerable.Empty<ReadUserDTO>()); 
                }
        
                // Map User entities to ReadUserDTOs
                var mappedUsers = _mapper.Map<IEnumerable<ReadUserDTO>>(users);

                //get the user role of the user

               
                return Ok(mappedUsers); // Return paginated users
        
            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                // Fetch paginated users using the UserRepository
                var user = await _repositoryManager.UserRepository.GetSingleUser(id);

                if(user != null)
                {
                    var mappedData = _mapper.Map<ReadUserDTO>(user);

                    var userRole = this._repositoryManager.UserRepository.GetUserRoleByUserId(user.Id);
                    string roleName = this._repositoryManager.UserRepository.GetRoleName(userRole.RoleId);

                    //update role name

                    mappedData.RoleName = roleName;
                    return Ok(_mapper.Map<ReadUserDTO>(mappedData));
                }
                return BadRequest("user not found");

            }
            catch (Exception ex)
            {

                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }
      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromForm]UpdateUserDTO userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _repositoryManager.UserRepository.GetSingleUser(id);
                if (user == null)
                {
                    return NotFound();
                }

                if(userDTO.DepartmentId == 0 || userDTO.DepartmentId == null)
                {
                    //get the department of the user from the database
                    userDTO.DepartmentId = user.DepartmentId;
                }
                _mapper.Map(userDTO, user);

                user.NormalizedEmail = user.Email.ToUpper();
                user.NormalizedUserName = user.Email.ToUpper();
                user.UserName = user.Email;
                user.NormalizedEmail = user.Email.ToUpper();

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "UserProfilePicture")
                                ?? new AttachmentType { Name = "UserProfilePicture" };

                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                if (userDTO.ProfilePictures?.Any() == true)
                {
                    // Clear existing profile pictures from the linking table
                    if (user.ProfilePictures?.Any() == true)
                    {
                        foreach (var picture in user.ProfilePictures.ToList())
                        {
                            await _repositoryManager.AttachmentRepository.DeleteAsync(picture);
                        }
                        user.ProfilePictures.Clear();
                    }

                    // Save and add new profile picture
                    var attachmentsList = await SaveProfilePicturesAsync(userDTO.ProfilePictures, attachmentType.Id);
                    user.ProfilePictures = new List<Attachment>(attachmentsList);
                }

                await _repositoryManager.UserRepository.UpdateAsync(user);

                if(userDTO.RoleName != null)
                {
                    var roleResult = await _repositoryManager.UserRepository.AddUserToRoleAsync(user, userDTO.RoleName);

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("Role", "Failed to associate the user with the specified role.");
                        return BadRequest(ModelState);
                    }                    
                }

                await _unitOfWork.CommitAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetSingleUser(id);
                if (user == null)
                {
                    return NotFound();
                }

                //if the user is a member, delete the member record
                if (user.Status == Lambda.Active)
                {
                    var member = await _repositoryManager.MemberRepository.GetMemberByUserId(id);
                    if (member != null)
                    {
                        await _repositoryManager.MemberRepository.DeleteAsync(member);
                    }
                }

                await _repositoryManager.UserRepository.DeleteAsync(user);
                await _unitOfWork.CommitAsync();

                return Ok(_mapper.Map<ReadUserDTO>(user));
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("activate/{userId}")]
        public async Task<IActionResult> activate(string userId)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetSingleUserNoFilter(userId);
                if (user == null)
                {
                    return NotFound();
                }
               
                _repositoryManager.UserRepository.ActivateAccount(user);
                await _repositoryManager.UserRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

       private string FormatRoleName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return string.Empty;
            }

            return char.ToUpper(roleName[0]) + roleName.Substring(1).ToLower();
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
               
                // Fetch paginated users using the DepartmentRepository
                var users = await _repositoryManager.UserRepository.GetAllAsync();

                // Check if users exist
                if (users == null || !users.Any())
                {
                    return Ok(Enumerable.Empty<ReadUserDTO>()); // Return 404 Not Found if no departments are found
                }

                // Map users entities to ReadCountryDTO
                var mappedDepartments = _mapper.Map<IEnumerable<ReadUserDTO>>(users);

                return Ok(mappedDepartments); // Return paginated users

            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountUsers()
        {
            try
            {
                // Get the count of countries from the data layer
                var count = await _repositoryManager.UserRepository.GetUsersCountAsync();

                return Ok(count);
            }
            catch (Exception ex)
            {
                // Log the error and return an error response
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("signature")]
        public async Task<IActionResult> UpdateEmailSignature([FromForm] SignatureDTO signatureDTO)
        {
            try
            {
                var user = await Lambda.GetCurrentUser(_repositoryManager, _httpContextAccessor.HttpContext);
                
                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Signature") 
                                    ?? new AttachmentType { Name = "Signature" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                // Handle attachments
                if (signatureDTO.Attachments?.Any() == true)
                {
                    var attachmentsToUpdate = signatureDTO.Attachments.Where(a => a.Length > 0).ToList();
                    if (attachmentsToUpdate.Any())
                    {
                        var attachmentsList = await SaveAttachmentsAsync(attachmentsToUpdate, attachmentType.Id);
                        signatureDTO.BannerImageUrl = "Uploads/SignatureAttachments/"+attachmentsList.FirstOrDefault()?.FileName;
                    }
                }

                // Store signature data
                user.SignatureData = JsonSerializer.Serialize(signatureDTO);
                await _repositoryManager.UserRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
                
                return Ok("Email signature updated successfully");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "An error occurred while updating the signature.");
            }
        }

        private async Task<List<Attachment>> SaveAttachmentsAsync(IEnumerable<IFormFile> attachments, int attachmentTypeId)
        {
            var attachmentsList = new List<Attachment>();
            var hostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var webRootPath = hostEnvironment.WebRootPath;

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null or empty");
            }

            var attachmentsPath = Path.Combine(webRootPath, "Uploads/SignatureAttachments");

            if (!Directory.Exists(attachmentsPath))
            {
                Directory.CreateDirectory(attachmentsPath);
            }

            foreach (var attachment in attachments)
            {
                if (attachment == null || string.IsNullOrWhiteSpace(attachment.FileName))
                    continue;

                var uniqueFileName = FileNameGenerator.GenerateUniqueFileName(attachment.FileName);
                var filePath = Path.Combine(attachmentsPath, uniqueFileName);

                try
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await attachment.CopyToAsync(stream);
                    }

                    attachmentsList.Add(new Attachment
                    {
                        FileName = uniqueFileName,
                        FilePath = filePath,
                        AttachmentTypeId = attachmentTypeId,
                        PropertyName = "Banner"
                    });
                }
                catch
                {
                    throw;
                }
            }

            return attachmentsList;
        }

        private async Task<List<Attachment>> SaveProfilePicturesAsync(IEnumerable<IFormFile> attachments, int attachmentTypeId)
        {
            var attachmentsList = new List<Attachment>();
            var hostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var webRootPath = hostEnvironment.WebRootPath;

            // Check if webRootPath is null or empty
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null or empty");
            }

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/UserProfilePictures");
            
            // Ensure the directory exists
            if (!Directory.Exists(AttachmentsPath))
            {
                Directory.CreateDirectory(AttachmentsPath);

            }

            foreach (var attachment in attachments)
            {
                if (attachment == null || string.IsNullOrWhiteSpace(attachment.FileName))
                {

                    continue;
                }

                var uniqueFileName = FileNameGenerator.GenerateUniqueFileName(attachment.FileName);
                var filePath = Path.Combine(AttachmentsPath, uniqueFileName);

                try
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await attachment.CopyToAsync(stream);
                    }

                    attachmentsList.Add(new Attachment
                    {
                        FileName = uniqueFileName,
                        FilePath = filePath,
                        AttachmentTypeId = attachmentTypeId,
                        PropertyName = attachment.Name
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return attachmentsList;
        }

        [HttpGet("signature")]
        public async Task<IActionResult> GetSignature()
        {
            try
            {
                var user = await Lambda.GetCurrentUser(_repositoryManager, _httpContextAccessor.HttpContext);
                if (string.IsNullOrEmpty(user.SignatureData))
                    return Ok(new SignatureDTO());
                    
                var signatureData = JsonSerializer.Deserialize<SignatureDTO>(user.SignatureData);
                if (!string.IsNullOrEmpty(signatureData.BannerImageUrl))
                {
                    signatureData.BannerImageUrl = signatureData.BannerImageUrl.StartsWith("http") 
                        ? signatureData.BannerImageUrl 
                        : $"{_configuration.GetValue<string>("AppSettings:APP_URL")?.TrimEnd('/')}/{signatureData.BannerImageUrl.TrimStart('/')}";
                }
                
                return Ok(signatureData);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "An error occurred while fetching the signature.");
            }
        }

        [HttpGet("signature/html")]
        public async Task<IActionResult> GetFormattedSignature()
        {
            try
            {
                // Get user first and complete that operation
                var user =  _repositoryManager.UserRepository.FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name).Result;
                if (user == null || string.IsNullOrEmpty(user.SignatureData))
                {
                    return Ok(string.Empty);
                }
                
                // Then process the signature data
                var signatureData = JsonSerializer.Deserialize<SignatureDTO>(user.SignatureData);
                var htmlSignature = await Task.Run(() => _signatureService.GenerateSignatureHtml(signatureData));
                
                return Ok(new { html = htmlSignature });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "An error occurred while generating the signature HTML.");
            }
        }

        [HttpGet("signature/html/{userId}")]
        public async Task<IActionResult> GetUserFormattedSignature(string userId)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetSingleUser(userId);
                if (user == null)
                    return NotFound("User not found");
                    
                if (string.IsNullOrEmpty(user.SignatureData))
                    return Ok(string.Empty);
                    
                var signatureData = JsonSerializer.Deserialize<SignatureDTO>(user.SignatureData);
                var htmlSignature = _signatureService.GenerateSignatureHtml(signatureData);
                
                return Ok(new { html = htmlSignature });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "An error occurred while generating the signature HTML.");
            }
        }

        [HttpDelete("signature/banner")]
        public async Task<IActionResult> RemoveSignatureBanner()
        {
            try
            {
                var user = await Lambda.GetCurrentUser(_repositoryManager, _httpContextAccessor.HttpContext);
                if (string.IsNullOrEmpty(user.SignatureData))
                    return NotFound("No signature found");

                var signatureData = JsonSerializer.Deserialize<SignatureDTO>(user.SignatureData);
                
                // Remove banner image file if it exists
                if (!string.IsNullOrEmpty(signatureData.BannerImageUrl))
                {
                    var hostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
                    var filePath = Path.Combine(hostEnvironment.WebRootPath, signatureData.BannerImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Clear banner URL from signature data
                signatureData.BannerImageUrl = null;
                user.SignatureData = JsonSerializer.Serialize(signatureData);
                
                await _repositoryManager.UserRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();

                return Ok("Banner removed successfully");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "An error occurred while removing the signature banner.");
            }
        }

    }

    
}