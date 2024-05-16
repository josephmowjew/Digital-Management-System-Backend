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

        public LicenseApplicationsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
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
                    Predicate = u => u.Status != Lambda.Deleted && (string.Equals(currentRole, "secretariat", StringComparison.OrdinalIgnoreCase) || u.CreatedById == user.Id),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<LicenseApplication, object>>[] {
                        p => p.YearOfOperation,
                        p => p.Member,
                    },
                    CreatedById = string.Equals(currentRole, "secretariat", StringComparison.OrdinalIgnoreCase) ? null : CreatedById,
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

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltred,
                        recordsTotal = resultTotalFiltred,
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
    [HttpPost]
    public async Task<IActionResult> AddLicenseApplication([FromForm] CreateLicenseApplicationDTO licenseApplicationDTO)
    {
            try
            {
              

              if (!licenseApplicationDTO.ActionType.Equals(Lambda.Draft,StringComparison.CurrentCultureIgnoreCase))
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    licenseApplicationDTO.ApplicationStatus = Lambda.Pending;
                }
                else
                {
                    licenseApplicationDTO.ApplicationStatus = Lambda.Draft;
                }

                var application = _mapper.Map<LicenseApplication>(licenseApplicationDTO);

                
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                application.CreatedById = user.Id;
               
               //get the current year of operation
                var currentYearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();

                if(currentYearOfOperation != null)
                {
                    application.YearOfOperationId = currentYearOfOperation.Id;
                   
                    
                }
                else
                {
                    ModelState.AddModelError("", "The current year of operation is not set");
                    return BadRequest(ModelState);
                }

                 //set the licence approval level to level zero 
                application.CurrentApprovalLevelID = 1;
                //get the id of the current member
                var currentMember = await _repositoryManager.MemberRepository.GetMemberByUserId(user.Id);

                if(currentMember != null)
                {
                    application.MemberId = currentMember.Id;
                   
                }

                //check if this is first application 
                var hasPreviousApplication = await _repositoryManager.LicenseApplicationRepository.HasPreviousApplicationsAsync(currentMember.Id);
                //set first application to true
                if(hasPreviousApplication is  false)
                {
                    application.FirstApplicationForLicense = true;
                }

                //create TODO code to check and set if the application has been created outside the allowed window


                // Check if a license application has been made in the same year and it is pending or hasn't been approved yet
                var existingApplication = await _repositoryManager.LicenseApplicationRepository.GetAsync(
                    a => a.YearOfOperationId == currentYearOfOperation.Id &&
                    (a.ApplicationStatus == Lambda.Pending || a.ApplicationStatus == Lambda.Approved)
                );



                if (existingApplication != null)
                {
                    ModelState.AddModelError("", "You already have a license application in the same year and it is pending or approved");
                    return BadRequest(ModelState);
                };


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
                // Save attachments if any
                if (formFileProperties.Any())
                {
                    var attachmentsList = await SaveAttachmentsAsync((IEnumerable<IFormFile>)formFileProperties, attachmentType.Id);

                    existingApplication = await this._repositoryManager.LicenseApplicationRepository.GetByIdAsync(application.Id);

                  if (existingApplication != null)
                    {
                        // Remove old attachments with the same name as the new ones
                        existingApplication.Attachments.RemoveAll(a => attachmentsList.Any(b => b.PropertyName == a.PropertyName));

                        // Add fresh list of attachments
                        existingApplication.Attachments.AddRange(attachmentsList);

                        //map update to existing application
                        _mapper.Map(licenseApplicationDTO, existingApplication);


                        // Update the application
                        await _repositoryManager.LicenseApplicationRepository.UpdateAsync(existingApplication);

                   }

                  
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
            
            if(!application.ApplicationStatus.Equals(Lambda.Draft, StringComparison.CurrentCultureIgnoreCase))
            {
                // Send status details email
                string emailBody = $"Your have made a license application for {currentYearOfOperation.StartDate.Year} - {currentYearOfOperation.EndDate.Year} year of operation. You can view the status of your application by clicking the link below.";
                var passwordEmailResult = await _emailService.SendMailWithKeyVarReturn(user.Email, "Annual Membership Application Status", emailBody);
            }

            
            
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

                string newFilePath = Path.Combine($"http://{HttpContext.Request.Host}/uploads/{Lambda.LicenseApplicationFolderName}", attachment.FileName);

                attachment.FilePath = newFilePath;
                //get the property with the same as the attachment property name
               // var property = licenseApplicationDTO.GetType().GetProperty(attachment.PropertyName);

                //set the new file path to the property
                //property.SetValue(licenseApplicationDTO, newFilePath);

                
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
        var applicationAttachmentsPath = Path.Combine(webRootPath, "Uploads/"+Lambda.LicenseApplicationFolderName);

        Directory.CreateDirectory(applicationAttachmentsPath);

        foreach (var attachment in attachments)
        {

            var propertyName = attachment.Name;
            var filePath = Path.Combine(applicationAttachmentsPath, attachment.FileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await attachment.CopyToAsync(stream);
            }

            attachmentsList.Add(new Attachment
            {
                FileName = attachment.FileName,
                FilePath = filePath,
                AttachmentTypeId = attachmentTypeId,
                PropertyName = attachment.Name
            });
        }

        return attachmentsList;
    }


    // POST api/licenseapplications/deny// POST api/licenseapplications/deny
    // [HttpPost("deny")]
    // public async Task<IActionResult> DenyLicenseApplication(DenyLicenseApplicationDTO denyLicenseApplicationDTO)
    // {
    //     try
    //     {
    //         var licenseApplication = await _repositoryManager.LicenseApplicationRepository.GetByIdAsync(denyLicenseApplicationDTO.Id);
    //         if (licenseApplication == null)
    //         {
    //             return NotFound();
    //         }

    //         licenseApplication.ApplicationStatus = Lambda.Denied;
    //         licenseApplication.DenialReason = denyLicenseApplicationDTO.DenialReason;

    //         await _repositoryManager.LicenseApplicationRepository.UpdateAsync(licenseApplication);
    //         await _unitOfWork.CommitAsync();

    //         // Send email notification to the applicant
    //         var emailBody = $"Your license application has been denied. Reason: {denyLicenseApplicationDTO.DenialReason}";
    //         await _emailService.SendMailWithKeyVarReturn(licenseApplication.ApplicantEmail, "License Application Status", emailBody);

    //         return Ok();
    //     }
    //     catch (Exception ex)
    //     {
    //         await _errorLogService.LogErrorAsync(ex);
    //         return StatusCode(500, "Internal server error");
    //     }
    // }
    [HttpGet("HasMemberMadePreviousApplications/{userId}")]
    public async Task<IActionResult> CheckIfMemberHasPreviousApplications(string userId)
    {
        try
        {
            var user = await _repositoryManager.UserRepository.GetSingleUser(userId);

            if(user is not null)
            {
                //get member record based on user id

                var member = await this._repositoryManager.MemberRepository.GetMemberByUserId(userId);

                if(member is not null)
                {
                    //check if the member has made successfuly licence applications in the past
                    var hasPreviousApplications = await this._repositoryManager.LicenseApplicationRepository.HasPreviousApplicationsAsync(member.Id);

                    return Ok(hasPreviousApplications);
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
            var licenseApplication = await _repositoryManager.LicenseApplicationRepository.GetByIdAsync(id);
            if (licenseApplication == null)
            {
                return NotFound();
            }

            licenseApplication.Status = Lambda.Approved;
            await _repositoryManager.LicenseApplicationRepository.UpdateAsync(licenseApplication);
            await _unitOfWork.CommitAsync();

              //send email to the user who created the probono application

            string username = _httpContextAccessor.HttpContext.User.Identity.Name;

            //get user id from username
            var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

            // Send email notification to the applicant
            var emailBody = "Your license application has been approved.";
            await _emailService.SendMailWithKeyVarReturn(user.Email, "License Application Status", emailBody);

            return Ok();
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    }
}