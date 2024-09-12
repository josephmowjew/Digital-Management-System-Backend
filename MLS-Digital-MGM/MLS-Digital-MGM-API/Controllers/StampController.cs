
using AutoMapper;
using DataStore.Core.DTOs.Stamp;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class StampController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public StampController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> AddStamp([FromForm] CreateStampDTO stampDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var stamp = _mapper.Map<Stamp>(stampDTO);

                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Stamp") 
                                    ?? new AttachmentType { Name = "Stamp" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }
                if (stampDTO.Attachments != null && stampDTO.Attachments.Count > 0)
                {
                    stamp.Attachments = await SaveAttachmentsAsync(stampDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.StampRepository.AddAsync(stamp);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetStamps", new { id = stamp.Id }, stamp);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetStamps(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
            
                var pagingParameters = new PagingParameters<Stamp>
                {
                    Predicate = s => s.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Stamp, object>>[] {
                        p => p.YearOfOperation,
                    },
                };

                var pagedStamps = await _repositoryManager.StampRepository.GetPagedAsync(pagingParameters);

                if (pagedStamps == null || !pagedStamps.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadStampDTO>(),
                        });
                    }
                    return Ok(Enumerable.Empty<ReadStampDTO>());
                }

                var mappedStamps = _mapper.Map<List<ReadStampDTO>>(pagedStamps);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = mappedStamps.Count;
                    var totalRecords = await _repositoryManager.StampRepository.CountAsync(pagingParameters);

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = totalRecords, 
                        recordsTotal = totalRecords, 
                        data = mappedStamps.ToList()
                    });
                }

                return Ok(mappedStamps);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStamp(int id)
        {
            try
            {   
                var stamp = await _repositoryManager.StampRepository.GetStampByIdAsync(id);
                if (stamp == null)
                {
                    return NotFound();
                }

                await _repositoryManager.StampRepository.DeleteAsync(stamp);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditStamp(int id, [FromForm] UpdateStampDTO stampDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                var stamp = await _repositoryManager.StampRepository.GetStampByIdAsync(id);
                if (stamp == null)
                {
                    return NotFound();
                }

                _mapper.Map(stampDTO, stamp);

                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Penalty") 
                                    ?? new AttachmentType { Name = "Penalty" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }
                if (stampDTO.Attachments != null && stamp.Attachments.Count > 0)
                {
                    stamp.Attachments = await SaveAttachmentsAsync(stampDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.StampRepository.UpdateAsync(stamp);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("singleStamp/{id}")]
        public async Task<IActionResult> GetStampById(int id)
        {
            try
            {
                var stamp = await _repositoryManager.StampRepository.GetStampByIdAsync(id);
                if (stamp == null)
                {
                    return NotFound();
                }

                foreach (var attachment in stamp.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;
                    string newFilePath = Path.Combine($"http://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.StampFolderName}", attachment.FileName);
                    attachment.FilePath = newFilePath;

                }

                var readStampDTO = _mapper.Map<ReadStampDTO>(stamp);

                return Ok(readStampDTO);
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

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/StampAttachments" );

           

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
