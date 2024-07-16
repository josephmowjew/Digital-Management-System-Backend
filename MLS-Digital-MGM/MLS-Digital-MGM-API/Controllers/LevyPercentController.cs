using AutoMapper;
using DataStore.Core.DTOs.LevyPercent;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LevyPercentController : Controller
    {
        // Dependency Injection
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor
        public LevyPercentController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // HTTP GET to retrieve paged levy percents
        [HttpGet("paged")]
        public async Task<IActionResult> GetLevyPercents(int pageNumber = 1, int pageSize = 10)
        {
            // Implementation similar to GetYearOfOperations in YearOfOperationsController
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<LevyPercent>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated levyPercents using the LevyPercentRepository
                var pagedLevyPercents = await _repositoryManager.LevyPercentRepository.GetPagedAsync(pagingParameters);

                // Check if levyPercents exist
                if (pagedLevyPercents == null || !pagedLevyPercents.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadLevyPercentDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadLevyPercentDTO>()); // Return empty list
                }

                // Map the LevyPercents to a list of ReadLevyPercentDTOs
                var mappedLevyPercents = _mapper.Map<List<ReadLevyPercentDTO>>(pagedLevyPercents);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedLevyPercents.Count;
                    var totalRecords = await _repositoryManager.LevyPercentRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = mappedLevyPercents.ToList() // Materialize the enumerable
                    });
                }

                // Return an Ok result with the mapped LevyPercents
                return Ok(mappedLevyPercents);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllLevyPercents()
        {
            // Implementation similar to GetAllYearOfOperations in YearOfOperationsController
            try
            {

                // Get paged list of levy percents from repository
                var levyPercents = await _repositoryManager.LevyPercentRepository.GetAllAsync();

                // If no levy percents found, return NotFound result
                if (levyPercents == null || !levyPercents.Any())
                {
                    return NotFound();
                }

                // Map levy percents to DTOs and return as Ok result
                var mappedLevyPercents = _mapper.Map<IEnumerable<ReadLevyPercentDTO>>(levyPercents);

                return Ok(mappedLevyPercents);
            }
            catch (Exception ex)
            {
                // Log error and return Internal Server Error result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }

        }

        // HTTP POST to add levy percent
        [HttpPost]
        public async Task<IActionResult> AddLevyPercent([FromBody] CreateLevyPercentDTO levyPercentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var levyPercent = _mapper.Map<LevyPercent>(levyPercentDTO);

                var existingLevyPercent = await _repositoryManager.LevyPercentRepository.GetAsync(c => c.PercentageValue.Equals(levyPercent.PercentageValue));

                if (existingLevyPercent != null)
                {
                    ModelState.AddModelError(nameof(levyPercentDTO.PercentageValue), "A levy percent already exists");
                    return BadRequest(ModelState);
                }

                await _repositoryManager.LevyPercentRepository.AddAsync(levyPercent);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetLevyPercents", new { id = levyPercent.Id }, levyPercent);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }

        }

        // HTTP PUT to update levy percent
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLevyPercent(int id, [FromBody] UpdateLevyPercentDTO levyPercentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var levyPercent = await _repositoryManager.LevyPercentRepository.GetByIdAsync(id);
                if (levyPercent == null)
                {
                    return NotFound();
                }

                _mapper.Map(levyPercentDTO, levyPercent);

                await _repositoryManager.LevyPercentRepository.UpdateAsync(levyPercent);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // HTTP DELETE to delete levy percent
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevyPercent(int id)
        {
            try
            {
                var levyPercent = await _repositoryManager.LevyPercentRepository.GetByIdAsync(id);
                if (levyPercent == null)
                {
                    return NotFound();
                }

                await _repositoryManager.LevyPercentRepository.DeleteAsync(levyPercent);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getlevypercent/{id}")]
        public async Task<IActionResult> GetLevyPercentById(int id)
        {
            try
            {
                var levyPercent = await _repositoryManager.LevyPercentRepository.GetByIdAsync(id);
                if (levyPercent == null)
                {
                    return NotFound();
                }

                var levyPercentDTO = _mapper.Map<ReadLevyPercentDTO>(levyPercent);

                return Ok(levyPercentDTO);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
