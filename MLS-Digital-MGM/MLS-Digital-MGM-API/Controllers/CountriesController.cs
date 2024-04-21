using AutoMapper;
using DataStore.Core.DTOs.Country;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager; // Interface to the data layer
        private readonly IErrorLogService _errorLogService; // Interface to the error logging service
        private readonly IUnitOfWork _unitOfWork; // Interface to the unit of work pattern
        private readonly IMapper _mapper; // Interface to the object-to-object mapping service

        /// <summary>
        /// Constructor for the CountriesController
        /// </summary>
        /// <param name="repositoryManager">Interface to the data layer</param>
        /// <param name="errorLogService">Interface to the error logging service</param>
        /// <param name="unitOfWork">Interface to the unit of work pattern</param>
        /// <param name="mapper">Interface to the object-to-object mapping service</param>
        public CountriesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetCountries(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                 // Create PagingParameters object
                var pagingParameters = new PagingParameters<Country>{
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Predicate = c => true
                    //SearchTerm = null

                };

                // Get paged data from the data layer
                var countries = await _repositoryManager.CountryRepository.GetPagedAsync(pagingParameters);

                if (countries == null || !countries.Any())
                {
                    return NotFound();
                }

                // Map the data to a DTO
                var mappedCountries = _mapper.Map<IEnumerable<ReadCountryDTO>>(countries);

                return Ok(mappedCountries);

            }
            catch (Exception ex)
            {
                // Log the error and return an error response
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry([FromBody] CreateCountryDTO countryDTO)
        {
            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map the DTO to a model
                var country = _mapper.Map<Country>(countryDTO);

                // Check if a country with the same name already exists
                var existingCountry = await _repositoryManager.CountryRepository.GetAsync(c => c.Name.Trim().Equals(country.Name.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existingCountry != null)
                {
                    ModelState.AddModelError(nameof(countryDTO.Name), "A country with the same name already exists");
                    return BadRequest(ModelState);
                }

                // Add the country to the data layer
                await _repositoryManager.CountryRepository.AddAsync(country);
                await _unitOfWork.CommitAsync();

                // Return a created response with the created country
                return CreatedAtAction("GetCountries", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                // Log the error and return an error response
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get the country from the data layer
                var country = await _repositoryManager.CountryRepository.GetByIdAsync(id);
                if (country == null)
                {
                    return NotFound();
                }

                // Map the DTO to the country
                _mapper.Map(countryDTO, country);

                // Update the country in the data layer
                await _repositoryManager.CountryRepository.UpdateAsync(country);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the error and return an error response
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            try
            {
                // Get the country from the data layer
                var country = await _repositoryManager.CountryRepository.GetByIdAsync(id);
                if (country == null)
                {
                    return NotFound();
                }

                // Delete the country from the data layer
                await _repositoryManager.CountryRepository.DeleteAsync(country);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the error and return an error response
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}