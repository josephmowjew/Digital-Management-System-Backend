using AutoMapper;
using DataStore.Core.DTOs.Role;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        // Dependency Injection
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // Constructor
        public RolesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // AddRole - Create a new Role
        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] CreateRoleDTO roleDTO)
        {
            try
            {
                // Check if model state is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map the DTO to a Role object
                var role = _mapper.Map<Role>(roleDTO);

                // Check if a Role with the same name already exists
                var existingRoles = await _repositoryManager.RoleRepository.GetRolesAsync();
                if (existingRoles.Any(r => r.Name.Trim().Equals(role.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(roleDTO.Name), "A role with the same name already exists");
                    return BadRequest(ModelState);
                }

                // Add the new Role to the repository and commit the changes
                _repositoryManager.RoleRepository.AddRole(role.Name);
                await _unitOfWork.CommitAsync();

                // Return a CreatedAtAction result
                return CreatedAtAction("GetRoles", new { id = role.Id }, role);
            }
            catch (Exception ex)
            {
                // Log the error and return a StatusCode(500) result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // GetRoles - Retrieve all Roles
        [HttpGet]
        public async Task<IActionResult> GetRoles(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                  // Create PagingParameters object
                var pagingParameters = new PagingParameters<Role>{
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    //SearchTerm = null

                };
                // Get a paged list of Roles from the repository
                var pagedRoles = await _repositoryManager.RoleRepository.GetPagedAsync(pagingParameters);

                // Map the Roles to a list of ReadRoleDTOs
                var mappedRoles = pagedRoles.Select(r => _mapper.Map<ReadRoleDTO>(r));

                // Return an Ok result with the mapped Roles
                return Ok(mappedRoles);
            }
            catch (Exception ex)
            {
                // Log the error and return a StatusCode(500) result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                
                // Get a paged list of Roles from the repository
                var pagedRoles = await _repositoryManager.RoleRepository.GetAllAsync();

                // Map the Roles to a list of ReadRoleDTOs
                var mappedRoles = pagedRoles.Select(r => _mapper.Map<ReadRoleDTO>(r));

                // Return an Ok result with the mapped Roles
                return Ok(mappedRoles);
            }
            catch (Exception ex)
            {
                // Log the error and return a StatusCode(500) result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // DeleteRole - Delete a Role by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                // Get the Role by id from the repository
                var role = await _repositoryManager.RoleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }

                // Delete the Role from the repository and commit the changes
                await _repositoryManager.RoleRepository.DeleteRoleAsync(role.Id);
                await _unitOfWork.CommitAsync();

                // Return a NoContent result
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the error and return a StatusCode(500) result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // EditRole - Update a Role by id
        [HttpPut("{id}")]
        public async Task<IActionResult> EditRole(string id, [FromBody] UpdateRoleDTO roleDTO)
        {
            try
            {
                // Check if model state is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get the Role by id from the repository
                var role = await _repositoryManager.RoleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }

                // Map the DTO to the Role object
                _mapper.Map(roleDTO, role);

                // Update the Role in the repository and commit the changes
                await _repositoryManager.RoleRepository.UpdateRoleAsync(role);
                await _unitOfWork.CommitAsync();

                // Return a NoContent result
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the error and return a StatusCode(500) result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}