using AutoMapper;
using DataStore.Core.DTOs.PenaltyPayment;
using DataStore.Core.DTOs.PenaltyType;
using DataStore.Core.DTOs.ProBonoApplication;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Linq.Expressions;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PenaltyPaymentsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PenaltyPaymentsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPenaltyPayments(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<PenaltyPayment>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<PenaltyPayment, object>>[] {
                        p => p.Attachments
                    },

                };

                var penaltyPaymentsPaged = await _repositoryManager.PenaltyPaymentRepository.GetPagedAsync(pagingParameters);
    
                if (penaltyPaymentsPaged == null || !penaltyPaymentsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadPenaltyPaymentDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadPenaltyPaymentDTO>());
                }

                var penaltyPaymentsDTOs = _mapper.Map<List<ReadPenaltyPaymentDTO>>(penaltyPaymentsPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = penaltyPaymentsDTOs.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltered,
                        recordsTotal = resultTotalFiltered,
                        data = penaltyPaymentsDTOs.ToList()
                    });
                }

                return Ok(penaltyPaymentsDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPenaltyPayment([FromForm] CreatePenaltyPaymentDTO penaltyPaymentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var penaltyPayment = _mapper.Map<PenaltyPayment>(penaltyPaymentDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                //penaltyType.CreatedById = user.Id;

                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "PenaltyPayment")
                                    ?? new AttachmentType { Name = "PenaltyPayment" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }
                if (penaltyPaymentDTO.Attachments != null && penaltyPaymentDTO.Attachments.Count > 0)
                {
                    penaltyPayment.Attachments = await SaveAttachmentsAsync(penaltyPaymentDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.PenaltyPaymentRepository.AddAsync(penaltyPayment);
                await _unitOfWork.CommitAsync();

                CreatedAtAction("GetPenaltyById", new { id = penaltyPayment.Id }, penaltyPayment);

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetPenaltyPaymentById/{id}")]
        public async Task<IActionResult> GetPenaltyPaymentById(int id)
        {
            try
            {
                var penaltyPayment = await _repositoryManager.PenaltyPaymentRepository.GetByIdAsync(id);
                if (penaltyPayment == null)
                {
                    return NotFound();
                }

                foreach (var attachment in penaltyPayment.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;

                    string newFilePath = Path.Combine($"http://{HttpContext.Request.Host}/uploads/{Lambda.PenaltyPaymentFolderName}", attachment.FileName);

                    attachment.FilePath = newFilePath;

                }

                var mappedPenaltyPayment = _mapper.Map<ReadPenaltyPaymentDTO>(penaltyPayment);
                return Ok(mappedPenaltyPayment);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePenaltyPayment(int id, [FromForm] UpdatePenaltyPaymentDTO penaltyPaymentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var penaltyPayment = await _repositoryManager.PenaltyPaymentRepository.GetByIdAsync(id);
                if (penaltyPayment == null)
                    return NotFound();

                _mapper.Map(penaltyPaymentDTO, penaltyPayment);

                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "PenaltyPayment")
                                    ?? new AttachmentType { Name = "PenaltyPayment" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }
                if (penaltyPaymentDTO.Attachments != null && penaltyPayment.Attachments.Count > 0)
                {
                    penaltyPayment.Attachments = await SaveAttachmentsAsync(penaltyPaymentDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.PenaltyPaymentRepository.UpdateAsync(penaltyPayment);
                await _unitOfWork.CommitAsync();

                return Ok();
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
                var penaltyPaymentRecords = await this._repositoryManager.PenaltyTypeRepository.GetAllAsync();

                var readPenaltyPaymentRecordsMapped = this._mapper.Map<List<ReadPenaltyPaymentDTO>>(penaltyPaymentRecords);

                return Ok(readPenaltyPaymentRecordsMapped);
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
            var penaltyPaymentAttachmentsPath = Path.Combine(webRootPath, "Uploads/PenaltyPaymentsAttachments");

            Directory.CreateDirectory(penaltyPaymentAttachmentsPath);

            foreach (var attachment in attachments)
            {
                var uniqueFileName = FileNameGenerator.GenerateUniqueFileName(attachment.FileName);
                var filePath = Path.Combine(penaltyPaymentAttachmentsPath, uniqueFileName);
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

            return attachmentsList;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePenaltyPayment(int id)
        {
            try
            {
                var penaltyPayment = await _repositoryManager.PenaltyPaymentRepository.GetByIdAsync(id);
                if (penaltyPayment == null)
                    return NotFound();

                await _repositoryManager.PenaltyPaymentRepository.DeleteAsync(penaltyPayment);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                // Fetch  penalty payment
                var payment = await _repositoryManager.PenaltyPaymentRepository.GetByIdAsync(id);

                if (payment != null)
                {
                    payment.PaymentStatus = Lambda.Approved;
                    payment.DateApproved = DateTime.UtcNow;
                    await _repositoryManager.PenaltyPaymentRepository.UpdateAsync(payment);
                    await _unitOfWork.CommitAsync();


                    //update penalty amount paid
                    var penalty = await _repositoryManager.PenaltyRepository.GetByIdAsync(payment.PenaltyId);
                    if(penalty.AmountPaid < penalty.Fee) {
                        penalty.AmountPaid += payment.Fee;
                        penalty.AmountRemaining -= penalty.AmountPaid;
                        await _repositoryManager.PenaltyRepository.UpdateAsync(penalty);
                        await _unitOfWork.CommitAsync();
                    }
                    


                    //send email to the user who created the probono application

                    //string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                    //get user id from username
                    //var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                    // Send status details email
                    //string emailBody = $"Your application for the pro bono application has been accepted. The file number is {fileNumber}";


                    //BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string> { user.Email }, emailBody, "Pro Bono Application Status"));



                    return Ok();
                }
                return BadRequest("Payment not found");

            }
            catch (Exception ex)
            {

                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("denyPayment")]
        public async Task<IActionResult> DenyPenaltyPayment(DenyPenaltyPaymentDTO denyPenaltyPaymentDTO)
        {
            try
            {
                var penaltyPayment = await _repositoryManager.PenaltyPaymentRepository.GetByIdAsync(denyPenaltyPaymentDTO.PenaltyPaymentId);
                penaltyPayment.PaymentStatus = Lambda.Denied;
                penaltyPayment.ReasonForDenial = denyPenaltyPaymentDTO.Reason;
                penaltyPayment.DateDenied = DateTime.UtcNow;

                await _repositoryManager.PenaltyPaymentRepository.UpdateAsync(penaltyPayment);
                await _unitOfWork.CommitAsync();

                //send email to the user who created the probono application

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                //var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                // Send status details email
                /*string emailBody = $"Your application for the pro bono application has been denied. <br/> Reason: {denyPenaltyPaymentDTO.Reason}";


                BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string> { user.Email }, emailBody, "Pro Bono Application Status"));*/


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
