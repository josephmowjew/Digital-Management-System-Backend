using AutoMapper;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        // Constructor
        public YearOfOperationsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // HTTP GET to retrieve paged year of operations
        [HttpGet("paged")]
        public async Task<IActionResult> GetYearOfOperations(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Retrieve paged year of operations from repository
                var yearOfOperations = await _repositoryManager.YearOfOperationRepository.GetPagedAsync(c => true, pageNumber, pageSize);

                if (yearOfOperations == null || !yearOfOperations.Any())
                {
                    return NotFound();
                }

                // Map year of operations to DTO
                var mappedYearOfOperations = _mapper.Map<IEnumerable<ReadYearOfOperationDTO>>(yearOfOperations);

                // Return OK with mapped year of operations
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

                var existingYearOfOperation = await _repositoryManager.YearOfOperationRepository.GetAsync(c => c.StartDate.Date.Equals(yearOfOperation.EndDate.Date));

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
    }
}
