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

namespace MLS_Digital_MGM_API.Controllers 
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class MembersController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        
        public MembersController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, IEmailService emailService )
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _emailService = emailService;
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

                var mappedMember = _mapper.Map<ReadMemberDTO>(member);
                return Ok(mappedMember);
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
                var memberRecords =  await this._repositoryManager.MemberRepository.GetAllAsync();

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

        
        [HttpGet("getLicensedMembers")]
        public async Task<IActionResult> GetLicensedMembers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Member>
                {
                    Predicate = u => u.Status != Lambda.Deleted && _context.Licenses.Any(l => l.MemberId == u.Id),
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

        [HttpGet("getUnlicensedMembers")]
        public async Task<IActionResult> GetUnlicensedMembers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Member>
                {
                    Predicate = u => u.Status != Lambda.Deleted && !_context.Licenses.Any(l => l.MemberId == u.Id),
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
            //get users who have the secretarial role
            var secretariatUsers = await _repositoryManager.UserRepository.GetUsersByRoleAsync("Secretariat");
            

            if (uploadedFile == null || uploadedFile.Length == 0)
                return BadRequest("File is empty");

            var extension = Path.GetExtension(uploadedFile.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls")
                return BadRequest("Invalid file format. Only .xlsx and .xls files are allowed.");

            if (secretariatUsers == null || !secretariatUsers.Any())
                return BadRequest("Secretariat users are required");

            // Save the file to a temporary location
            var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            // Enqueue the background job
            BackgroundJob.Enqueue(() => ProcessBulkRegistration(tempFilePath, secretariatUsers));

            return Ok("Bulk registration process has been started. You will receive an email when it's completed.");
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ProcessBulkRegistration(string filePath, List<ApplicationUser> secretariatUsers)
        {
            var result = new BulkRegistrationResult();
            var successfulRegistrations = new List<(ApplicationUser User, string Password)>();

            try
            {
              
                using var package = new ExcelPackage(new FileInfo(filePath));
                var worksheet = package.Workbook.Worksheets[0];

                    int lastRow = worksheet.Dimension.End.Row;
                    for (int row = 2; row <= lastRow; row++)
                    {
                        if (IsRowEmpty(worksheet, row)) continue;

                        try
                        {
                            await using var transaction = await _unitOfWork.BeginTransactionAsync();
                            
                            var memberData = ExtractMemberData(worksheet, row);
                            if (memberData == null) continue;

                            if (!ValidateMemberData(memberData, out var validationErrors))
                            {
                                result.Errors.Add(new RegistrationError { Row = row, Errors = validationErrors });
                                continue;
                            }

                            var existingUser = await _repositoryManager.UserRepository.FindByEmailAsync(memberData.Email);
                            if (existingUser != null)
                            {
                                result.Errors.Add(new RegistrationError { Row = row, Errors = new List<string> { "Email already exists" } });
                                continue;
                            }

                            var department = await _repositoryManager.DepartmentRepository.GetAsync(d => d.Name == Lambda.MemberDepartment);
                            if (department == null)
                            {
                                result.Errors.Add(new RegistrationError { Row = row, Errors = new List<string> { "Invalid Department ID" } });
                                continue;
                            }

                            var identityType = await _repositoryManager.IdentityTypeRepository.GetAsync(i => i.Name == memberData.IdentityType);
                           
                            var countryName = string.IsNullOrWhiteSpace(memberData.Country) ? "Malawi" : memberData.Country;
                            var country = await _repositoryManager.CountryRepository.GetAsync(c => c.Name.ToLower() == countryName.ToLower());
                            if (country == null)
                            {
                                result.Errors.Add(new RegistrationError { Row = row, Errors = new List<string> { $"Invalid Country: {countryName}" } });
                                continue;
                            }

                            var title = await _repositoryManager.TitleRepository.GetAsync(t => t.Name.ToLower() == memberData.Title.ToLower());
                            if (title == null)
                            {
                                title = new Title { Name = memberData.Title };
                                await _repositoryManager.TitleRepository.AddAsync(title);
                            }

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

                            var password = GenerateRandomPassword();
                            var createUserResult = await _repositoryManager.UserRepository.AddAsync(user, password);
                            if (!createUserResult.Succeeded)
                            {
                                result.Errors.Add(new RegistrationError { Row = row, Errors = createUserResult.Errors.Select(e => e.Description).ToList() });
                                await _unitOfWork.RollbackAsync();
                                continue;
                            }

                            var roleResult = await _repositoryManager.UserRepository.AddUserToRoleAsync(user, "Member");
                            if (!roleResult.Succeeded)
                            {
                                result.Errors.Add(new RegistrationError { Row = row, Errors = new List<string> { "Failed to associate the user with the Member role." } });
                                await _unitOfWork.RollbackAsync();
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
                            await _unitOfWork.SaveChangesAsync();
                            await _unitOfWork.CommitAsync();

                            result.SuccessfulRegistrations++;
                            successfulRegistrations.Add((user, password));

                            var missingOptionalFields = CheckMissingOptionalFields(memberData);
                            if (missingOptionalFields.Any())
                            {
                                await SendMissingFieldsEmailAsync(user.Email, missingOptionalFields);
                            }
                        }
                        catch (Exception ex)
                        {
                            await _unitOfWork.RollbackAsync();
                            result.Errors.Add(new RegistrationError { Row = row, Errors = new List<string> { ex.Message } });
                        }
                    }
                

                foreach (var (user, password) in successfulRegistrations)
                {
                    await SendWelcomeEmailAsync(user, password);
                }

                foreach (var user in secretariatUsers)
                {
                    await SendCompletionEmailAsync(user?.Email, result);
                }
            }
            catch (Exception ex)
            {
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
        private async Task SendCompletionEmailAsync(string? email, BulkRegistrationResult result)
        {
            if (string.IsNullOrWhiteSpace(email)) return;

            var subject = "Bulk Member Registration Completed";
            var body = $"The bulk member registration process has completed.\n\n" +
                       $"Successful Registrations: {result.SuccessfulRegistrations}\n\n" +
                       $"Please check the system for more details.";

            await _emailService.SendMailWithKeyVarReturn(email, subject, body);
        }

        private async Task SendErrorEmailAsync(string? email, string errorMessage)
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

        private string GenerateRandomPassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()";
            var random = new Random();
            
            var password = new StringBuilder();
            password.Append(upperCase[random.Next(upperCase.Length)]);
            password.Append(lowerCase[random.Next(lowerCase.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(special[random.Next(special.Length)]);

            const string allChars = upperCase + lowerCase + digits + special;
            for (int i = 4; i < 12; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
        }

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

        private async Task SendMissingFieldsEmailAsync(string email, List<string> missingFields)
        {
            var subject = "Please Update Your Member Profile";
            var body = $"Dear Member,\n\nWelcome to our platform. We noticed that some optional information is missing from your profile. " +
                       $"Please log in and update the following fields:\n\n{string.Join("\n", missingFields)}\n\n" +
                       $"Updating this information will help us serve you better.\n\nThank you for your cooperation.";

            await _emailService.SendMailWithKeyVarReturn(email, subject, body);
        }

        private async Task SendWelcomeEmailAsync(ApplicationUser user, string password)
        {
            // Send login details email
            string passwordBody = $"Your account has been created on Malawi Law Society. Your login details are as follows: <br /> Email: {user.Email} <br /> Password: {password} <br /> Login to Malawi Law Society. Your password is {password}";
            BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(user.Email, "Login Details", passwordBody));

            // Send OTP email
            string pinBody = $"An account has been created on Malawi Law Society. Your OTP is {user.Pin} <br /> Enter the OTP to activate your account <br /> You can activate your account by clicking <a href='https://mls.sparcsystems.africa'>here</a>";
            BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(user.Email, "Account Activation", pinBody));
        }

        private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            return worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column].All(c => c.Value == null || string.IsNullOrWhiteSpace(c.Value.ToString()));
        }
    }
}