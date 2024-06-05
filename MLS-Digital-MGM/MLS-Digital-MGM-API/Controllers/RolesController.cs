using AutoMapper;
using DataStore.Core.DTOs.Role;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RolesController : Controller
    {
        // Dependency Injection
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;


        // Constructor
        public RolesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

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
                    //get the role with the name

                    var existingRole = await _repositoryManager.RoleRepository.GetRoleByNameAsync(role.Name);

                    //change the status of the role to active
                    existingRole.Status = Lambda.Active;
                    existingRole.DeletedDate = null;

                }else{

                    // Add the new Role to the repository and commit the changes
                    _repositoryManager.RoleRepository.AddRole(role.Name);

                }

                
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
        [HttpGet("paged")]
        public async Task<IActionResult> GetRoles(int pageNumber = 1, int pageSize = 10)
        {
            try
            {

                // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();
            
                var pagingParameters = new PagingParameters<Role>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

              
                // Get a paged list of Roles from the repository
                var pagedRoles = await _repositoryManager.RoleRepository.GetPagedAsync(pagingParameters);

                // Check if roles exist
                if (pagedRoles == null || !pagedRoles.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadRoleDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadRoleDTO>()); // Return empty list
                }

                // Map the Roles to a list of ReadRoleDTOs
                var mappedRoles = _mapper.Map<List<ReadRoleDTO>>(pagedRoles);

                 // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedRoles.Count;

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = resultTotalFiltred, 
                        recordsTotal = resultTotalFiltred, 
                        data = mappedRoles.ToList() // Materialize the enumerable
                    });
                }


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

        [HttpGet("singleRole/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            try
            {
                // Get the Role by id from the repository
                var role = await _repositoryManager.RoleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
                // Map the Role to a ReadRoleDTO
                var readRoleDTO = _mapper.Map<ReadRoleDTO>(role);

                // Return an Ok result with the mapped Role
                return Ok(readRoleDTO);
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