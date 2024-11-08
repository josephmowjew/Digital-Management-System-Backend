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
    public async Task<IActionResult> CreateGenericSignature([FromBody] SignatureDTO signatureDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var signature = _mapper.Map<GenericSignature>(signatureDTO);
            
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
    public async Task<IActionResult> UpdateGenericSignature(int id, [FromBody] SignatureDTO signatureDTO)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var signature = await _repositoryManager.GenericSignatureRepository.GetByIdAsync(id);
            if (signature == null)
                return NotFound();

            _mapper.Map(signatureDTO, signature);
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
}
