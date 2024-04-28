using AutoMapper;
using DataStore.Core.DTOs.Firms;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;

[Route("api/[controller]")]
public class FirmsController : Controller
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IErrorLogService _errorLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private IHttpContextAccessor _httpContextAccessor;


    public FirmsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _repositoryManager = repositoryManager;
        _errorLogService = errorLogService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetFirms(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            // Create a new DataTablesParameters object
            var dataTableParams = new DataTablesParameters();

            var pagingParameters = new PagingParameters<Firm>
            {
                Predicate = u => u.Status != Lambda.Deleted,
                PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
            };

            // Fetch paginated firms using the FirmRepository
            var pagedFirms = await _repositoryManager.FirmRepository.GetPagedAsync(pagingParameters);

            // Check if roles exist
            if (pagedFirms == null || !pagedFirms.Any())
            {
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    return Json(new
                    {
                        draw,
                        recordsFiltered = 0,
                        recordsTotal = 0,
                        data = Enumerable.Empty<ReadFirmDTO>()
                    });
                }
                return Ok(Enumerable.Empty<ReadFirmDTO>()); // Return empty list
            }

            // Map the Roles to a list of ReadFirmDTOs
            var mappedFirms = _mapper.Map<List<ReadFirmDTO>>(pagedFirms);

            // Return datatable JSON if the request came from a datatable
            if (dataTableParams.LoadFromRequest(_httpContextAccessor))
            {
                var draw = dataTableParams.Draw;
                var resultTotalFiltred = mappedFirms.Count;

                return Json(new
                {
                    draw,
                    recordsFiltered = resultTotalFiltred,
                    recordsTotal = resultTotalFiltred,
                    data = mappedFirms.ToList() // Materialize the enumerable
                });
            }


            // Return an Ok result with the mapped Roles
            return Ok(mappedFirms);

        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddFirm([FromBody] CreateFirmDTO firmDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firm = _mapper.Map<Firm>(firmDTO);

            var existingFirm = await _repositoryManager.FirmRepository.GetAsync(f => f.Name.Trim().Equals(firm.Name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existingFirm != null)
            {
                ModelState.AddModelError(nameof(firmDTO.Name), "A firm with the same name already exists");
                return BadRequest(ModelState);
            }

            await _repositoryManager.FirmRepository.AddAsync(firm);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction("GetFirms", new { id = firm.Id }, firm);
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFirm(int id, [FromBody] UpdateFirmDTO firmDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firm = await _repositoryManager.FirmRepository.GetByIdAsync(id);
            if (firm == null)
            {
                return NotFound();
            }

            _mapper.Map(firmDTO, firm);
            await _repositoryManager.FirmRepository.UpdateAsync(firm);
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
    public async Task<IActionResult> DeleteFirm(int id)
    {
        try
        {
            var firm = await _repositoryManager.FirmRepository.GetByIdAsync(id);
            if (firm == null)
            {
                return NotFound();
            }

            await _repositoryManager.FirmRepository.DeleteAsync(firm);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("getfirm/{id}")]
    public async Task<IActionResult> GetFirmById(int id)
    {
        try
        {
            var firm = await _repositoryManager.FirmRepository.GetByIdAsync(id);
            if (firm == null)
            {
                return NotFound();
            }
            var firmDTO = _mapper.Map<ReadFirmDTO>(firm);
            return Ok(firmDTO);
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }
}