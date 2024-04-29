using AutoMapper;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using MLS_Digital_MGM.DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class YearOfOperationsController : Controller
    {
        // Dependency Injection
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        // Constructor
        public YearOfOperationsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // HTTP GET to retrieve paged year of operations
        [HttpGet("paged")]
        public async Task<IActionResult> GetYearOfOperations(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<YearOfOperation>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated yearOfOperations using the YearOfOperationRepository
                var pagedYearOfOperations = await _repositoryManager.YearOfOperationRepository.GetPagedAsync(pagingParameters);

                // Check if roles exist
                if (pagedYearOfOperations == null || !pagedYearOfOperations.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadYearOfOperationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadYearOfOperationDTO>()); // Return empty list
                }

                // Map the Roles to a list of ReadYearOfOperationDTOs
                var mappedYearOfOperations = _mapper.Map<List<ReadYearOfOperationDTO>>(pagedYearOfOperations);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedYearOfOperations.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltred,
                        recordsTotal = resultTotalFiltred,
                        data = mappedYearOfOperations.ToList() // Materialize the enumerable
                    });
                }


                // Return an Ok result with the mapped Roles
                return Ok(mappedYearOfOperations);

            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // HTTP POST to add year of operation
        [HttpPost]
        public async Task<IActionResult> AddYearOfOperation([FromBody] CreateYearOfOperationDTO yearOfOperationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var yearOfOperation = _mapper.Map<YearOfOperation>(yearOfOperationDTO);

                var existingYearOfOperation = await _repositoryManager.YearOfOperationRepository.GetAsync(c => c.StartDate.Equals(yearOfOperation.EndDate));

                if (existingYearOfOperation != null)
                {
                    ModelState.AddModelError(nameof(yearOfOperationDTO.StartDate), "A year of operation with the same year already exists");
                    return BadRequest(ModelState);
                }

                await _repositoryManager.YearOfOperationRepository.AddAsync(yearOfOperation);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetYearOfOperations", new { id = yearOfOperation.Id }, yearOfOperation);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // HTTP PUT to update year of operation
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateYearOfOperation(int id, [FromBody] UpdateYearOfOperationDTO yearOfOperationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetByIdAsync(id);
                if (yearOfOperation == null)
                {
                    return NotFound();
                }

                _mapper.Map(yearOfOperationDTO, yearOfOperation);

                await _repositoryManager.YearOfOperationRepository.UpdateAsync(yearOfOperation);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // HTTP DELETE to delete year of operation
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteYearOfOperation(int id)
        {
            try
            {
                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetByIdAsync(id);
                if (yearOfOperation == null)
                {
                    return NotFound();
                }

                await _repositoryManager.YearOfOperationRepository.DeleteAsync(yearOfOperation);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getyearofoperation/{id}")]
        public async Task<IActionResult> GetYearOfOperationById(int id)
        {
            try
            {
                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetByIdAsync(id);
                if (yearOfOperation == null)
                {
                    return NotFound();
                }

                var yearOfOperationDTO = _mapper.Map<ReadYearOfOperationDTO>(yearOfOperation);

                return Ok(yearOfOperationDTO);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
