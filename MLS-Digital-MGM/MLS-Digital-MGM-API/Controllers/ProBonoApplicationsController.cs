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
using DataStore.Core.DTOs.ProBonoApplication;
using Microsoft.Extensions.Hosting;
using DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Hangfire;

namespace MLS_Digital_MGM_API.Controllers 
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProBonoApplicationsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        public ProBonoApplicationsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
    
        [HttpGet("paged")]
        public async Task<IActionResult> GetProBonoApplications(int pageNumber = 1, int pageSize = 10)
        {
            try
            {   // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;


                 string currentRole  = Lambda.GetCurrentUserRole(_repositoryManager,user.Id);
                
                var pagingParameters = new PagingParameters<ProBonoApplication>();

                // Check if the user is secretariat and approve the application if so
                pagingParameters = new PagingParameters<ProBonoApplication>
                {
                    Predicate = u => u.Status != Lambda.Deleted && (string.Equals(currentRole, "secretariat", StringComparison.OrdinalIgnoreCase) || u.CreatedById == user.Id),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<ProBonoApplication, object>>[] {
                        p => p.YearOfOperation,
                        p => p.ProbonoClient,
                        p => p.CreatedBy
                    },
                    CreatedById = string.Equals(currentRole, "secretariat", StringComparison.OrdinalIgnoreCase) ? null : CreatedById,

                };
                

                
                var proBonoApplicationspaged = await _repositoryManager.ProBonoApplicationRepository.GetPagedAsync(pagingParameters);
    
                if (proBonoApplicationspaged == null || !proBonoApplicationspaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadProBonoApplicationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadProBonoApplicationDTO>()); // Return empty list

                }
    
                    // Map the Roles to a list of ReadFirmDTOs
                var probonoapplicationFirms = _mapper.Map<List<ReadProBonoApplicationDTO>>(proBonoApplicationspaged);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = probonoapplicationFirms.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltred,
                        recordsTotal = resultTotalFiltred,
                        data = probonoapplicationFirms.ToList() // Materialize the enumerable
                    });
                }


                // Return an Ok result with the mapped Roles
                return Ok(probonoapplicationFirms);
    
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("denyApplication")]
        public async Task<IActionResult> DenyProBonoApplication(DenyProBonoApplicationDTO denyProBonoApplicationDTO)
        {
            try
            {
                var proBonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(denyProBonoApplicationDTO.ProBonoApplicationId);
                proBonoApplication.ApplicationStatus = Lambda.Denied;
                proBonoApplication.DenialReason = denyProBonoApplicationDTO.Reason;

                await _repositoryManager.ProBonoApplicationRepository.UpdateAsync(proBonoApplication);
                await _unitOfWork.CommitAsync();

                //send email to the user who created the probono application

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                
                 // Send status details email
                string emailBody = $"Your application for the pro bono application has been denied. <br/> Reason: {denyProBonoApplicationDTO.Reason}";
                

                BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string>{user.Email},emailBody,"Pro Bono Application Status"));


                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddProBonoApplication([FromForm] CreateProBonoApplicationDTO proBonoApplicationDTO)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var proBonoApplication = _mapper.Map<ProBonoApplication>(proBonoApplicationDTO);

                
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                proBonoApplication.CreatedById = user.Id;


                await _repositoryManager.ProBonoApplicationRepository.AddAsync(proBonoApplication);

               

                // Check if a ProBonoApplication with the same nature of dispute already exists
                var existingProBonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetAsync(
                    d => d.NatureOfDispute.Trim().Equals(proBonoApplication.NatureOfDispute.Trim(), StringComparison.OrdinalIgnoreCase) && 
                    d.ApprovedDate == null);

                if (existingProBonoApplication != null)
                {
                    ModelState.AddModelError(nameof(proBonoApplicationDTO.NatureOfDispute), "A pro bono application with the same nature of dispute already exists");
                    return BadRequest(ModelState);
                }

                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "ProBonoApplication") 
                                    ?? new AttachmentType { Name = "ProBonoApplication" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                // Save attachments if any
                if (proBonoApplicationDTO.Attachments?.Any() == true)
                {
                    proBonoApplication.Attachments = await SaveAttachmentsAsync(proBonoApplicationDTO.Attachments, attachmentType.Id);
                }

                //get the current role of the user

            
                 string currentRole  = Lambda.GetCurrentUserRole(_repositoryManager,user.Id);


                // Check if the user is secretariat and approve the application if so
                if (string.Equals(currentRole, "secretariat", StringComparison.OrdinalIgnoreCase))
                {
                    proBonoApplication.ApplicationStatus = Lambda.Approved;
                    proBonoApplication.ApprovedDate = DateTime.UtcNow;

                    //added a record of an actual pro bono itself as well once the application has been saved

                    var probono = this._mapper.Map<ProBono>(proBonoApplication);

                    //generate a unique file number
                    string fileNumber = await GenerateUniqueFileNumber();
                    probono.ProBonoApplicationId = proBonoApplication.Id;
                    probono.FileNumber = fileNumber;
                    
                

                    await _repositoryManager.ProBonoRepository.AddAsync(probono);

                
                    
                    // Send status details email
                    string emailBody = $"Your application for the pro bono application has been accepted.";
                   

                    BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string>{user.Email},emailBody,"Pro Bono Application Status"));
                    
                    await _unitOfWork.CommitAsync();
                }

               
                // Add ProBonoApplication to repository
                await _repositoryManager.ProBonoApplicationRepository.AddAsync(proBonoApplication);
                await _unitOfWork.CommitAsync();

                

              

                // Return created ProBonoApplication
                return CreatedAtAction("GetProBonoApplications", new { id = proBonoApplication.Id }, proBonoApplication);
            }
            catch (Exception ex)
            {
                // Log error and return internal server error
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<string> GenerateUniqueFileNumber()
        {
            var lastProBono = await _repositoryManager.ProBonoRepository.GetLastProBonoAsync();

            int id = lastProBono?.Id + 1 ?? 1;

            return $"MLS-ProBono{id}";
        }   

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProBonoApplication(int id, [FromBody] UpdateProBonoApplicationDTO proBonoApplicationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var proBonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(id);
                if (proBonoApplication == null)
                {
                    return NotFound();
                }

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "ProBonoApplication") 
                                    ?? new AttachmentType { Name = "ProBonoApplication" };

                if (proBonoApplicationDTO.Attachments?.Any() == true)
                {
                    proBonoApplication.Attachments = await SaveAttachmentsAsync(proBonoApplicationDTO.Attachments, attachmentType.Id);
                }

                _mapper.Map(proBonoApplicationDTO, proBonoApplication);
                await _repositoryManager.ProBonoApplicationRepository.UpdateAsync(proBonoApplication);
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
        public async Task<IActionResult> DeleteProBonoApplication(int id)
        {
            try
            {
                var proBonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(id);
                if (proBonoApplication == null)
                {
                    return NotFound();
                }
    
                await _repositoryManager.ProBonoApplicationRepository.DeleteAsync(proBonoApplication);
                await _unitOfWork.CommitAsync();
    
                return NoContent();
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
            var proBonoApplicationAttachmentsPath = Path.Combine(webRootPath, "Uploads/ProBonoAttachments");

            Directory.CreateDirectory(proBonoApplicationAttachmentsPath);

            foreach (var attachment in attachments)
            {
                var uniqueFileName = FileNameGenerator.GenerateUniqueFileName(attachment.FileName);
                var filePath = Path.Combine(proBonoApplicationAttachmentsPath, uniqueFileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await attachment.CopyToAsync(stream);
                }

                attachmentsList.Add(new Attachment
                {
                    FileName = uniqueFileName,
                    FilePath = filePath,
                    AttachmentTypeId = attachmentTypeId
                });
            }

            return attachmentsList;
        }

        [HttpGet("getprobonoapplication/{id}")]
        public async Task<IActionResult> GetProBonoApplicationById(int id)
        {
            try
            {
                var proBonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(id);
                if (proBonoApplication == null)
                {
                    return NotFound();
                }
                var proBonoApplicationDTO = _mapper.Map<ReadProBonoApplicationDTO>(proBonoApplication);
                return Ok(proBonoApplicationDTO);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("activate/{id}")]
        public async Task<IActionResult> Activate(int id)
        {
             try
            {
                // Fetch  clients using the UserRepository
                var application = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(id);

                if(application != null)
                {
                    application.ApplicationStatus = Lambda.Approved;
                    application.ApprovedDate = DateTime.UtcNow;
                    await _repositoryManager.ProBonoApplicationRepository.UpdateAsync(application);
                    await _unitOfWork.CommitAsync();

                    //added a record of an actual pro bono itself as well once the application has been saved

                    var probono = this._mapper.Map<ProBono>(application);

                    //generate a unique file number
                    string fileNumber = await GenerateUniqueFileNumber();
                    probono.ProBonoApplicationId = application.Id;
                    probono.FileNumber = fileNumber;

                    await _repositoryManager.ProBonoRepository.AddAsync(probono);

                     await _unitOfWork.CommitAsync();

                    //send email to the user who created the probono application

                    string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                    //get user id from username
                    var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                    
                    // Send status details email
                    string emailBody = $"Your application for the pro bono application has been accepted. The file number is {fileNumber}";
                   

                    BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string>{user.Email},emailBody,"Pro Bono Application Status"));


                    
                    return Ok();
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


    }

}