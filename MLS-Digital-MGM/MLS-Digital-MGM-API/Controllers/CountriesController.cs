using AutoMapper;
using DataStore.Core.DTOs.Country;
using DataStore.Core.DTOs.Country;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager; // Interface to the data layer
        private readonly IErrorLogService _errorLogService; // Interface to the error logging service
        private readonly IUnitOfWork _unitOfWork; // Interface to the unit of work pattern
        private readonly IMapper _mapper; // Interface to the object-to-object mapping service
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor for the CountriesController
        /// </summary>
        /// <param name="repositoryManager">Interface to the data layer</param>
        /// <param name="errorLogService">Interface to the error logging service</param>
        /// <param name="unitOfWork">Interface to the unit of work pattern</param>
        /// <param name="mapper">Interface to the object-to-object mapping service</param>
        public CountriesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        

       [HttpGet("paged")]
       public async Task<IActionResult> GetCountries(int pageNumber = 1, int pageSize = 10)
       {
           try
           {
                // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Country>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated countries using the CountryRepository
                var pagedCountries = await _repositoryManager.CountryRepository.GetPagedAsync(pagingParameters);

                // Check if roles exist
                if (pagedCountries == null || !pagedCountries.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadCountryDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadCountryDTO>()); // Return empty list
                }

                // Map the Roles to a list of ReadCountryDTOs
                var mappedCountries = _mapper.Map<List<ReadCountryDTO>>(pagedCountries);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedCountries.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltred,
                        recordsTotal = resultTotalFiltred,
                        data = mappedCountries.ToList() // Materialize the enumerable
                    });
                }


                // Return an Ok result with the mapped Roles
                return Ok(mappedCountries);

            }
           catch (Exception ex)
           {
               await _errorLogService.LogErrorAsync(ex);
               return StatusCode(500, "Internal server error");
           }
       }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
               
                // Fetch paginated countries using the DepartmentRepository
                var countries = await _repositoryManager.CountryRepository.GetAllAsync();

                // Check if countries exist
                if (countries == null || !countries.Any())
                {
                    return Ok(Enumerable.Empty<ReadCountryDTO>()); // Return 404 Not Found if no departments are found
                }

                // Map countries entities to ReadCountryDTO
                var mappedDepartments = _mapper.Map<IEnumerable<ReadCountryDTO>>(countries);

                return Ok(mappedDepartments); // Return paginated countries

            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
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