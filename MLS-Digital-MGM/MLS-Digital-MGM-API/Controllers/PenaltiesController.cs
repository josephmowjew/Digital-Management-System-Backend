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
        public async Task<IActionResult> GetPenalties(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Penalty>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Penalty, object>>[] {
                        p => p.CreatedBy,
                        p => p.PenaltyType
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

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = penaltyDTOs.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltered,
                        recordsTotal = resultTotalFiltered,
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

        private async Task<List<Attachment>> SaveAttachmentsAsync(IEnumerable<IFormFile> attachments, int attachmentTypeId)
        {
            var attachmentsList = new List<Attachment>();
            var hostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var webRootPath = hostEnvironment.WebRootPath;
            var proBonoReportAttachmentsPath = Path.Combine(webRootPath, "Uploads/PenaltyAttachments");
        
            Directory.CreateDirectory(proBonoReportAttachmentsPath);
        
            foreach (var attachment in attachments)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(attachment.FileName)}";
                var filePath = Path.Combine(proBonoReportAttachmentsPath, uniqueFileName);
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

    }

}
