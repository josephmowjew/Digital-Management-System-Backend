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
using Microsoft.Extensions.Hosting;
using DataStore.Helpers;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MLS_Digital_MGM.DataStore.Helpers;
using DataStore.Core.DTOs.CPDTrainingRegistration;
using DataStore.Core.DTOs.License;
using Newtonsoft.Json;
using Hangfire;


namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CPDTrainingRegistrationsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public CPDTrainingRegistrationsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetCPDTrainingRegistrations(int pageNumber = 1, int pageSize = 10,int cpdTrainingId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);

                var pagingParameters = new PagingParameters<CPDTrainingRegistration>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.CPDTrainingId == cpdTrainingId,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<CPDTrainingRegistration, object>>[] {
                        p => p.CPDTraining,
                        p => p.CreatedBy,
                        p => p.Member,
                        p => p.Attachments
                    },
                   
                };

                var cpdTrainingRegistrationsPaged = await _repositoryManager.CPDTrainingRegistrationRepository.GetPagedAsync(pagingParameters);

                if (cpdTrainingRegistrationsPaged == null || !cpdTrainingRegistrationsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadCPDTrainingRegistrationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadCPDTrainingRegistrationDTO>());
                }

                var cpdTrainingRegistrationDTOs = _mapper.Map<List<ReadCPDTrainingRegistrationDTO>>(cpdTrainingRegistrationsPaged);
                
                //get the current year of operation
                var currentYearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();

                foreach (var cpdTrainingRegistration in cpdTrainingRegistrationDTOs)
                {
                    //get license with member id and current year of operation
                    var license = await _repositoryManager.LicenseRepository.GetSingleAsync(p => p.MemberId == cpdTrainingRegistration.MemberId && p.YearOfOperationId == currentYearOfOperation.Id);

                    if(license  != null)
                    {
                        cpdTrainingRegistration.Member.CurrentLicense = this._mapper.Map<ReadLicenseDTO>(license);
                    
                    }

                    //include attachments
                     foreach (var attachment in cpdTrainingRegistration.Attachments)
                    {
                        string attachmentTypeName = attachment.AttachmentType.Name;


                        string newfilePath = Path.Combine("/Uploads/CPDTrainingRegistration/", attachment.FileName);

                        attachment.FilePath = newfilePath;
                    }
                }

                //get the current cu

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = cpdTrainingRegistrationDTOs.Count;
                    var totalRecords = await _repositoryManager.CPDTrainingRegistrationRepository.CountAsync(pagingParameters);


                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = cpdTrainingRegistrationDTOs.ToList()
                    });
                }

                return Ok(cpdTrainingRegistrationDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCPDTrainingRegistration([FromForm] CreateCPDTrainingRegistrationDTO cpdTrainingRegistrationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cpdTrainingRegistration = _mapper.Map<CPDTrainingRegistration>(cpdTrainingRegistrationDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                cpdTrainingRegistration.CreatedById = user.Id;
                var member = await _repositoryManager.MemberRepository.GetMemberByUserId(user.Id);

                if (member == null)
                {
                    ModelState.AddModelError(nameof(cpdTrainingRegistrationDTO.CPDTrainingId), "Your membership application is incomplete. Please complete it by filling details in your profile to proceed");
                    return BadRequest(ModelState);
                }

                

                var existingCPDTrainingRegistration = await _repositoryManager.CPDTrainingRegistrationRepository.GetAsync(
                    d => d.CPDTrainingId == cpdTrainingRegistration.CPDTrainingId && d.MemberId == member.Id);

                if (existingCPDTrainingRegistration != null)
                {
                    ModelState.AddModelError(nameof(cpdTrainingRegistrationDTO.CPDTrainingId), "You have already registered for this CPD training");
                    return BadRequest(ModelState);
                }

                //get the cpd training from the database
                var cpdTraining = await _repositoryManager.CPDTrainingRepository.GetAsync(d => d.Id == int.Parse(cpdTrainingRegistrationDTO.CPDTrainingId));

                if(cpdTraining == null)
                {
                    ModelState.AddModelError(nameof(cpdTrainingRegistrationDTO.CPDTrainingId), "CPD training not found");
                    return BadRequest(ModelState);
                }


                //set member id
                cpdTrainingRegistration.MemberId = member.Id;
                cpdTrainingRegistration.Member = member;

                string currentUserRole = await Lambda.GetCurrentUserRole(_repositoryManager, HttpContext, _errorLogService);

                                // Check if it is a paid event/training or not
                if (cpdTraining.IsFree)
                {
                    if (cpdTrainingRegistrationDTO.Attachments.Count < 1)
                    {
                        ModelState.AddModelError(nameof(cpdTrainingRegistrationDTO.Attachments), "Please upload proof of payment");
                        return BadRequest(ModelState);
                    }

                    // Set the status of the registration to registered for free events
                    
                    cpdTrainingRegistration.RegistrationStatus = Lambda.Registered;
                }
                else
                {
                    // Check if proof of payment was submitted
                    if (!cpdTrainingRegistrationDTO.Attachments.Any())
                    {
                        ModelState.AddModelError(nameof(cpdTrainingRegistrationDTO.Attachments), "Please upload proof of payment");
                        return BadRequest(ModelState);
                    }

                    // Determine the fee based on the user's role and attendance mode
                    double fee = currentUserRole switch
                    {
                        "Unknown" => GetNonMemberFee(cpdTraining, cpdTrainingRegistrationDTO.AttendanceMode),
                        "NonMember" => GetNonMemberFee(cpdTraining, cpdTrainingRegistrationDTO.AttendanceMode),
                        _ => GetMemberFee(cpdTraining, cpdTrainingRegistrationDTO.AttendanceMode)
                    };

                    // Set the status of the registration to pending for paid events
                    cpdTrainingRegistration.RegistrationStatus = Lambda.Pending;
                    cpdTrainingRegistration.Fee = fee;
                }


                //set cpd training id
                cpdTrainingRegistration.CPDTrainingId = int.Parse(cpdTrainingRegistrationDTO.CPDTrainingId);
                //set cpd training registration created by
                cpdTrainingRegistration.CreatedById = user.Id;
                cpdTrainingRegistration.CreatedBy = user;

                  var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "CPDTrainingRegistration") 
                                    ?? new AttachmentType { Name = "CPDTrainingRegistration" };

                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                 if (cpdTrainingRegistrationDTO.Attachments?.Any() == true)
                {
                    cpdTrainingRegistration.Attachments = await SaveAttachmentsAsync(cpdTrainingRegistrationDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.CPDTrainingRegistrationRepository.AddAsync(cpdTrainingRegistration);
                await _unitOfWork.CommitAsync();

                //send email to finance team
                var financeTeam = await _repositoryManager.UserRepository.GetUsersByRoleAsync(Lambda.Finance);
                if(financeTeam.Any())
                {
                    List<string> emailTo = financeTeam.Select(u => u.Email).ToList();
                    string emailBody = $"New CPD training registration has been submitted by {member.User.FirstName} {member.User.LastName}. kindly review the attached proof of payment";
            
                    BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(emailTo,emailBody,"CPD Training Registration Status"));

                }

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetCPDTrainingRegistrationById/{id}")]
        public async Task<IActionResult> GetCPDTrainingRegistrationById(int id)
        {
            try
            {
                var cpdTrainingRegistration = await _repositoryManager.CPDTrainingRegistrationRepository.GetByIdAsync(id);
                if (cpdTrainingRegistration == null)
                {
                    return NotFound();
                }

                var mappedCPDTrainingRegistration = _mapper.Map<ReadCPDTrainingRegistrationDTO>(cpdTrainingRegistration);
                return Ok(mappedCPDTrainingRegistration);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCPDTrainingRegistration(int id, [FromForm] UpdateCPDTrainingRegistrationDTO cpdTrainingRegistrationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cpdTrainingRegistration = await _repositoryManager.CPDTrainingRegistrationRepository.GetByIdAsync(id);
                if (cpdTrainingRegistration == null)
                    return NotFound();

                _mapper.Map(cpdTrainingRegistrationDTO, cpdTrainingRegistration);
                await _repositoryManager.CPDTrainingRegistrationRepository.UpdateAsync(cpdTrainingRegistration);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCPDTrainingRegistration(int id)
        {
            try
            {
                var cpdTrainingRegistration = await _repositoryManager.CPDTrainingRegistrationRepository.GetByIdAsync(id);
                if (cpdTrainingRegistration == null)
                    return NotFound();

                await _repositoryManager.CPDTrainingRegistrationRepository.DeleteAsync(cpdTrainingRegistration);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<List<Attachment>> SaveAttachmentsAsync(IEnumerable<IFormFile> attachments, int attachmentTypeId)
     {
            var attachmentsList = new List<Attachment>();
            var hostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var webRootPath = hostEnvironment.WebRootPath;

            // Log the web root path for debugging
           

            // Check if webRootPath is null or empty
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null or empty");
            }

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/CPDTrainingRegistration" );

          

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

                // Log the file path
               

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

        //method to accept cpd training registration taking the id of the cpd training registration
        [HttpGet("AcceptCPDTrainingRegistration/{id}")]
        public async Task<IActionResult> AcceptCPDTrainingRegistration(int id)
        {
            try
            {
                var cpdTrainingRegistration = await _repositoryManager.CPDTrainingRegistrationRepository.GetByIdAsync(id);
                if (cpdTrainingRegistration == null)
                    return NotFound();
                    //set the status of the registration to accepted
                    cpdTrainingRegistration.RegistrationStatus = Lambda.Registered;
                await _repositoryManager.CPDTrainingRegistrationRepository.UpdateAsync(cpdTrainingRegistration);
                await _unitOfWork.CommitAsync();

                //send email to the user
                 string emailTo = cpdTrainingRegistration.CreatedBy.Email;
                 string emailBody = "Congratulation, Your CPD registration has been approved in MLS.";
                 BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string>{emailTo},emailBody,"CPD Training Registration Status"));


                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        //method to reject cpd training registration taking the id of the cpd training registration
        [HttpPut("RejectCPDTrainingRegistration/{id}")]
        public async Task<IActionResult> RejectCPDTrainingRegistration([FromForm] CPDRejectionDTO cPDRejectionDTO, int id)
        {
            try
            {
                var cpdTrainingRegistration = await _repositoryManager.CPDTrainingRegistrationRepository.GetByIdAsync(id);
                if (cpdTrainingRegistration == null)
                    return NotFound();
                    //set the status of the registration to rejected
                    cpdTrainingRegistration.RegistrationStatus = Lambda.Denied;
                    cpdTrainingRegistration.DeniedReason = cPDRejectionDTO.Reason;
                await _repositoryManager.CPDTrainingRegistrationRepository.UpdateAsync(cpdTrainingRegistration);
                await _unitOfWork.CommitAsync();

                //send email to the user
                 string emailTo = cpdTrainingRegistration.CreatedBy.Email;
                 string emailBody = "Your CPD registration has been rejected in MLS-Digital-MGM. <br>" + cPDRejectionDTO.Reason;
                 await _emailService.SendMailWithKeyVarReturn(emailTo, "CPD Registration Status", emailBody);

                BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string>{emailTo},emailBody,"CPD Training Registration Status"));


                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("MarkAttendance")]
        public async Task<IActionResult> MarkAttendance([FromForm] string ids)
        {
            try{
                var idArray = JsonConvert.DeserializeObject<int[]>(ids);
                // Use the idArray to mark attendance
            
                //get the cpd training registration from the database whose ids are in the idArray
                var cpdTrainingRegistrations = await _repositoryManager.CPDTrainingRegistrationRepository.GetAll(d => idArray.Contains(d.Id));
                List<string> memberEmails = new List<string>();
                //loop through the cpd training registrations and mark attendance
                foreach (var cpdTrainingRegistration in cpdTrainingRegistrations)
                {
                    cpdTrainingRegistration.RegistrationStatus = Lambda.Attended;

                    await _repositoryManager.CPDTrainingRegistrationRepository.UpdateAsync(cpdTrainingRegistration);

                    //assign the cpd units to the member 
                    CPDUnitsEarned cPDUnitsEarned = new CPDUnitsEarned(){
                        CPDTrainingId = cpdTrainingRegistration.CPDTrainingId,
                        MemberId = cpdTrainingRegistration.MemberId,
                        UnitsEarned = cpdTrainingRegistration.CPDTraining.CPDUnitsAwarded,
                        YearOfOperationId = cpdTrainingRegistration.CPDTraining.YearOfOperationId
                    };

                    await _repositoryManager.CPDUnitsEarnedRepository.AddAsync(cPDUnitsEarned);

                    memberEmails.Add(cpdTrainingRegistration.Member.User.Email);
                    
                }
                await _unitOfWork.CommitAsync();

                BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(memberEmails,"Your attendance has been marked in MLS-Digital-MGM and you have been awarded with CPD units. Kindnly visit the system for more details. Thank you.","CPD Training Status"));

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
            
        }

        private double GetMemberFee(CPDTraining cpdTraining, string attendanceMode)
    {
        return attendanceMode switch
        {
            "Virtual" => (double)cpdTraining.MemberVirtualAttendanceFee,
            "Physical" => (double)cpdTraining.MemberPhysicalAttendanceFee,
            _ => 0.0
        };
    }

        private double GetNonMemberFee(CPDTraining cpdTraining, string attendanceMode)
    {
        return attendanceMode switch
        {
            "Virtual" => (double)cpdTraining.NonMemberVirtualAttandanceFee,
            "Physical" => (double)cpdTraining.NonMemberPhysicalAttendanceFee,
            _ => 0.0
        };
    }

        [HttpGet("count")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                var member = await _repositoryManager.MemberRepository.GetMemberByUserId(CreatedById);

                var count = await _repositoryManager.CPDTrainingRegistrationRepository.GetCpdTrainingsAttendedCountByUserAsync(member.Id);

                return Ok(count);
            }
            catch (Exception ex)
            {

                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }


    public class CPDRejectionDTO
    {
        public string Reason { get; set; }
        public int Id { get; set; }
    }
}
