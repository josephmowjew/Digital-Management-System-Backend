using System.Linq.Expressions;
using AutoMapper;
using DataStore.Core.DTOs.User;
using DataStore.Core.Models;
using DataStore.Core.Services;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class GenericSignaturesController : Controller
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IErrorLogService _errorLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SignatureService _signatureService;

    public GenericSignaturesController(
        IRepositoryManager repositoryManager,
        IErrorLogService errorLogService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        SignatureService signatureService)
    {
        _repositoryManager = repositoryManager;
        _errorLogService = errorLogService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _signatureService = signatureService;
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetGenericSignatures(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var dataTableParams = new DataTablesParameters();
            
            var pagingParameters = new PagingParameters<GenericSignature>
            {
                PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
            };

            var signatures = await _repositoryManager.GenericSignatureRepository.GetPagedAsync(pagingParameters);

            if (signatures == null || !signatures.Any())
            {
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    return Json(new { draw = dataTableParams.Draw, recordsFiltered = 0, recordsTotal = 0, data = Enumerable.Empty<SignatureDTO>() });
                }
                return Ok(Enumerable.Empty<SignatureDTO>());
            }

            var mappedSignatures = _mapper.Map<List<SignatureDTO>>(signatures);

            if (dataTableParams.LoadFromRequest(_httpContextAccessor))
            {
                var totalRecords = await _repositoryManager.GenericSignatureRepository.CountAsync(pagingParameters);
                return Json(new
                {
                    draw = dataTableParams.Draw,
                    recordsFiltered = totalRecords,
                    recordsTotal = totalRecords,
                    data = mappedSignatures
                });
            }

            return Ok(mappedSignatures);
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveGenericSignature()
    {
        try
        {
            var signature = await _repositoryManager.GenericSignatureRepository.GetSingleAsync(s => s.IsActive);
            if (signature == null)
                return NotFound("No active generic signature found");

            return Ok(_mapper.Map<SignatureDTO>(signature));
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateGenericSignature([FromForm] SignatureDTO signatureDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var signature = _mapper.Map<GenericSignature>(signatureDTO);
            
            // Get or create attachment type
            var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Signature") 
                                ?? new AttachmentType { Name = "Signature" };

            if (attachmentType.Id == 0)
            {
                await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                await _unitOfWork.CommitAsync();
            }

            // Handle attachments
            if (signatureDTO.Attachments?.Any() == true)
            {
                var attachmentsToUpdate = signatureDTO.Attachments.Where(a => a.Length > 0).ToList();
                if (attachmentsToUpdate.Any())
                {
                    signature.Attachments = await SaveAttachmentsAsync(attachmentsToUpdate, attachmentType.Id);
                }
            }

            // Deactivate current active signature if exists
            var currentActive = await _repositoryManager.GenericSignatureRepository.GetSingleAsync(s => s.IsActive);
            if (currentActive != null)
            {
                currentActive.IsActive = false;
                currentActive.UpdatedDate = DateTime.UtcNow;
                await _repositoryManager.GenericSignatureRepository.UpdateAsync(currentActive);
            }

            await _repositoryManager.GenericSignatureRepository.AddAsync(signature);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetActiveGenericSignature), _mapper.Map<SignatureDTO>(signature));
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGenericSignature(int id, [FromForm] SignatureDTO signatureDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var signature = await _repositoryManager.GenericSignatureRepository.GetSingleAsync(
                s => s.Id == id,
                new Expression<Func<GenericSignature, object>>[] { s => s.Attachments }
            );
            if (signature == null)
                return NotFound();

            var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Signature") 
                                ?? new AttachmentType { Name = "Signature" };

            if (attachmentType.Id == 0)
            {
                await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                await _unitOfWork.CommitAsync();
            }

            if (signatureDTO.Attachments?.Any() == true)
            {
                var attachmentsToUpdate = signatureDTO.Attachments.Where(a => a.Length > 0).ToList();
                if (attachmentsToUpdate.Any())
                {
                    var attachmentsList = await SaveAttachmentsAsync(attachmentsToUpdate, attachmentType.Id);
                    
                    // Remove old attachments with same property name
                    signature.Attachments.RemoveAll(a => attachmentsList.Any(b => b.PropertyName == a.PropertyName));
                    
                    // Add new attachments
                    signature.Attachments.AddRange(attachmentsList);
                }
            }

            var currentIsActive = signature.IsActive;  // Store current IsActive state
            _mapper.Map(signatureDTO, signature);
            signature.IsActive = currentIsActive;      // Restore IsActive state
            signature.UpdatedDate = DateTime.UtcNow;

            await _repositoryManager.GenericSignatureRepository.UpdateAsync(signature);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("activate/{id}")]
    public async Task<IActionResult> SetActiveSignature(int id)
    {
        try
        {
            var signature = await _repositoryManager.GenericSignatureRepository.GetByIdAsync(id);
            if (signature == null)
                return NotFound();

            // Deactivate current active signature
            var currentActive = await _repositoryManager.GenericSignatureRepository.GetSingleAsync(s => s.IsActive);
            if (currentActive != null)
            {
                currentActive.IsActive = false;
                currentActive.UpdatedDate = DateTime.UtcNow;
                await _repositoryManager.GenericSignatureRepository.UpdateAsync(currentActive);
            }

            // Activate new signature
            signature.IsActive = true;
            signature.UpdatedDate = DateTime.UtcNow;
            await _repositoryManager.GenericSignatureRepository.UpdateAsync(signature);
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
    public async Task<IActionResult> DeleteGenericSignature(int id)
    {
        try
        {
            var signature = await _repositoryManager.GenericSignatureRepository.GetByIdAsync(id);
            if (signature == null)
                return NotFound();

            if (signature.IsActive)
                return BadRequest("Cannot delete the active generic signature");

            await _repositoryManager.GenericSignatureRepository.DeleteAsync(signature);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenericSignature(int id)
    {
        try
        {
            var signature = await _repositoryManager.GenericSignatureRepository.GetSingleAsync(s => s.Id == id, new Expression<Func<GenericSignature, object>>[] { s => s.Attachments });
            if (signature == null)
                return NotFound();

            var signatureDto = _mapper.Map<SignatureDTO>(signature);
            
            // Set banner image URL if attachment exists
            var bannerAttachment = signature.Attachments?.FirstOrDefault(a => a.PropertyName == "Banner");
            if (bannerAttachment != null)
            {
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                signatureDto.BannerImageUrl = $"{baseUrl}/Uploads/SignatureAttachments/{bannerAttachment.FileName}";
            }

            return Ok(signatureDto);
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

        var attachmentsPath = Path.Combine(webRootPath, "Uploads/SignatureAttachments");

        if (!Directory.Exists(attachmentsPath))
        {
            Directory.CreateDirectory(attachmentsPath);
        }

        foreach (var attachment in attachments)
        {
            if (attachment == null || string.IsNullOrWhiteSpace(attachment.FileName))
                continue;

            var uniqueFileName = FileNameGenerator.GenerateUniqueFileName(attachment.FileName);
            var filePath = Path.Combine(attachmentsPath, uniqueFileName);

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
                    PropertyName = "Banner"
                });
            }
            catch
            {
                throw;
            }
        }

        return attachmentsList;
    }
}
