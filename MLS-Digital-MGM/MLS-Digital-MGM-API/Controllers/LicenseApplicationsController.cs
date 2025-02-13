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
using DataStore.Core.DTOs.LicenseApplication;
using Microsoft.Extensions.Hosting;
using DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using Hangfire;
using DataStore.Core.DTOs.License;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LicenseApplicationsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public LicenseApplicationsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _configuration = configuration;
        }

        // GET api/licenseapplications/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetLicenseApplications(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                // get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, (user.Id));

                var pagingParameters = new PagingParameters<LicenseApplication>();



                // Check if the user is secretariat and approve the application if so
                pagingParameters = new PagingParameters<LicenseApplication>
                {
                    Predicate = u => u.Status != Lambda.Deleted && ((!string.Equals(currentRole, "member", StringComparison.OrdinalIgnoreCase) && u.ApplicationStatus != Lambda.Draft && u.ApplicationStatus != Lambda.Approved && u.ApplicationStatus != Lambda.Denied) ||
                        (string.Equals(currentRole, "member", StringComparison.OrdinalIgnoreCase) && u.CreatedById == user.Id)),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : "dateSubmitted",
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : "desc",
                    Includes = new Expression<Func<LicenseApplication, object>>[] {
                        p => p.YearOfOperation,
                        p => p.Member,
                        p => p.CurrentApprovalLevel,
                        p => p.License
                    },
                    CreatedById = string.Equals(currentRole, "member", StringComparison.OrdinalIgnoreCase) ? CreatedById : null,
                };

                var licenseApplicationsPaged = await _repositoryManager.LicenseApplicationRepository.GetPagedAsync(pagingParameters);

                if (licenseApplicationsPaged == null || !licenseApplicationsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadLicenseApplicationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadLicenseApplicationDTO>()); // Return empty list
                }

                // Map the Roles to a list of ReadFirmDTOs
                var licenseApplicationFirms = _mapper.Map<List<ReadLicenseApplicationDTO>>(licenseApplicationsPaged);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = licenseApplicationFirms.Count;
                    var totalRecords = await _repositoryManager.LicenseApplicationRepository.CountAsync(pagingParameters);


                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = licenseApplicationFirms.ToList() // Materialize the enumerable
                    });
                }

                // Return an Ok result with the mapped Roles
                return Ok(licenseApplicationFirms);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("denied/paged")]
        public async Task<IActionResult> GetDeniedLicenseApplications(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                // get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, (user.Id));

                var pagingParameters = new PagingParameters<LicenseApplication>();



                // Check if the user is secretariat and approve the application if so
                pagingParameters = new PagingParameters<LicenseApplication>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.ApplicationStatus == Lambda.Denied,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : "dateSubmitted",
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : "desc",
                    Includes = new Expression<Func<LicenseApplication, object>>[] {
                        p => p.YearOfOperation,
                        p => p.Member,
                        p => p.CurrentApprovalLevel,
                        p => p.License
                    },
                    CreatedById = string.Equals(currentRole, "member", StringComparison.OrdinalIgnoreCase) ? CreatedById : null,
                };

                var licenseApplicationsPaged = await _repositoryManager.LicenseApplicationRepository.GetPagedAsync(pagingParameters);

                if (licenseApplicationsPaged == null || !licenseApplicationsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadLicenseApplicationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadLicenseApplicationDTO>()); // Return empty list
                }

                // Map the Roles to a list of ReadFirmDTOs
                var licenseApplicationFirms = _mapper.Map<List<ReadLicenseApplicationDTO>>(licenseApplicationsPaged);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = licenseApplicationFirms.Count;
                    var totalRecords = await _repositoryManager.LicenseApplicationRepository.CountAsync(pagingParameters);


                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = licenseApplicationFirms.ToList() // Materialize the enumerable
                    });
                }

                // Return an Ok result with the mapped Roles
                return Ok(licenseApplicationFirms);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("approved/paged")]
        public async Task<IActionResult> GetApprovedLicenseApplications(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                // get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, (user.Id));

                var pagingParameters = new PagingParameters<LicenseApplication>();


                pagingParameters = new PagingParameters<LicenseApplication>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.ApplicationStatus == Lambda.Approved,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : "dateSubmitted",
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : "desc",
                    Includes = new Expression<Func<LicenseApplication, object>>[] {
                        p => p.YearOfOperation,
                        p => p.Member,
                        p => p.CurrentApprovalLevel,
                        p => p.License
                    },
                    CreatedById = string.Equals(currentRole, "member", StringComparison.OrdinalIgnoreCase) ? CreatedById : null,
                };

                var licenseApplicationsPaged = await _repositoryManager.LicenseApplicationRepository.GetPagedAsync(pagingParameters);

                if (licenseApplicationsPaged == null || !licenseApplicationsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadLicenseApplicationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadLicenseApplicationDTO>()); // Return empty list
                }

                // Map the Roles to a list of ReadFirmDTOs
                var licenseApplicationFirms = _mapper.Map<List<ReadLicenseApplicationDTO>>(licenseApplicationsPaged);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = licenseApplicationFirms.Count;
                    var totalRecords = await _repositoryManager.LicenseApplicationRepository.CountAsync(pagingParameters);


                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = licenseApplicationFirms.ToList() // Materialize the enumerable
                    });
                }

                // Return an Ok result with the mapped Roles
                return Ok(licenseApplicationFirms);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/licenseapplications
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        [RequestSizeLimit(104857600)]
        [HttpPost]
        public async Task<IActionResult> AddLicenseApplication([FromForm] CreateLicenseApplicationDTO licenseApplicationDTO)
        {
            try
            {


                if (!licenseApplicationDTO.ActionType.Equals(Lambda.Draft, StringComparison.CurrentCultureIgnoreCase))
                {
                    //check if the Id has been submitted. If not the set it to zero

                    if (licenseApplicationDTO.Id == null)
                    {
                        licenseApplicationDTO.Id = 0;
                    }
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    licenseApplicationDTO.ApplicationStatus = Lambda.Pending;


                }
                else
                {
                    licenseApplicationDTO.ApplicationStatus = Lambda.Draft;
                }

                var application = _mapper.Map<LicenseApplication>(licenseApplicationDTO);

                if (!licenseApplicationDTO.ActionType.Equals(Lambda.Draft, StringComparison.CurrentCultureIgnoreCase))
                    application.DateSubmitted = DateTime.Now;


                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                application.CreatedById = user.Id;

                //get the current year of operation
                var currentYearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();

                var yearOfOperation = currentYearOfOperation;

                if (currentYearOfOperation != null)
                {
                    if (DateTime.Now.Month == currentYearOfOperation.EndDate.Month)
                    {
                        // Assign the next year of operation (current year + 1)
                        var nextYearOfOperation = await _repositoryManager.YearOfOperationRepository.GetNextYearOfOperation();
                        if (nextYearOfOperation != null)
                        {
                            application.YearOfOperationId = nextYearOfOperation.Id;
                            yearOfOperation = nextYearOfOperation;
                        }
                        else
                        {
                            return BadRequest("No next year of operation found or the current year of operation is not set");
                        }
                    }
                    else
                    {
                        application.YearOfOperationId = currentYearOfOperation.Id;
                    }

                }
                else
                {
                    ModelState.AddModelError("", "The current year of operation is not set");
                    return BadRequest(ModelState);
                }

                //get license approval level record by passing level
                var licenseApprovalLevel = await _repositoryManager.LicenseApprovalLevelRepository.GetLicenseApprovalLevelByLevel(1);

                if (licenseApprovalLevel is not null)
                {
                    application.CurrentApprovalLevelID = licenseApprovalLevel.Id;
                    licenseApplicationDTO.CurrentApprovalLevelID = licenseApprovalLevel.Id;

                }


                //get the id of the current member
                var currentMember = await _repositoryManager.MemberRepository.GetMemberByUserId(user.Id);

                if (currentMember != null)
                {
                    application.MemberId = currentMember.Id;

                    currentMember.FirmId = licenseApplicationDTO.FirmId;

                    await _repositoryManager.MemberRepository.UpdateAsync(currentMember);

                }

                //check if this is first application 
                var hasPreviousApplication = await _repositoryManager.LicenseApplicationRepository.HasPreviousApplicationsAsync(currentMember.Id);
                //set first application to true
                if (hasPreviousApplication is false)
                {
                    //check if the current member has a license
                    var license = await _repositoryManager.LicenseRepository.GetLicenseByMemberId(currentMember.Id);
                    if (license is null)
                    {
                        application.FirstApplicationForLicense = true;
                        application.RenewedLicensePreviousYear = false;

                    }
                    else
                    {
                        application.FirstApplicationForLicense = false;
                        application.RenewedLicensePreviousYear = true;
                    }
                }

                //create TODO code to check and set if the application has been created outside the allowed window


                // Check if a license application has been made in the same year and it is under review or hasn't been approved yet
                var existingApplication = await _repositoryManager.LicenseApplicationRepository.GetAsync(
                    a => a.YearOfOperationId == currentYearOfOperation.Id && a.MemberId == currentMember.Id &&
                    (a.ApplicationStatus == Lambda.UnderReview || a.ApplicationStatus == Lambda.Approved)
                );



                if (existingApplication != null)
                {
                    ModelState.AddModelError("", "You already have a license application in the same year and it is pending or approved");
                    return BadRequest(ModelState);
                }

                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == Lambda.LicenseApplication)
                                    ?? new AttachmentType { Name = Lambda.LicenseApplication };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                //get all the IForm File properties from the DTO
                var formFileProperties = GetFormFileProperties(licenseApplicationDTO);

                //check if it is a new application or an existing one
                if (application.Id == 0)
                {
                    // Save attachments if any
                    application.Attachments = formFileProperties.Any()
                        ? await SaveAttachmentsAsync((IEnumerable<IFormFile>)formFileProperties, attachmentType.Id)
                        : null;

                    // Add license application to repository
                    await _repositoryManager.LicenseApplicationRepository.AddAsync(application);
                }
                else
                {
                    existingApplication = await _repositoryManager.LicenseApplicationRepository.GetByIdAsync(application.Id);

                    if (existingApplication != null)
                    {
                        // Save attachments if any
                        if (formFileProperties.Any())
                        {

                            //get attachments from the LicenseApplicationDTO that have at least greater than zero kb
                            var attachmentsToUpdate = formFileProperties.Where(a => a.Length > 0).ToList();

                            //save attachments
                            var attachmentsList = await SaveAttachmentsAsync(attachmentsToUpdate, attachmentType.Id);


                            // Remove old attachments with the same name as the new ones
                            existingApplication.Attachments.RemoveAll(a => attachmentsToUpdate.Any(b => b.Name == a.PropertyName));


                            // Add fresh list of attachments
                            existingApplication.Attachments.AddRange(attachmentsList);
                        }

                        if (hasPreviousApplication is false)
                        {
                            //check if the current member has a license
                            var license = await _repositoryManager.LicenseRepository.GetLicenseByMemberId(currentMember.Id);
                            if (license is null)
                            {
                                existingApplication.FirstApplicationForLicense = true;
                                existingApplication.RenewedLicensePreviousYear = false;

                            }
                            else
                            {
                                existingApplication.FirstApplicationForLicense = false;
                                existingApplication.RenewedLicensePreviousYear = true;
                            }
                        }

                        //map update to existing application
                        _mapper.Map(licenseApplicationDTO, existingApplication);

                        if (!licenseApplicationDTO.ActionType.Equals(Lambda.Draft, StringComparison.CurrentCultureIgnoreCase))
                        {
                            existingApplication.DateSubmitted = DateTime.Now;
                            existingApplication.ApplicationStatus = Lambda.Pending;
                        }


                        // Update the application
                        await _repositoryManager.LicenseApplicationRepository.UpdateAsync(existingApplication);
                    }
                }


                // Update member firm details if the firmId is not null or 0
                if (licenseApplicationDTO.FirmId is not null && licenseApplicationDTO.FirmId != 0)
                {
                    if (currentMember is not null && currentMember.FirmId != licenseApplicationDTO.FirmId)
                    {
                        currentMember.FirmId = licenseApplicationDTO.FirmId;
                        await _repositoryManager.MemberRepository.UpdateAsync(currentMember);
                    }
                }

                await _unitOfWork.CommitAsync();

                if (!application.ApplicationStatus.Equals(Lambda.Draft, StringComparison.CurrentCultureIgnoreCase))
                {
                    // Send status details email
                    string emailBody = $"Your have made a license application for {yearOfOperation.StartDate.Year} - {yearOfOperation.EndDate.Year} practice year. You can view the status of your application by clicking the link below.<br/><br/><a href='https://members.malawilawsociety.net'>Click here to view your application</a>";
                    BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(user.Email, "Annual Membership Application Status", emailBody, false));
                }

                //only add application approval history if the application is not a draft
                if (!application.ApplicationStatus.Equals(Lambda.Draft, StringComparison.CurrentCultureIgnoreCase))
                {
                    //add application approval history
                    var applicationApprovalHistory = new LicenseApprovalHistory
                    {
                        LicenseApplicationId = application.Id,
                        ApprovalLevelId = licenseApprovalLevel.Id,
                        Status = application.ApplicationStatus,
                        ChangedById = user.Id,
                        CreatedDate = DateTime.Now,
                        ChangeDate = DateTime.Now
                    };

                    await _repositoryManager.LicenseApprovalHistoryRepository.AddAsync(applicationApprovalHistory);
                }



                // Return created ProBonoApplication

                await _unitOfWork.CommitAsync();




                // Return created ProBonoApplication
                return CreatedAtAction("GetLicenseApplications", new { id = application.Id }, application);
            }
            catch (Exception ex)
            {
                // Log error and return internal server error
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/licenseapplications/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicenseApplicationById(int id)
        {
            try
            {
                var licenseApplication = await _repositoryManager.LicenseApplicationRepository.GetByIdAsync(id);
                if (licenseApplication == null)
                {
                    return NotFound();
                }
                var licenseApplicationDTO = _mapper.Map<ReadLicenseApplicationDTO>(licenseApplication);


                foreach (var attachment in licenseApplicationDTO.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;

                    string newFilePath = Path.Combine($"{Lambda.https}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.LicenseApplicationFolderName}", attachment.FileName);

                    attachment.FilePath = newFilePath;
                }

                return Ok(licenseApplicationDTO);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        // PUT api/licenseapplications/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicenseApplication(int id, [FromBody] UpdateLicenseApplicationDTO updateLicenseApplicationDTO)
        {
            try
            {
                var licenseApplication = await _repositoryManager.LicenseApplicationRepository.GetByIdAsync(id);
                if (licenseApplication == null)
                {
                    return NotFound();
                }

                _mapper.Map(updateLicenseApplicationDTO, licenseApplication);

                await _repositoryManager.LicenseApplicationRepository.UpdateAsync(licenseApplication);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/licenseapplications/{id}// DELETE api/licenseapplications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicenseApplication(int id)
        {
            try
            {
                var licenseApplication = await _repositoryManager.LicenseApplicationRepository.GetByIdAsync(id);
                if (licenseApplication == null)
                {
                    return NotFound();
                }

                await _repositoryManager.LicenseApplicationRepository.DeleteAsync(licenseApplication);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        private static List<IFormFile> GetFormFileProperties(object obj)
        {
            var formFileProperties = new List<IFormFile>();
            var properties = obj.GetType().GetProperties().Where(p => p.PropertyType == typeof(IFormFile));

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    var formFile = (IFormFile)value;
                    formFileProperties.Add(formFile);
                }
            }

            return formFileProperties;
        }

        private async Task<List<Attachment>> SaveAttachmentsAsync(IEnumerable<IFormFile> attachments, int attachmentTypeId)
        {
            var attachmentsList = new List<Attachment>();
            var hostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var webRootPath = hostEnvironment.WebRootPath;

            // Log the web root path for debugging
            Log($"Web Root Path: {webRootPath}");

            // Check if webRootPath is null or empty
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null or empty");
            }

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads", Lambda.LicenseApplicationFolderName);



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

        private void Log(string message)
        {
            // Implement your logging mechanism here
            Console.WriteLine(message);
        }

        // POST api/licenseapplications/deny// POST api/licenseapplications/deny
        [HttpPost("deny")]
        public async Task<IActionResult> DenyLicenseApplication(DenyLicenseApplicationDTO denyLicenseApplicationDTO)
        {
            try
            {
                var licenseApplication = await _repositoryManager.LicenseApplicationRepository.GetByIdAsync(denyLicenseApplicationDTO.LicenseApplicationId);
                if (licenseApplication == null)
                    return NotFound();

                licenseApplication.ApplicationStatus = Lambda.Denied;

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);


                LicenseApprovalHistory appHistory = CreateLicenseApprovalHistory(licenseApplication, user, denyLicenseApplicationDTO.Reason);
                await _repositoryManager.LicenseApprovalHistoryRepository.AddAsync(appHistory);

                await _unitOfWork.CommitAsync();

                var currentApprovalLevel = await _repositoryManager.LicenseApprovalLevelRepository.GetLicenseApprovalLevelByLevel(1);
                licenseApplication.CurrentApprovalLevelID = currentApprovalLevel.Id;

                await _repositoryManager.LicenseApplicationRepository.UpdateAsync(licenseApplication);
                await _unitOfWork.CommitAsync();

                var appHistoryList = await _repositoryManager.LicenseApprovalHistoryRepository.GetLicenseApprovalHistoryByLicenseApplication(licenseApplication.Id);
                await NotifyUsersAsync(licenseApplication, user, denyLicenseApplicationDTO.Reason, appHistoryList);

                BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(licenseApplication.CreatedBy.Email, "License Application Status", $"Member please note that your license application has been denied.<br/> Reason: {denyLicenseApplicationDTO.Reason}", false));

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        private LicenseApprovalHistory CreateLicenseApprovalHistory(LicenseApplication licenseApplication, ApplicationUser user, string reason)
        {
            return new LicenseApprovalHistory
            {
                ApprovalLevelId = licenseApplication.CurrentApprovalLevel.Id,
                LicenseApplicationId = licenseApplication.Id,
                ChangedById = user.Id,
                ChangeDate = DateTime.UtcNow,
                Status = licenseApplication.ApplicationStatus,
                Comments = new List<LicenseApprovalComment>
                {
                    new LicenseApprovalComment
                    {
                        Comment = reason,
                        CommentedById = user.Id,
                        CommentDate = DateTime.UtcNow,
                    }
                }
            };
        }

        private async Task NotifyUsersAsync(LicenseApplication licenseApplication, ApplicationUser user, string reason, IEnumerable<LicenseApprovalHistory> appHistoryList)
        {
            if (appHistoryList != null)
            {
                var distinctUsers = appHistoryList.Where(x => x.ChangedById != licenseApplication.CreatedBy.Id).Select(x => x.ChangedById).Distinct();
                foreach (var userId in distinctUsers)
                {
                    var userToNotify = await _repositoryManager.UserRepository.GetSingleUser(userId);
                    string emailBody = $"Member license application has been denied. <br/>Reason: {reason}  <br/>Action Taken by:{user.Email}";
                    BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(userToNotify.Email, "License Application Status", emailBody, false));
                }
            }
        }
        [HttpGet("HasMemberMadePreviousApplications/{userId}")]
        public async Task<IActionResult> CheckIfMemberHasPreviousApplications(string userId)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetSingleUser(userId);

                if (user is not null)
                {
                    //get member record based on user id

                    var member = await this._repositoryManager.MemberRepository.GetMemberByUserId(userId);

                    if (member is not null)
                    {
                        //check if the member has made successfuly licence applications in the past
                        var hasPreviousApplications = await this._repositoryManager.LicenseApplicationRepository.HasPreviousApplicationsAsync(member.Id);

                        if (!hasPreviousApplications)
                        {
                            //check if there's a license with the memberId
                            var license = await this._repositoryManager.LicenseRepository.GetLicenseByMemberId(member.Id);
                            if (license != null)
                            {
                                return Ok(true);
                            }
                        }
                        else
                        {
                            return Ok(hasPreviousApplications);
                        }
                    }
                    return Ok(false);
                }

                return Ok(false);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        // GET api/licenseapplications/activate/{id}
        [HttpGet("accept/{id}")]
        public async Task<IActionResult> AcceptLicenseApplication(int id)
        {
            try
            {
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);

                var licenseApplication = await _repositoryManager.LicenseApplicationRepository.GetByIdAsync(id);
                if (licenseApplication == null)
                    return NotFound();

                licenseApplication.ApplicationStatus = Lambda.UnderReview;
                var currentLicenseApprovalLevel = licenseApplication.CurrentApprovalLevel;
                var currentLicenseApprovalLevelBeforeChanges = currentLicenseApprovalLevel.Id;
                var currentDepartment = currentLicenseApprovalLevel.Department;
                string licenseNumber = string.Empty;

                if (currentDepartment.Name.Equals("Executive", StringComparison.OrdinalIgnoreCase) && (currentRole.Equals("president", StringComparison.OrdinalIgnoreCase) || currentRole.Equals("honarary secretary", StringComparison.OrdinalIgnoreCase) || currentRole.Equals("ceo", StringComparison.OrdinalIgnoreCase)))
                {
                    licenseApplication.ApplicationStatus = Lambda.Approved;
                    licenseNumber = await GenerateLicenseNumber(licenseApplication);
                }
                else
                {
                    var nextApprovalLevel = await _repositoryManager.LicenseApprovalLevelRepository.GetNextApprovalLevel(currentLicenseApprovalLevel);
                    if (nextApprovalLevel != null)
                    {
                        licenseApplication.CurrentApprovalLevelID = nextApprovalLevel.Id;
                        licenseApplication.CurrentApprovalLevel = nextApprovalLevel;

                    }
                }

                await _repositoryManager.LicenseApplicationRepository.UpdateAsync(licenseApplication);

                var currentLicenseApprovalHistory = await _repositoryManager.LicenseApprovalHistoryRepository.GetLicenseApprovalHistoryByLicenseApplication(licenseApplication.Id);

                foreach (var approvalHistory in currentLicenseApprovalHistory)
                {
                    approvalHistory.Status = Lambda.Approved;
                    approvalHistory.ChangeDate = DateTime.UtcNow;
                    approvalHistory.UpdatedDate = DateTime.UtcNow;
                    await _repositoryManager.LicenseApprovalHistoryRepository.UpdateAsync(approvalHistory);
                }

                //add licence approval history record
                LicenseApprovalHistory appHistory = new LicenseApprovalHistory()
                {
                    ApprovalLevelId = licenseApplication.CurrentApprovalLevelID,
                    LicenseApplicationId = licenseApplication.Id,
                    ChangedById = user.Id,
                    ChangeDate = DateTime.UtcNow,
                    Status = licenseApplication.ApplicationStatus,
                };

                //add a record of license history to the datastore
                await _repositoryManager.LicenseApprovalHistoryRepository.AddAsync(appHistory);

                await _unitOfWork.CommitAsync();



                if (licenseApplication.ApplicationStatus == Lambda.Approved)
                {
                    await SendApprovedNotifications(licenseApplication, user);
                    //create a new licence record
                    var license = new License()
                    {
                        LicenseApplicationId = licenseApplication.Id,
                        MemberId = licenseApplication.MemberId,
                        LicenseNumber = licenseNumber,
                        ExpiryDate = licenseApplication.YearOfOperation.EndDate,
                        YearOfOperationId = licenseApplication.YearOfOperationId,

                    };

                    license.LicenseNumber = licenseNumber;

                    //save to data store
                    await _repositoryManager.LicenseRepository.AddAsync(license);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    //check if the current approval level changed
                    if (licenseApplication.CurrentApprovalLevel.Id != currentLicenseApprovalLevelBeforeChanges)
                    {
                        //send notification to the current approver
                        await SendReviewNotification(licenseApplication, user);
                    }

                }

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<string> GenerateLicenseNumber(LicenseApplication licenseApplication)
        {
            string licenseNumber = string.Empty;
            var licenseYearOfApplication = await _repositoryManager.YearOfOperationRepository.GetByIdAsync(licenseApplication.YearOfOperationId);
            //var activeYear = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();

            var lastLicenseNumber = await _repositoryManager.LicenseRepository.GetLastLicenseNumber(licenseYearOfApplication.Id);

            if (lastLicenseNumber != null)
            {
                string lastLicenseNumberString = lastLicenseNumber.LicenseNumber;
                int indexOfMLS = lastLicenseNumberString.IndexOf("MLS");

                if (indexOfMLS != -1)
                {
                    int startIndex = indexOfMLS + 3;
                    string numberAfterMLS = lastLicenseNumberString.Substring(startIndex);
                    int newNumber = int.Parse(numberAfterMLS) + 1;
                    licenseNumber = $"{licenseYearOfApplication.StartDate.Year}/{licenseYearOfApplication.EndDate.Year}MLS{newNumber.ToString("D4")}";
                }
                else
                {
                    licenseNumber = $"{licenseYearOfApplication.StartDate.Year}/{licenseYearOfApplication.EndDate.Year}MLS0001";
                }
            }
            else
            {
                licenseNumber = $"{licenseYearOfApplication.StartDate.Year}/{licenseYearOfApplication.EndDate.Year}MLS0001";
            }

            return licenseNumber;

        }

        private async Task SendApprovedNotifications(LicenseApplication licenseApplication, ApplicationUser user)
        {
            string emailTo = licenseApplication.CreatedBy.Email;
            string emailBody = "Your license has been approved in the system. Please visit the system to get your license number.";
            BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(emailTo, "License Application Status", emailBody, false));

            emailTo = user.Email;
            emailBody = $"You have approved a license application for a member {licenseApplication.CreatedBy.FirstName} {licenseApplication.CreatedBy.LastName}.";
            BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(emailTo, "License Application Status", emailBody, false));
        }

        private async Task SendReviewNotification(LicenseApplication licenseApplication, ApplicationUser user)
        {
            string emailTo = user.Email;
            string emailBody = $"You have approved a license application for a member {licenseApplication.CreatedBy.FirstName} {licenseApplication.CreatedBy.LastName}. It has been sent to {licenseApplication.CurrentApprovalLevel.Department.Name} department for further review.";
            BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(emailTo, "License Application Status", emailBody, false));

            emailBody = $"Your license application is under review and currently it has been sent to {licenseApplication.CurrentApprovalLevel.Department.Name} department for review.";
            BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(licenseApplication.CreatedBy.Email, "License Application Status", emailBody, false));

        }

        [HttpGet("count")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var count = await _repositoryManager.LicenseApplicationRepository.GetLicenseApplicationsTotal();

                return Ok(count);
            }
            catch (Exception ex)
            {

                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("generate-licenses/{year}")]
        public async Task<IActionResult> GenerateLicensesForYear(string? year = null)
        {
            try
            {
                var licenseGenerationService = HttpContext.RequestServices
                    .GetRequiredService<ILicenseGenerationService>();

                // Remove the query parameter and just use the route value
                year = year?.TrimEnd('?');

                if (string.IsNullOrEmpty(year))
                {
                    var result = await licenseGenerationService.GenerateLicensesForYear();
                    return Ok(new
                    {
                        message = $"License generation complete. Success: {result.success}, Failed: {result.failed}",
                        successCount = result.success,
                        failedCount = result.failed
                    });
                }

                // Clean up the year format
                year = year.Replace("year=", "").Trim();

                var yearParts = year.Split('-');
                if (yearParts.Length != 2 || !int.TryParse(yearParts[0], out int startYear) || !int.TryParse(yearParts[1], out int endYear))
                {
                    return BadRequest("Invalid year format. Please use 'YYYY-YYYY' format.");
                }

                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetAsync(
                    y => y.StartDate.Year == startYear && y.EndDate.Year == endYear);

                if (yearOfOperation == null)
                {
                    return NotFound("Year of operation not found.");
                }

                var (success, failed) = await licenseGenerationService.GenerateLicensesForYear(yearOfOperation.Id);

                return Ok(new
                {
                    message = $"License generation complete. Success: {success}, Failed: {failed}",
                    successCount = success,
                    failedCount = failed
                });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
