using AutoMapper;
using DataStore.Core.DTOs.InstitutionType;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLS_Digital_MGM_API.Controllers
{
    // Controller for handling requests related to institution types
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InstitutionTypesController : Controller
    {
        // Dependency Injection of required services
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor for dependency injection
        public InstitutionTypesController(IRepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Action for retrieving a paged list of institution types
        [HttpGet("paged")]
        public async Task<IActionResult> GetInstitutionTypes(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                // Create PagingParameters object
                var pagingParameters = new PagingParameters<InstitutionType>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                };
                // Get paged list of institution types from repository
                var institutionTypes = await _repositoryManager.InstitutionTypeRepository.GetPagedAsync(pagingParameters);

                // If no institution types found, return NotFound result
                if (institutionTypes == null || !institutionTypes.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadInstitutionTypeDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadInstitutionTypeDTO>()); // Return empty list
                }

                // Map the Roles to a list of ReadFirmDTOs
                var mappedInstitutionTypes = _mapper.Map<List<ReadInstitutionTypeDTO>>(institutionTypes);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedInstitutionTypes.Count;
                    var totalRecords = await _repositoryManager.InstitutionTypeRepository.CountAsync(pagingParameters);


                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = mappedInstitutionTypes.ToList() // Materialize the enumerable
                    });
                }


                // Return an Ok result with the mapped Roles
                return Ok(mappedInstitutionTypes);
            }
            catch (Exception ex)
            {
                // Log error and return Internal Server Error result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllInstitutionTypes()
        {
            try
            {

                // Get paged list of institution types from repository
                var institutionTypes = await _repositoryManager.InstitutionTypeRepository.GetAllAsync();

                // If no institution types found, return NotFound result
                if (institutionTypes == null || !institutionTypes.Any())
                {
                    return NotFound();
                }

                // Map institution types to DTOs and return as Ok result
                var mappedinstitutionTypes = _mapper.Map<IEnumerable<ReadInstitutionTypeDTO>>(institutionTypes);

                return Ok(mappedinstitutionTypes);
            }
            catch (Exception ex)
            {
                // Log error and return Internal Server Error result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Action for adding a new institution type
        [HttpPost]
        public async Task<IActionResult> AddinstitutionType([FromBody] CreateInstitutionTypeDTO institutionTypeDTO)
        {
            try
            {
                // If model state is invalid, return BadRequest result with invalid fields
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map DTO to new institution type and add to repository
                var institutionType = _mapper.Map<InstitutionType>(institutionTypeDTO);
                await _repositoryManager.InstitutionTypeRepository.AddAsync(institutionType);
                await _unitOfWork.CommitAsync();

                // Return Created result with location of new institution type
                return CreatedAtAction("GetInstitutionTypes", new { id = institutionType.Id }, institutionType);
            }
            catch (Exception ex)
            {
                // Log error and return Internal Server Error result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstitutionTypeById(int id)
        {
            try
            {
                var institutionType = await _repositoryManager.InstitutionTypeRepository.GetByIdAsync(id);
                if (institutionType == null)
                {
                    return NotFound();
                }

                var mappedInstitutionType = _mapper.Map<ReadInstitutionTypeDTO>(institutionType);

                return Ok(mappedInstitutionType);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Action for updating an existing institution type
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateinstitutionType(int id, [FromBody] UpdateInstitutionTypeDTO institutionTypeDTO)
        {
            try
            {
                // If model state is invalid, return BadRequest result with invalid fields
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get institution type by id from repository
                var institutionType = await _repositoryManager.InstitutionTypeRepository.GetByIdAsync(id);
                if (institutionType == null)
                {
                    return NotFound();
                }

                // Map DTO to existing institution type and update repository
                _mapper.Map(institutionTypeDTO, institutionType);
                await _repositoryManager.InstitutionTypeRepository.UpdateAsync(institutionType);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log error and return Internal Server Error result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Action for deleting an existing institution type
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteinstitutionType(int id)
        {
            try
            {
                // Get institution type by id from repository
                var institutionType = await _repositoryManager.InstitutionTypeRepository.GetByIdAsync(id);
                if (institutionType == null)
                {
                    return NotFound();
                }

                // Delete institution type from repository
                await _repositoryManager.InstitutionTypeRepository.DeleteAsync(institutionType);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log error and return Internal Server Error result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }


}