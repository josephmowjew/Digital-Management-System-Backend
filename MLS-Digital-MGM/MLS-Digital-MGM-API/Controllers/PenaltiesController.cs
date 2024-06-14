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
using DataStore.Core.DTOs.Penalty;
using DataStore.Core.DTOs.PenaltyType;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PenaltiesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PenaltiesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPenalties(int pageNumber = 1, int pageSize = 10, int memberId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
               
                var pagingParameters = new PagingParameters<Penalty>
                {

                    Predicate = u => u.Status != Lambda.Deleted && (memberId > 0 ? u.MemberId == memberId : true),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Penalty, object>>[] {
                        p => p.Member,
                        p => p.CreatedBy,
                        p => p.PenaltyType,
                        p => p.Attachments
                    }
                };

                var penaltiesPaged = await _repositoryManager.PenaltyRepository.GetPagedAsync(pagingParameters);

                if (penaltiesPaged == null || !penaltiesPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadPenaltyDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadPenaltyDTO>());
                }

                var penaltyDTOs = _mapper.Map<List<ReadPenaltyDTO>>(penaltiesPaged);

                 foreach (var report in penaltyDTOs)
            {
                foreach (var attachment in report.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;


                      string newfilePath = Path.Combine("/uploads/PenaltyAttachments/", attachment.FileName);

                    attachment.FilePath = newfilePath;
                }
            }

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = penaltyDTOs.Count;
                    var totalRecords = await _repositoryManager.PenaltyRepository.CountAsync(pagingParameters);
                    

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = penaltyDTOs.ToList()
                    });
                }

                return Ok(penaltyDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPenalty([FromForm] CreatePenaltyDTO penaltyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var penalty = _mapper.Map<Penalty>(penaltyDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                penalty.CreatedById = user.Id;

                //assign amount remaining equal to penalty fee on creation
                penalty.AmountRemaining = penalty.Fee;

                 // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Penalty") 
                                    ?? new AttachmentType { Name = "Penalty" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }
                if (penaltyDTO.Attachments != null && penaltyDTO.Attachments.Count > 0)
                {
                    penalty.Attachments = await SaveAttachmentsAsync(penaltyDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.PenaltyRepository.AddAsync(penalty);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetPenaltyById", new { id = penalty.Id }, penalty);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetPenaltyById/{id}")]
        public async Task<IActionResult> GetPenaltyById(int id)
        {
            try
            {
                var penalty = await _repositoryManager.PenaltyRepository.GetByIdAsync(id);
                if (penalty == null)
                {
                    return NotFound();
                }

                foreach (var attachment in penalty.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;

                    string newFilePath = Path.Combine($"http://{HttpContext.Request.Host}/uploads/{Lambda.PenaltyFolderName}", attachment.FileName);

                    attachment.FilePath = newFilePath;

                }

                var mappedPenalty = _mapper.Map<ReadPenaltyDTO>(penalty);
                return Ok(mappedPenalty);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetPEnaltyByMemberId/{id}")]
        public async Task<IActionResult> GetPenaltyByMemberId(int id)
        {
            try
            {
                var penalty = await _repositoryManager.PenaltyRepository.GetByIdAsync(id);
                if (penalty == null)
                {
                    return NotFound();
                }

                foreach (var attachment in penalty.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;

                    string newFilePath = Path.Combine($"http://{HttpContext.Request.Host}/uploads/{Lambda.PenaltyFolderName}", attachment.FileName);

                    attachment.FilePath = newFilePath;

                }

                var mappedPenalty = _mapper.Map<ReadPenaltyDTO>(penalty);
                return Ok(mappedPenalty);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePenalty(int id, [FromForm] UpdatePenaltyDTO penaltyDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                var penalty = await _repositoryManager.PenaltyRepository.GetByIdAsync(id);

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                //penalty.CreatedById = user.Id;

                if (penalty == null)
                    return NotFound();

                _mapper.Map(penaltyDTO, penalty);

                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Penalty") 
                                    ?? new AttachmentType { Name = "Penalty" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }
                if (penaltyDTO.Attachments != null && penalty.Attachments.Count > 0)
                {
                    penalty.Attachments = await SaveAttachmentsAsync(penaltyDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.PenaltyRepository.UpdateAsync(penalty);
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
        public async Task<IActionResult> DeletePenalty(int id)
        {
            try
            {
                var penalty = await _repositoryManager.PenaltyRepository.GetByIdAsync(id);
                if (penalty == null)
                    return NotFound();

                await _repositoryManager.PenaltyRepository.DeleteAsync(penalty);
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
                var penaltyRecords = await this._repositoryManager.PenaltyRepository.GetAllAsync();

                var readPenaltyRecordsMapped = this._mapper.Map<List<ReadPenaltyDTO>>(penaltyRecords);

                return Ok(readPenaltyRecordsMapped);
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

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/PenaltyAttachments" );

           

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
