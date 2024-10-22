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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MLS_Digital_MGM.DataStore.Helpers;
using DataStore.Helpers;
using System.Linq.Expressions;
using DataStore.Core.DTOs.NotaryPublic;
using Hangfire;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotaryPublicController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public NotaryPublicController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetNotaryPublics(int pageNumber = 1, int pageSize = 10, int memberId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<NotaryPublic>
                {
                    Predicate = u => ((memberId > 0 ? u.MemberId == memberId : true) 
                      && u.Status != Lambda.Deleted && u.ApplicationStatus == Lambda.Approved),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<NotaryPublic, object>>[] {
                        p => p.Member,
                        p => p.YearOfOperation,
                        p => p.Attachments,
                    }
                };

                var notaryPublicsPaged = await _repositoryManager.NotaryPublicRepository.GetPagedAsync(pagingParameters);

                if (notaryPublicsPaged == null || !notaryPublicsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadNotaryPublicDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadNotaryPublicDTO>());
                }

                var notaryPublicDTOs = _mapper.Map<List<ReadNotaryPublicDTO>>(notaryPublicsPaged);

                foreach (var notaryPublic in notaryPublicDTOs)
                {
                    foreach (var attachment in notaryPublic.Attachments)
                    {
                        string newfilePath = Path.Combine("Uploads/NotaryPublicAttachments/", attachment.FileName);
                        attachment.FilePath = newfilePath;
                    }
                }

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = notaryPublicDTOs.Count;
                    var totalRecords = await _repositoryManager.NotaryPublicRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = notaryPublicDTOs.ToList()
                    });
                }

                return Ok(notaryPublicDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingNotaryPublics(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<NotaryPublic>
                {
                    Predicate = u => u.ApplicationStatus == Lambda.Pending && u.Status == Lambda.Active,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<NotaryPublic, object>>[] {
                        p => p.Member,
                        p => p.YearOfOperation,
                        p => p.Attachments,
                    }
                };

                var notaryPublicsPaged = await _repositoryManager.NotaryPublicRepository.GetPagedAsync(pagingParameters);

                if (notaryPublicsPaged == null || !notaryPublicsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadNotaryPublicDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadNotaryPublicDTO>());
                }

                var notaryPublicDTOs = _mapper.Map<List<ReadNotaryPublicDTO>>(notaryPublicsPaged);

                foreach (var notaryPublic in notaryPublicDTOs)
                {
                    foreach (var attachment in notaryPublic.Attachments)
                    {
                        string newfilePath = Path.Combine("Uploads/NotaryPublicAttachments/", attachment.FileName);
                        attachment.FilePath = newfilePath;
                    }
                }

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = notaryPublicDTOs.Count;
                    var totalRecords = await _repositoryManager.NotaryPublicRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = notaryPublicDTOs.ToList()
                    });
                }

                return Ok(notaryPublicDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNotaryPublic([FromForm] CreateNotaryPublicDTO notaryPublicDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var notaryPublic = _mapper.Map<NotaryPublic>(notaryPublicDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "NotaryPublic")
                                    ?? new AttachmentType { Name = "NotaryPublic" };

                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                if (notaryPublicDTO.Attachments != null && notaryPublicDTO.Attachments.Count > 0)
                {
                    notaryPublic.Attachments = await SaveAttachmentsAsync(notaryPublicDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.NotaryPublicRepository.AddAsync(notaryPublic);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetNotaryPublicById", new { id = notaryPublic.Id }, notaryPublic);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("denyApplication")]
        public async Task<IActionResult> DenyNotaryPublicSubmission(DenyNotaryPublicDTO denyNotaryPublicDTO)
        {
            try
            {
                var notariesPublicSubmission = await _repositoryManager.NotaryPublicRepository.GetByIdAsync(denyNotaryPublicDTO.NotaryPublicId);
                notariesPublicSubmission.ApplicationStatus = Lambda.Denied;
                notariesPublicSubmission.DenialReason = denyNotaryPublicDTO.Reason;

                await _repositoryManager.NotaryPublicRepository.UpdateAsync(notariesPublicSubmission);
                await _unitOfWork.CommitAsync();

                //send email to the user who created the probono application

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                // Send status details email
                string emailBody = $"Your submission for the Notaries Public has been denied. <br/> Reason: {denyNotaryPublicDTO.Reason}";


                BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string> { user.Email }, emailBody, "Pro Bono Application Status"));


                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetNotaryPublicById/{id}")]
        public async Task<IActionResult> GetNotaryPublicById(int id)
        {
            try
            {
                var notaryPublic = await _repositoryManager.NotaryPublicRepository.GetByIdAsync(id);
                if (notaryPublic == null)
                {
                    return NotFound();
                }

                foreach (var attachment in notaryPublic.Attachments)
                {
                    string newFilePath = Path.Combine($"{Lambda.https}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/NotaryPublicAttachments", attachment.FileName);
                    attachment.FilePath = newFilePath;
                }

                var mappedNotaryPublic = _mapper.Map<ReadNotaryPublicDTO>(notaryPublic);
                return Ok(mappedNotaryPublic);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("SubmitNotaryPublicDocument")]
        public async Task<IActionResult> SubmitNotaryPublicDocument([FromForm] CreateNotaryPublicDTO notaryPublicDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var notaryPublic = _mapper.Map<NotaryPublic>(notaryPublicDTO);
                notaryPublic.Status = "Active";

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "NotaryPublic")
                                    ?? new AttachmentType { Name = "NotaryPublic" };

                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                if (notaryPublicDTO.Attachments != null && notaryPublicDTO.Attachments.Count > 0)
                {
                    notaryPublic.Attachments = await SaveAttachmentsAsync(notaryPublicDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.NotaryPublicRepository.AddAsync(notaryPublic);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetNotaryPublicById", new { id = notaryPublic.Id }, notaryPublic);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("RejectNotaryPublicSubmission/{id}")]
        public async Task<IActionResult> RejectNotaryPublicSubmission(int id, [FromBody] string rejectionReason)
        {
            try
            {
                var notaryPublic = await _repositoryManager.NotaryPublicRepository.GetByIdAsync(id);

                if (notaryPublic == null)
                    return NotFound();

                notaryPublic.Status = "Rejected";
                //notaryPublic.RejectionReason = rejectionReason;

                await _repositoryManager.NotaryPublicRepository.UpdateAsync(notaryPublic);
                await _unitOfWork.CommitAsync();

                // Send message to the member
                //var message = $"Your Notary Public submission has been rejected. Reason: {rejectionReason}";
                //await _messagingService.SendMessageAsync(notaryPublic.MemberId, message);

                return Ok("Notary Public submission rejected and member notified.");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotaryPublic(int id, [FromForm] UpdateNotaryPublicDTO notaryPublicDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var notaryPublic = await _repositoryManager.NotaryPublicRepository.GetByIdAsync(id);

                if (notaryPublic == null)
                    return NotFound();

                _mapper.Map(notaryPublicDTO, notaryPublic);

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "NotaryPublic")
                                    ?? new AttachmentType { Name = "NotaryPublic" };

                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                if (notaryPublicDTO.Attachments?.Any() == true)
                {
                    var attachmentsToUpdate = notaryPublicDTO.Attachments.Where(a => a.Length > 0).ToList();

                    if (attachmentsToUpdate.Any())
                    {
                        var attachmentsList = await SaveAttachmentsAsync(attachmentsToUpdate, attachmentType.Id);
                        notaryPublic.Attachments.RemoveAll(a => attachmentsList.Any(b => b.PropertyName == a.PropertyName));
                        notaryPublic.Attachments.AddRange(attachmentsList);
                    }
                }

                await _repositoryManager.NotaryPublicRepository.UpdateAsync(notaryPublic);
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
        public async Task<IActionResult> DeleteNotaryPublic(int id)
        {
            try
            {
                var notaryPublic = await _repositoryManager.NotaryPublicRepository.GetByIdAsync(id);
                if (notaryPublic == null)
                    return NotFound();

                await _repositoryManager.NotaryPublicRepository.DeleteAsync(notaryPublic);
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
                var notaryPublicRecords = await _repositoryManager.NotaryPublicRepository.GetAllAsync();
                var readNotaryPublicRecordsMapped = _mapper.Map<List<ReadNotaryPublicDTO>>(notaryPublicRecords);
                return Ok(readNotaryPublicRecordsMapped);
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

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null or empty");
            }

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/NotaryPublicAttachments");

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
                catch (Exception)
                {
                    throw;
                }
            }

            return attachmentsList;
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            try
            {
                var count = await _repositoryManager.NotaryPublicRepository.GetNotaryPublicCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("getByMemberId/{memberId}")]
        public async Task<IActionResult> GetNotariesPublicByMemberId(int memberId)
        {
            try
            {
                var notariesPublic = await _repositoryManager.NotaryPublicRepository.GetNotariesPublicByMemberIdAsync(memberId);
                if (notariesPublic == null)
                {
                    return NotFound();
                }

                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();

                var validNotariesPublic = notariesPublic.YearOfOperationId == yearOfOperation.Id;

                if(notariesPublic.YearOfOperationId == yearOfOperation.Id){
                    var notaryPublicDTO = _mapper.Map<ReadNotaryPublicDTO>(notariesPublic);

                    return Ok(notaryPublicDTO);

                }
                else{
                    return NotFound($"No active Notaries Public found for member with ID {memberId} for the current year");
                }
                
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
                var notaryPublic = await _repositoryManager.NotaryPublicRepository.GetByIdAsync(id);

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);

                if (notaryPublic != null)
                {
                    if (currentRole.Equals("secretariat", StringComparison.OrdinalIgnoreCase))
                    {
                        notaryPublic.ApplicationStatus = Lambda.Approved;
                        notaryPublic.ApprovedDate = DateTime.UtcNow;
                        await _repositoryManager.NotaryPublicRepository.UpdateAsync(notaryPublic);
                        await _unitOfWork.CommitAsync();

                        string emailBody = $"Your Notaries Public submission has been approved.";

                        BackgroundJob.Enqueue(() => _emailService.SendCPDStatusEmailsAsync(new List<string> { user.Email }, emailBody, "Notary Public Application Status"));
                    }

                    return Ok();
                }
                return BadRequest("Notary Public not found");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
