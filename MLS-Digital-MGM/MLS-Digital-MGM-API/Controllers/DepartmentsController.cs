using AutoMapper;
using DataStore.Core.DTOs.Department;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class DepartmentsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        public DepartmentsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // Add a method to fetch paginated departments
        [HttpGet("paged")]
        public async Task<IActionResult> GetDepartments(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Create a new DataTablesParameters object
                 var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Department>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated departments using the DepartmentRepository
                var pagedDepartments = await _repositoryManager.DepartmentRepository.GetPagedAsync(pagingParameters);

                // Check if roles exist
                if (pagedDepartments == null || !pagedDepartments.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadDepartmentDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadDepartmentDTO>()); // Return empty list
                }

                // Map the Roles to a list of ReadDepartmentDTOs
                var mappedDepartments = _mapper.Map<List<ReadDepartmentDTO>>(pagedDepartments);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedDepartments.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltred,
                        recordsTotal = resultTotalFiltred,
                        data = mappedDepartments.ToList() // Materialize the enumerable
                    });
                }


                // Return an Ok result with the mapped Roles
                return Ok(mappedDepartments);

            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {

                // Fetch paginated departments using the DepartmentRepository
                var departments = await _repositoryManager.DepartmentRepository.GetAllAsync();

                // Check if departments exist
                if (departments == null || !departments.Any())
                {
                    return Ok(Enumerable.Empty<ReadDepartmentDTO>()); // Return 404 Not Found if no departments are found
                }

                // Map Department entities to ReadDepartmentDTOs
                var mappedDepartments = _mapper.Map<IEnumerable<ReadDepartmentDTO>>(departments);

                return Ok(mappedDepartments); // Return paginated departments

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
        public async Task<IActionResult> AddDepartment([FromBody] CreateDepartmentDTO departmentDTO)
        {
            try
            {
                // Check ModelState.IsValid for validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map the DTO to Department entity
                var department = _mapper.Map<Department>(departmentDTO);

                //check if there isn't a department with the name already

                // Check if there isn't a department with the same name already
                var existingDepartment = await _repositoryManager.DepartmentRepository.GetAsync(d => d.Name.Trim().Equals(department.Name.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existingDepartment != null)
                {
                    ModelState.AddModelError(nameof(departmentDTO.Name), "A department with the same name already exists");
                    return BadRequest(ModelState);
                }

                // Add the department using the DepartmentRepository
                await _repositoryManager.DepartmentRepository.AddAsync(department);
                await _unitOfWork.CommitAsync(); // Save changes to the database

                return CreatedAtAction("GetDepartments", new { id = department.Id }, department);
            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/departments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDTO departmentDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var department = await _repositoryManager.DepartmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    return NotFound();
                }

                _mapper.Map(departmentDTO, department);
                await _repositoryManager.DepartmentRepository.UpdateAsync(department);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/departments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var department = await _repositoryManager.DepartmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    return NotFound();
                }

                await _repositoryManager.DepartmentRepository.DeleteAsync(department);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getdepartment/{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                var department = await _repositoryManager.DepartmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    return NotFound();
                }

                var mappedDepartment = _mapper.Map<ReadDepartmentDTO>(department);
                return Ok(mappedDepartment);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
