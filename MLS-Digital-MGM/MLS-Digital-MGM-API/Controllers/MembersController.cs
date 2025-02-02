using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataStore.Core.DTOs;
using DataStore.Core.Services;
using DataStore.Persistence.Interfaces;
using DataStore.Core.Services.Interfaces;
using DataStore.Core.Models;
using DataStore.Core.DTOs.Member;
using DataStore.Helpers;
using System.Linq.Expressions;
using MLS_Digital_MGM.DataStore.Helpers;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Authorization;
using DataStore.Data;
using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Cors;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowFrontend")]  // Add this attribute
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MembersController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public MembersController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, IEmailService emailService, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetMembers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                var pagingParameters = new PagingParameters<Member>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Member, object>>[] {
                        p => p.Customer,
                        p => p.User,
                        p => p.Firm,

                    }
                };

                var members = await _repositoryManager.MemberRepository.GetPagedAsync(pagingParameters);

                if (members == null || !members.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadMemberDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadMemberDTO>());
                }

                var mappedMembers = _mapper.Map<List<ReadMemberDTO>>(members);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedMembers.Count;
                    var totalRecords = await _repositoryManager.MemberRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = mappedMembers.ToList()
                    });
                }

                return Ok(mappedMembers);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] CreateMemberDTO memberDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var member = _mapper.Map<Member>(memberDTO);

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);


                var existingMember = await _repositoryManager.MemberRepository.GetAsync(m => m.UserId == user.Id);
                if (existingMember != null)
                {
                    ModelState.AddModelError(nameof(memberDTO.UserId), $"A member already exists associated with the user @{user.Email}.");
                    return BadRequest(ModelState);
                }

                await _repositoryManager.MemberRepository.AddAsync(member);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetMember", new { id = member.Id }, member);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] UpdateMemberDTO memberDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var member = await _repositoryManager.MemberRepository.GetByIdAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                _mapper.Map(memberDTO, member);

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

                await _repositoryManager.MemberRepository.UpdateAsync(member);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                var member = await _repositoryManager.MemberRepository.GetByIdAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                await _repositoryManager.MemberRepository.DeleteAsync(member);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            try
            {
                var member = await _repositoryManager.MemberRepository.GetByIdAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                var mappedMember = _mapper.Map<ReadMemberDTO>(member);
                return Ok(mappedMember);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getByUserId/{id}")]
        public async Task<IActionResult> GetMemberByUserId(string id)
        {
            try
            {
                var member = await _repositoryManager.MemberRepository.GetMemberByUserId(id);
                if (member == null)
                {
                    return NotFound();
                }

                foreach (var attachment in member.User.ProfilePictures)
                {
                    //string attachmentTypeName = attachment.AttachmentType.Name;

                    string newFilePath = $"{Lambda.http}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/UserProfilePictures/{attachment.FileName}";

                    attachment.FilePath = newFilePath;

                }

                var mappedMember = _mapper.Map<ReadMemberDTO>(member);
                return Ok(mappedMember);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getFirmMembers")]
        public async Task<IActionResult> GetFirmMembers()
        {
            try
            {
                // Get the current user's email from the token
                var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User not authenticated");
                }

                // Get the current user's member record
                var currentMember = await _repositoryManager.MemberRepository.GetMemberByUserId(
                    (await _repositoryManager.UserRepository.FindByEmailAsync(userEmail)).Id);

                if (currentMember == null)
                {
                    return NotFound("Member record not found");
                }

                if (!currentMember.FirmId.HasValue)
                {
                    return NotFound("Member is not associated with any firm");
                }

                // Get all members from the same firm
                var firmMembers = await _repositoryManager.MemberRepository.GetMembersByFirmIdAsync(currentMember.FirmId.Value);

                var mappedMembers = _mapper.Map<List<ReadMemberDTO>>(firmMembers);
                return Ok(mappedMembers);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var memberRecords = await this._repositoryManager.MemberRepository.GetAllAsync();

                var readMemberRecordsMapped = this._mapper.Map<List<ReadMemberDTO>>(memberRecords);

                return Ok(readMemberRecordsMapped);
            }
            catch (Exception ex)
            {

                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> count()
        {
            try
            {
                var count = await _repositoryManager.MemberRepository.GetMembersCountAsync();

                return Ok(count);
            }
            catch (Exception ex)
            {

                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("countLicensed")]
        public async Task<IActionResult> CountLicensed()
        {
            try
            {
                //get current year of operation
                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();
                var count = await _repositoryManager.MemberRepository.GetLicensedMembersCountAsync(yearOfOperation.Id);

                return Ok(count);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        //get count of unlicensedMembers
        [HttpGet("countUnlicensed")]
        public async Task<IActionResult> CountUnlicensed()
        {
            try
            {
                //get current year of operation
                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();
                var count = await _repositoryManager.MemberRepository.GetUnlicensedMembersCountAsync(yearOfOperation.Id);

                return Ok(count);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getLicensedMembers")]
        public async Task<IActionResult> GetLicensedMembers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                //get current year of operation
                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Member>
                {
                    Predicate = u => u.Status != Lambda.Deleted && _context.Licenses.Any(l => l.MemberId == u.Id && l.Status == Lambda.Active && l.YearOfOperationId == yearOfOperation.Id),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Member, object>>[] {
                        p => p.Customer,
                        p => p.User,
                        p => p.Licenses,
                        p => p.Firm,
                    }
                };

                var members = await _repositoryManager.MemberRepository.GetPagedAsync(pagingParameters);

                if (members == null || !members.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadMemberDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadMemberDTO>());
                }

                var mappedMembers = _mapper.Map<List<ReadMemberDTO>>(members);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedMembers.Count;
                    var totalRecords = await _repositoryManager.MemberRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = mappedMembers.ToList()
                    });
                }

                return Ok(mappedMembers);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getUnlicensedMembers")]
        public async Task<IActionResult> GetUnlicensedMembers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                //get current year of operation
                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Member>
                {
                    Predicate = u => u.Status != Lambda.Deleted && !_context.Licenses.Any(l => l.MemberId == u.Id && l.Status == Lambda.Active && l.YearOfOperationId == yearOfOperation.Id),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Member, object>>[] {
                        p => p.Customer,
                        p => p.User,
                        p => p.Firm,
                    }
                };

                var members = await _repositoryManager.MemberRepository.GetPagedAsync(pagingParameters);

                if (members == null || !members.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadMemberDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadMemberDTO>());
                }

                var mappedMembers = _mapper.Map<List<ReadMemberDTO>>(members);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedMembers.Count;
                    var totalRecords = await _repositoryManager.MemberRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = mappedMembers.ToList()
                    });
                }

                return Ok(mappedMembers);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("bulk-register")]
        public async Task<IActionResult> BulkRegisterMembers(IFormFile uploadedFile)
        {
            try
            {
                var secretariatUsers = await _repositoryManager.UserRepository.GetUsersByRoleAsync("Secretariat");


                if (uploadedFile == null || uploadedFile.Length == 0)
                    return BadRequest("File is empty or not provided.");

                var extension = Path.GetExtension(uploadedFile.FileName).ToLower();
                if (extension != ".xlsx" && extension != ".xls")
                    return BadRequest("Invalid file format. Only .xlsx and .xls files are allowed.");

                if (secretariatUsers == null || !secretariatUsers.Any())
                    return BadRequest("Secretariat users are required");

                var webRootPath = _webHostEnvironment.WebRootPath;
                var uploadsFolder = Path.Combine(webRootPath, "Uploads");

                // Ensure the Uploads folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Save the file to the Uploads folder
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(stream);
                }

                // Enqueue the background job
                BackgroundJob.Enqueue(() => ProcessBulkRegistration(filePath, secretariatUsers));

                return Ok("Bulk registration process has been started. You will receive an email when it's completed.");
            }catch(Exception ex){
                await _errorLogService.LogErrorAsync(ex);                
                return StatusCode(500, "Internal server error");
            }
        }

        [AutomaticRetry(Attempts = 3)]
        [NonAction]
        public async Task ProcessBulkRegistration(string filePath, List<ApplicationUser> secretariatUsers)
        {
            var result = new BulkRegistrationResult();
            var successfulRegistrations = new List<(ApplicationUser User, string Password)>();
            var errors = new ConcurrentBag<RegistrationError>();

            try
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();

                // Pre-fetch lookup data
                var departments = await _repositoryManager.DepartmentRepository.GetAllAsync();
                var identityTypes = await _repositoryManager.IdentityTypeRepository.GetAllAsync();
                var countries = await _repositoryManager.CountryRepository.GetAllAsync();
                var titles = (await _repositoryManager.TitleRepository.GetAllAsync()).ToList();

                using var package = new ExcelPackage(new FileInfo(filePath));
                var worksheet = package.Workbook.Worksheets[0];
                int lastRow = worksheet.Dimension.End.Row;

                var tasks = new List<Task>();

                for (int row = 2; row <= lastRow; row++)
                {
                    if (IsRowEmpty(worksheet, row)) continue;


                    try
                    {
                        var memberData = ExtractMemberData(worksheet, row);
                        if (memberData == null) continue;

                        if (!ValidateMemberData(memberData, out var validationErrors))
                        {
                            errors.Add(new RegistrationError { Row = row, Errors = validationErrors });
                            continue;
                        }

                        var existingUser = await _repositoryManager.UserRepository.FindByEmailAsync(memberData.Email);
                        if (existingUser != null)
                        {
                            errors.Add(new RegistrationError { Row = row, Errors = new List<string> { "Email already exists" } });
                            continue;
                        }

                        var department = departments.FirstOrDefault(d => d.Name == Lambda.MemberDepartment);
                        if (department == null)
                        {
                            errors.Add(new RegistrationError { Row = row, Errors = new List<string> { "Invalid Department" } });
                            continue;
                        }

                        var identityType = identityTypes.FirstOrDefault(i => i.Name == memberData.IdentityType);
                        var country = countries.FirstOrDefault(c => c.Name.Equals(string.IsNullOrWhiteSpace(memberData.Country) ? "Malawi" : memberData.Country, StringComparison.OrdinalIgnoreCase));
                        if (country == null)
                        {
                            errors.Add(new RegistrationError { Row = row, Errors = new List<string> { $"Invalid Country: {memberData.Country}" } });
                            continue;
                        }

                        var title = titles.FirstOrDefault(t => t.Name.Equals(memberData.Title, StringComparison.OrdinalIgnoreCase))
                                    ?? new Title { Name = memberData.Title };

                        if (title.Id == 0)
                        {
                            await _repositoryManager.TitleRepository.AddAsync(title);
                            titles.Add(title);
                        }

                        var password = $"M@l@wi{DateTime.Now.Year}#L@wS0c!ety"; // Fixed, strong password with dynamic year
                        var user = new ApplicationUser
                        {
                            UserName = memberData.Email,
                            Email = memberData.Email,
                            FirstName = memberData.FirstName,
                            LastName = memberData.LastName,
                            OtherName = memberData.OtherName,
                            Gender = memberData.Gender,
                            PhoneNumber = memberData.PhoneNumber,
                            IdentityNumber = memberData.IdentityNumber,
                            IdentityExpiryDate = string.IsNullOrEmpty(memberData.IdentityExpiryDate) ? (DateTime?)null : DateTime.Parse(memberData.IdentityExpiryDate),
                            DateOfBirth = string.IsNullOrEmpty(memberData.DateOfBirth) ? (DateTime?)null : DateTime.Parse(memberData.DateOfBirth),
                            DepartmentId = department.Id,
                            IdentityTypeId = identityType?.Id,
                            TitleId = title.Id,
                            CountryId = country?.Id,
                            Status = Lambda.Active,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            EmailConfirmed = true,
                        };

                        var createUserResult = await _repositoryManager.UserRepository.AddAsync(user, password);
                        if (!createUserResult.Succeeded)
                        {
                            errors.Add(new RegistrationError { Row = row, Errors = createUserResult.Errors.Select(e => e.Description).ToList() });
                            continue;
                        }

                        var roleResult = await _repositoryManager.UserRepository.AddUserToRoleAsync(user, "Member");
                        if (!roleResult.Succeeded)
                        {
                            errors.Add(new RegistrationError { Row = row, Errors = new List<string> { "Failed to assign Member role." } });
                            continue;
                        }

                        var member = new Member
                        {
                            UserId = user.Id,
                            PostalAddress = memberData.PostalAddress,
                            PermanentAddress = memberData.PermanentAddress,
                            ResidentialAddress = memberData.ResidentialAddress,
                            DateOfAdmissionToPractice = DateTime.Parse(memberData.DateOfAdmissionToPractice),
                            Status = Lambda.Active
                        };

                        var memberQualification = new MemberQualification
                        {
                            Name = memberData.EducationQualification,
                            IssuingInstitution = memberData.Institution,
                            DateObtained = string.IsNullOrEmpty(memberData.DateQualificationObtained) ? (DateTime?)null : DateTime.Parse(memberData.DateQualificationObtained),
                            Member = member
                        };

                        member.QualificationTypes = new List<QualificationType> { new QualificationType { Name = memberData.EducationQualification } };

                        await _repositoryManager.MemberRepository.AddAsync(member);
                        successfulRegistrations.Add((user, password));

                        // Check for missing optional fields
                        var missingFields = CheckMissingOptionalFields(memberData);
                        if (missingFields.Any())
                        {
                            await _emailService.QueueEmailAsync(user.Email, "Please Update Your Member Profile", GenerateMissingFieldsEmailBody(missingFields), "MissingFields");
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new RegistrationError { Row = row, Errors = new List<string> { ex.Message } });
                    }

                }



                await _unitOfWork.CommitAsync();

                // Bulk update the database
                await _unitOfWork.SaveChangesAsync();

                // Enqueue email sending
                foreach (var (user, password) in successfulRegistrations)
                {
                    await _emailService.QueueEmailAsync(user.Email, "Welcome to Malawi Law Society - Your Login Details", GenerateWelcomeEmailBody(user, password), "Welcome");
                }

                // Schedule the email processing job
                BackgroundJob.Schedule(() => ProcessEmailQueue(), TimeSpan.FromMinutes(5));

                // Update result with successful registrations count
                result.SuccessfulRegistrations = successfulRegistrations.Count;

                // Send completion emails
                foreach (var user in secretariatUsers)
                {
                    await SendCompletionEmailAsync(user?.Email, result);
                }

                // Log errors
                result.Errors = errors.ToList();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                await _errorLogService.LogErrorAsync(ex);
                foreach (var user in secretariatUsers)
                {
                    await SendErrorEmailAsync(user?.Email, "An error occurred while processing the file");
                }
            }
            finally
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
        [NonAction]
        public async Task SendCompletionEmailAsync(string? email, BulkRegistrationResult result)
        {
            if (string.IsNullOrWhiteSpace(email)) return;

            var subject = "Bulk Member Registration Completed";
            var body = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Bulk Member Registration Completed</title>
                </head>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='background-color: #f8f8f8; border: 1px solid #ddd; border-radius: 5px; padding: 20px;'>
                        <h2 style='color: #0066cc; margin-top: 0;'>Bulk Member Registration Completed</h2>
                        <p>Dear Administrator,</p>
                        <p>We are pleased to inform you that the bulk member registration process has been successfully completed.</p>
                        <p style='background-color: #e6f3ff; border-left: 4px solid #0066cc; padding: 10px;'>
                            <strong>Successful Registrations:</strong> {result.SuccessfulRegistrations}
                        </p>
                        <p>For a detailed overview of the registration process, including any potential issues or discrepancies, please log in to the system and review the complete report.</p>
                        <p>If you have any questions or concerns, please don't hesitate to contact the IT support team.</p>
                        <p>Best regards,<br>Malawi Law Society</p>
                    </div>
                </body>
                </html>";

            await _emailService.SendMailWithKeyVarReturn(email, subject, body);
        }

        [NonAction]
        public async Task SendErrorEmailAsync(string? email, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(email)) return;

            var subject = "Error in Bulk Member Registration";
            var body = $"An error occurred during the bulk member registration process:\n\n{errorMessage}";

            await _emailService.SendMailWithKeyVarReturn(email, subject, body);
        }

        private MemberData ExtractMemberData(ExcelWorksheet worksheet, int row)
        {
            var data = new MemberData
            {
                FirstName = worksheet.Cells[row, 1].Value?.ToString().Trim(),
                LastName = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                OtherName = worksheet.Cells[row, 3].Value?.ToString(),
                Title = worksheet.Cells[row, 4].Value?.ToString(),
                Gender = worksheet.Cells[row, 5].Value?.ToString(),
                Email = worksheet.Cells[row, 6].Value?.ToString(),
                IdentityType = worksheet.Cells[row, 7].Value?.ToString(),
                IdentityNumber = worksheet.Cells[row, 8].Value?.ToString(),
                IdentityExpiryDate = GetDateTimeValue(worksheet.Cells[row, 9].Value),
                Country = worksheet.Cells[row, 10].Value?.ToString(),
                PhoneNumber = worksheet.Cells[row, 11].Value?.ToString(),
                DateOfBirth = GetDateTimeValue(worksheet.Cells[row, 12].Value),
                PostalAddress = worksheet.Cells[row, 13].Value?.ToString(),
                PermanentAddress = worksheet.Cells[row, 14].Value?.ToString(),
                ResidentialAddress = worksheet.Cells[row, 15].Value?.ToString(),
                EducationQualification = worksheet.Cells[row, 16].Value?.ToString(),
                Institution = worksheet.Cells[row, 17].Value?.ToString(),
                DateQualificationObtained = GetDateTimeValue(worksheet.Cells[row, 18].Value),
                DateOfAdmissionToPractice = GetDateTimeValue(worksheet.Cells[row, 19].Value)
            };

            // Check if essential fields are empty
            if (string.IsNullOrWhiteSpace(data.FirstName) ||
                string.IsNullOrWhiteSpace(data.LastName) ||
                string.IsNullOrWhiteSpace(data.Email))
            {
                return null; // Return null for invalid data
            }

            return data;
        }

        private string GetDateTimeValue(object cellValue)
        {
            if (cellValue == null)
                return null;

            if (cellValue is DateTime dateTime)
                return dateTime.ToString("yyyy-MM-dd");

            if (double.TryParse(cellValue.ToString(), out double excelDate))
                return DateTime.FromOADate(excelDate).ToString("yyyy-MM-dd");

            return cellValue.ToString();
        }

        private bool ValidateMemberData(MemberData data, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(data.FirstName))
                errors.Add("First Name is required");
            if (string.IsNullOrWhiteSpace(data.LastName))
                errors.Add("Last Name is required");
            if (string.IsNullOrWhiteSpace(data.Email) || !IsValidEmail(data.Email))
                errors.Add("Valid Email is required");
            if (string.IsNullOrWhiteSpace(data.Title))
                errors.Add("Title is required");
            if (string.IsNullOrWhiteSpace(data.Gender))
                errors.Add("Gender is required");
            if (string.IsNullOrWhiteSpace(data.PostalAddress))
                errors.Add("Postal Address is required");
            if (string.IsNullOrWhiteSpace(data.EducationQualification))
                errors.Add("Education Qualification is required");
            if (string.IsNullOrWhiteSpace(data.Institution))
                errors.Add("Institution is required");
            if (string.IsNullOrWhiteSpace(data.DateOfAdmissionToPractice) || !DateTime.TryParse(data.DateOfAdmissionToPractice, out _))
                errors.Add("Valid Date of Admission to Practice is required");

            if (!string.IsNullOrWhiteSpace(data.PhoneNumber) && !IsValidPhoneNumber(data.PhoneNumber))
                errors.Add("Invalid Phone Number format");

            return errors.Count == 0;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$");
        }

        // private string GenerateRandomPassword()
        // {
        //     const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //     const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        //     const string digits = "0123456789";
        //     const string special = "!@#$%^&*()";
        //     var random = new Random();

        //     var password = new StringBuilder();
        //     password.Append(upperCase[random.Next(upperCase.Length)]);
        //     password.Append(lowerCase[random.Next(lowerCase.Length)]);
        //     password.Append(digits[random.Next(digits.Length)]);
        //     password.Append(special[random.Next(special.Length)]);

        //     const string allChars = upperCase + lowerCase + digits + special;
        //     for (int i = 4; i < 12; i++)
        //     {
        //         password.Append(allChars[random.Next(allChars.Length)]);
        //     }

        //     return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
        // }

        public class MemberData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Title { get; set; }
            public string Gender { get; set; }
            public string PostalAddress { get; set; }
            public string EducationQualification { get; set; }
            public string Institution { get; set; }
            public string DateOfAdmissionToPractice { get; set; }
            public string OtherName { get; set; }
            public string IdentityType { get; set; }
            public string IdentityNumber { get; set; }
            public string IdentityExpiryDate { get; set; }
            public string Country { get; set; }
            public string PhoneNumber { get; set; }
            public string DateOfBirth { get; set; }
            public string PermanentAddress { get; set; }
            public string ResidentialAddress { get; set; }
            public string DateQualificationObtained { get; set; }
        }

        public class BulkRegistrationResult
        {
            public int SuccessfulRegistrations { get; set; }
            public List<RegistrationError> Errors { get; set; } = new List<RegistrationError>();
        }

        public class RegistrationError
        {
            public int Row { get; set; }
            public List<string> Errors { get; set; }
        }

        private List<string> CheckMissingOptionalFields(MemberData memberData)
        {
            var missingFields = new List<string>();

            if (string.IsNullOrWhiteSpace(memberData.OtherName)) missingFields.Add("Other Name");
            if (string.IsNullOrWhiteSpace(memberData.IdentityType)) missingFields.Add("Identity Type");
            if (string.IsNullOrWhiteSpace(memberData.IdentityNumber)) missingFields.Add("Identity Number");
            if (string.IsNullOrWhiteSpace(memberData.IdentityExpiryDate)) missingFields.Add("Identity Expiry Date");
            if (string.IsNullOrWhiteSpace(memberData.Country)) missingFields.Add("Country");
            if (string.IsNullOrWhiteSpace(memberData.PhoneNumber)) missingFields.Add("Phone Number");
            if (string.IsNullOrWhiteSpace(memberData.DateOfBirth)) missingFields.Add("Date of Birth");
            if (string.IsNullOrWhiteSpace(memberData.PermanentAddress)) missingFields.Add("Permanent Address");
            if (string.IsNullOrWhiteSpace(memberData.ResidentialAddress)) missingFields.Add("Residential Address");
            if (string.IsNullOrWhiteSpace(memberData.DateQualificationObtained)) missingFields.Add("Date Qualification Obtained");

            return missingFields;
        }

        [NonAction]
        public async Task SendMissingFieldsEmailAsync(string email, List<string> missingFields)
        {
            var subject = "Please Update Your Member Profile";
            var body = $@"Dear Member,

            Welcome to Malawi Law Society. We noticed that some information is missing from your profile. 
            Please log in and update the following fields:

            {string.Join(Environment.NewLine, missingFields.Select(field => $"• {field}"))}

            Updating this information will help us serve you better.

            Thank you for your cooperation.";

            await _emailService.SendMailWithKeyVarReturn(email, subject, body);
        }
        [NonAction]
        public async Task SendWelcomeEmailAsync(ApplicationUser user, string password)
        {
            // Send login details email
            string passwordBody = $@"Dear Member,

            Welcome to Malawi Law Society!

            Your account has been successfully created. Here are your login details:

            Email: {user.Email}
            Password: {password}

            To access your account, please visit our member portal at:
            https://members.malawilawsociety.net

            For security reasons, we recommend changing your password after your first login.

            If you have any questions or need assistance, please don't hesitate to contact our support team.

            Best regards,
            Malawi Law Society";

            BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(user.Email, "Welcome to Malawi Law Society - Your Login Details", passwordBody, false));

            // // Send OTP email
            // string pinBody = $"An account has been created on Malawi Law Society. Your OTP is {user.Pin} <br /> Enter the OTP to activate your account <br /> You can activate your account by clicking <a href='https://members.malawilawsociety.net'>here</a>";
            // BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(user.Email, "Account Activation", pinBody));
        }

        private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            return worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column].All(c => c.Value == null || string.IsNullOrWhiteSpace(c.Value.ToString()));
        }

        private string GenerateWelcomeEmailBody(ApplicationUser user, string password)
        {
            // Generate the welcome email body
            return $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Welcome to Malawi Law Society</title>
                </head>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <div style='background-color: #f8f8f8; border: 1px solid #ddd; border-radius: 5px; padding: 20px;'>
                        <h2 style='color: #0066cc; margin-top: 0;'>Welcome to Malawi Law Society!</h2>
                        <p>Dear Member,</p>
                        <p>Your account has been successfully created. Here are your login details:</p>
                        <p style='background-color: #e6f3ff; border-left: 4px solid #0066cc; padding: 10px;'>
                            <strong>Email:</strong> {user.Email}<br>
                            <strong>Password:</strong> {password}
                        </p>
                        <p>To access your account, please visit our member portal at:<br>
                        <a href='https://members.malawilawsociety.net' style='color: #0066cc;'>https://members.malawilawsociety.net</a></p>
                        <p><strong>Important:</strong> For security reasons, we strongly recommend changing your password after your first login.</p>
                        <p>If you have any questions or need assistance, please don't hesitate to contact our support team.</p>
                        <p>Best regards,<br>Malawi Law Society</p>
                    </div>
                </body>
                </html>";
        }

        private string GenerateMissingFieldsEmailBody(List<string> missingFields)
        {
            // Generate the missing fields email body
            return $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Profile Update Request - Malawi Law Society</title>
            </head>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;'>
                <div style='background-color: #f8f8f8; border: 1px solid #ddd; border-radius: 5px; padding: 20px;'>
                    <h2 style='color: #0066cc; margin-top: 0;'>Welcome to Malawi Law Society</h2>
                    <p>Dear Member,</p>
                    <p>We hope this email finds you well. We've noticed that some information is missing from your profile. To ensure we can provide you with the best possible service, we kindly request that you log in and update the following fields:</p>
                    <ul style='background-color: #e6f3ff; border-left: 4px solid #0066cc; padding: 10px;'>
                        {string.Join("", missingFields.Select(field => $"<li>{field}</li>"))}
                    </ul>
                    <p>Updating this information will help us serve you more effectively and ensure you receive all relevant communications.</p>
                    <p>To update your profile, please follow these steps:</p>
                    <ol>
                        <li>Visit our member portal at <a href='https://members.malawilawsociety.net' style='color: #0066cc;'>https://members.malawilawsociety.net</a></li>
                        <li>Log in to your account</li>
                        <li>Navigate to your profile settings</li>
                        <li>Update the missing information</li>
                        <li>Save your changes</li>
                    </ol>
                    <p>If you encounter any issues or need assistance, please don't hesitate to contact our support team.</p>
                    <p>Thank you for your prompt attention to this matter. Your cooperation is greatly appreciated.</p>
                    <p>Best regards,<br>Malawi Law Society</p>
                </div>
            </body>
            </html>";
        }

        [AutomaticRetry(Attempts = 3)]
        [NonAction]
        public async Task ProcessEmailQueue()
        {
            await _emailService.ProcessEmailQueueAsync();
        }
    }
}