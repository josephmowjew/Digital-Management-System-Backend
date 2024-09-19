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
using DataStore.Core.DTOs.CPDTraining;
using MLS_Digital_MGM.DataStore.Helpers;
using DataStore.Core.DTOs.InvoiceRequest;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CPDTrainingsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public CPDTrainingsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetCPDTrainings(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);
                
                var pagingParameters = new PagingParameters<CPDTraining>
                {
                    Predicate = u => u.Status != Lambda.Deleted,

                    //Predicate = u => u.Status != Lambda.Deleted && u.ApprovalStatus == Lambda.Approved && (!string.Equals(currentRole, "member", StringComparison.OrdinalIgnoreCase) || u.CreatedById == user.Id),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<CPDTraining, object>>[] {
                        p => p.Attachments,
                        p => p.CreatedBy,
                        p => p.CPDTrainingRegistration
                    },
                    //CreatedById = string.Equals(currentRole, "member", StringComparison.OrdinalIgnoreCase) ? CreatedById: null ,
                };

                var cpdTrainingsPaged = await _repositoryManager.CPDTrainingRepository.GetPagedAsync(pagingParameters);

                if (cpdTrainingsPaged == null || !cpdTrainingsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadCPDTrainingDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadCPDTrainingDTO>());
                }

                var cpdTrainingDTOs = _mapper.Map<List<ReadCPDTrainingDTO>>(cpdTrainingsPaged);

                foreach (var training in cpdTrainingDTOs)
                {

                    //search if there is an invoice request beareing the cpd training id
                    var invoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetAsync(i => i.ReferencedEntityId == training.Id.ToString() && i.ReferencedEntityType == "CPDTrainings");

                    if (invoiceRequest != null)
                    {
                        training.InvoiceRequestId = invoiceRequest.Id;
                        training.InvoiceRequest = this._mapper.Map<ReadInvoiceRequestDTO>(invoiceRequest);
                    }

                    //set the file path for the attachments
                    foreach (var attachment in training.Attachments)
                    {
                        string attachmentTypeName = attachment.AttachmentType.Name;


                        string newfilePath = Path.Combine("/Uploads/CPDTrainings/", attachment.FileName);

                        attachment.FilePath = newfilePath;
                    }

                    //count the number of pending registrations on each training

                    if (training.CPDTrainingRegistration != null)
                    {
                        training.NumberOfPendingRegistrations = training.CPDTrainingRegistration.Count(r => r.RegistrationStatus == Lambda.Pending);
                    }



                }

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = cpdTrainingDTOs.Count;
                    var totalRecords = await _repositoryManager.CPDTrainingRepository.CountAsync(pagingParameters);


                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = cpdTrainingDTOs.ToList()
                    });
                }

                return Ok(cpdTrainingDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCPDTraining([FromForm] CreateCPDTrainingDTO cpdTrainingDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                if(string.IsNullOrEmpty(cpdTrainingDTO.AccreditingInstitution))
                {
                    cpdTrainingDTO.AccreditingInstitution = "MLS";
                }
                //check if it is a free training by getting the prices of all fees




                var cpdTraining = _mapper.Map<CPDTraining>(cpdTrainingDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                cpdTraining.CreatedById = user.Id;


                var existingCPDTraining = await _repositoryManager.CPDTrainingRepository.GetAsync(
                    d => d.Title.Trim().Equals(cpdTraining.Title.Trim(), StringComparison.OrdinalIgnoreCase) &&
                         d.DateToBeConducted == cpdTraining.DateToBeConducted);

                if (existingCPDTraining != null)
                {
                    ModelState.AddModelError(nameof(cpdTrainingDTO.Title), "A CPD training with the same title and date already exists");
                    return BadRequest(ModelState);
                }

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "CPDTraining")
                                    ?? new AttachmentType { Name = "CPDTraining" };

                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                if (cpdTrainingDTO.Attachments?.Any() == true)
                {
                    cpdTraining.Attachments = await SaveAttachmentsAsync(cpdTrainingDTO.Attachments, attachmentType.Id);
                }
                //check if it is a free training

               if ((cpdTraining.MemberPhysicalAttendanceFee == null || cpdTraining.MemberPhysicalAttendanceFee < 1) &&
                    (cpdTraining.MemberVirtualAttendanceFee == null || cpdTraining.MemberVirtualAttendanceFee < 1) &&
                    (cpdTraining.NonMemberPhysicalAttendanceFee == null || cpdTraining.NonMemberPhysicalAttendanceFee < 1) &&
                    (cpdTraining.NonMemberVirtualAttandanceFee == null || cpdTraining.NonMemberVirtualAttandanceFee < 1))
                {
                    cpdTraining.IsFree = true;
                }

                //get the current year of operation
                var currentYear = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();
                if (currentYear == null)
                {
                    ModelState.AddModelError(nameof(cpdTrainingDTO.Title), "No current year of operation found");
                    return BadRequest(ModelState);
                }
                else{
                    cpdTraining.YearOfOperationId = currentYear.Id;
                }

                //set created by
                cpdTraining.CreatedBy = user;
                cpdTraining.CreatedById = user.Id;



                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);
                if (!string.Equals(currentRole, "member", StringComparison.OrdinalIgnoreCase))
                {
                    cpdTraining.ApprovalStatus = Lambda.Approved;
                    cpdTraining.ProposedUnits = cpdTraining.CPDUnitsAwarded;

                }
                else{
                    cpdTraining.ApprovalStatus = Lambda.Pending;
                }


                await _repositoryManager.CPDTrainingRepository.AddAsync(cpdTraining);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetCPDTrainingById", new { id = cpdTraining.Id }, cpdTraining);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetCPDTrainingById/{id}")]
        public async Task<IActionResult> GetCPDTrainingById(int id)
        {
            try
            {
                var cPDTraining = await _repositoryManager.CPDTrainingRepository.GetByIdAsync(id);
                if (cPDTraining == null)
                {
                    return NotFound();
                }

                foreach (var attachment in cPDTraining.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;


                    string newFilePath = $"{Lambda.http}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.CPDTrainingFolderName}/{attachment.FileName}";

                    attachment.FilePath = newFilePath;

                }
                //add stamp to the attachments from stamps folder
                var stamp = await _repositoryManager.StampRepository.GetStampByNameAsync(Lambda.Seal);
                if (stamp != null)
                {
                    string stampFilePath = $"{Lambda.http}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.StampFolderName}/{stamp.Attachments[0].FileName}";
                    cPDTraining.Attachments.Add(new Attachment
                    {
                        FileName = stamp.Attachments[0].FileName,
                        FilePath = stampFilePath,
                        AttachmentTypeId = stamp.Attachments.FirstOrDefault().AttachmentTypeId,
                        PropertyName = stamp.Attachments.FirstOrDefault()?.PropertyName
                    });
                }

                //add signature to the attachments from signatures folder
                var signature = await _repositoryManager.SignatureRepository.GetSignatureByNameAsync(Lambda.PresidentSignature);
                if (signature != null)
                {
                    string signatureFilePath = $"{Lambda.http}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.SignatureFolderName}/{signature.Attachments[0].FileName}";
                    cPDTraining.Attachments.Add(new Attachment
                    {
                        FileName = signature.Attachments[0].FileName,
                        FilePath = signatureFilePath,
                        AttachmentTypeId = signature.Attachments.FirstOrDefault().AttachmentTypeId,
                        PropertyName = signature.Attachments.FirstOrDefault()?.PropertyName
                    });
                }

                var mappedCPDTraining = _mapper.Map<ReadCPDTrainingDTO>(cPDTraining);
                return Ok(mappedCPDTraining);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCPDTraining(int id, [FromForm] UpdateCPDTrainingDTO cpdTrainingDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cpdTraining = await _repositoryManager.CPDTrainingRepository.GetByIdAsync(id);
                if (cpdTraining == null)
                    return NotFound();


                cpdTrainingDTO.SetDefaultValues();

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "CPDTraining")
                                    ?? new AttachmentType { Name = "CPDTraining" };

                if (cpdTrainingDTO.Attachments?.Any() == true)
                {
                      
                       //get attachments from the cpdTrainingDTO that have at least greater than zero kb
                       var attachmentsToUpdate = cpdTrainingDTO.Attachments.Where(a => a.Length > 0).ToList();
                     
                        if(attachmentsToUpdate.Any())
                        {
                             var attachmentsList = await SaveAttachmentsAsync(attachmentsToUpdate, attachmentType.Id);

                             
                            // Remove old attachments with the same name as the new ones
                            cpdTraining.Attachments.RemoveAll(a => attachmentsList.Any(b => b.PropertyName == a.PropertyName));

                            // Add fresh list of attachments
                            cpdTraining.Attachments.AddRange(attachmentsList);


                        }
                }
                       

                if(string.IsNullOrEmpty(cpdTrainingDTO.AccreditingInstitution))
                {
                    cpdTrainingDTO.AccreditingInstitution = "MLS";
                }



                _mapper.Map(cpdTrainingDTO, cpdTraining);
                await _repositoryManager.CPDTrainingRepository.UpdateAsync(cpdTraining);
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
        public async Task<IActionResult> DeleteCPDTraining(int id)
        {
            try
            {
                var cpdTraining = await _repositoryManager.CPDTrainingRepository.GetByIdAsync(id);
                if (cpdTraining == null)
                    return NotFound();

                await _repositoryManager.CPDTrainingRepository.DeleteAsync(cpdTraining);
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


            // Check if webRootPath is null or empty
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null or empty");
            }

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/CPDTrainingsAttachments" );



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

    }
}

