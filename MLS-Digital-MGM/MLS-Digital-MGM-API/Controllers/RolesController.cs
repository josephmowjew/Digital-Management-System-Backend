using AutoMapper;
using DataStore.Core.DTOs.Role;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
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
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RolesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] CreateRoleDTO roleDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var role = _mapper.Map<Role>(roleDTO);

                var existingRoles = await _repositoryManager.RoleRepository.GetRolesAsync();
                if (existingRoles.Any(r => r.Name.Trim().Equals(role.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(roleDTO.Name), "A role with the same name already exists");
                    return BadRequest(ModelState);
                }

                 _repositoryManager.RoleRepository.AddRole(role.Name);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetRoles", new { id = role.Id }, role);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pagedRoles = await _repositoryManager.RoleRepository.GetPagedAsync(c => true, pageNumber, pageSize);
                var mappedRoles = pagedRoles.Select(r => _mapper.Map<ReadRoleDTO>(r));
                return Ok(mappedRoles);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var role = await _repositoryManager.RoleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }

                await _repositoryManager.RoleRepository.DeleteRoleAsync(role.Id);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditRole(string id, [FromBody] UpdateRoleDTO roleDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var role = await _repositoryManager.RoleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }

                _mapper.Map(roleDTO, role);
                await _repositoryManager.RoleRepository.UpdateRoleAsync(role);
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