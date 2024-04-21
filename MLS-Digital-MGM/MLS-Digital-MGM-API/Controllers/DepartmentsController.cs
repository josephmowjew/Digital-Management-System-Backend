using AutoMapper;
using DataStore.Core.DTOs.Department;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class DepartmentsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepartmentsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Add a method to fetch paginated departments
        [HttpGet("paged")]
        public async Task<IActionResult> GetDepartments(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                      // Create PagingParameters object
                var pagingParameters = new PagingParameters<Department>{
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    //SearchTerm = null

                };
                // Fetch paginated departments using the DepartmentRepository
                var departments = await _repositoryManager.DepartmentRepository.GetPagedAsync(pagingParameters);

                // Check if departments exist
                if (departments == null || !departments.Any())
                {
                    return NotFound(); // Return 404 Not Found if no departments are found
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
    }
}
