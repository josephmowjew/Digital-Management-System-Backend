using AutoMapper;
using DataStore.Core.DTOs.IdentityType;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLS_Digital_MGM_API.Controllers
{
    // Controller for handling requests related to identity types
    [Route("api/[controller]")]
    public class IdentityTypesController : Controller
    {
        // Dependency Injection of required services
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // Constructor for dependency injection
        public IdentityTypesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Action for retrieving a paged list of identity types
        [HttpGet("paged")]
        public async Task<IActionResult> GetIdentityTypes(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Get paged list of identity types from repository
                var identityTypes = await _repositoryManager.IdentityTypeRepository.GetPagedAsync(d => true, pageNumber, pageSize);

                // If no identity types found, return NotFound result
                if (identityTypes == null || !identityTypes.Any())
                {
                    return NotFound();
                }

                // Map identity types to DTOs and return as Ok result
                var mappedIdentityTypes = _mapper.Map<IEnumerable<ReadIdentityTypeDTO>>(identityTypes);

                return Ok(mappedIdentityTypes);
            }
            catch (Exception ex)
            {
                // Log error and return Internal Server Error result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Action for adding a new identity type
        [HttpPost]
        public async Task<IActionResult> AddIdentityType([FromBody] CreateIdentityTypeDTO identityTypeDTO)
        {
            try
            {
                // If model state is invalid, return BadRequest result with invalid fields
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map DTO to new identity type and add to repository
                var identityType = _mapper.Map<IdentityType>(identityTypeDTO);
                await _repositoryManager.IdentityTypeRepository.AddAsync(identityType);
                await _unitOfWork.CommitAsync();

                // Return Created result with location of new identity type
                return CreatedAtAction("GetIdentityTypes", new { id = identityType.Id }, identityType);
            }
            catch (Exception ex)
            {
                // Log error and return Internal Server Error result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Action for updating an existing identity type
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIdentityType(int id, [FromBody] UpdateIdentityTypeDTO identityTypeDTO)
        {
            try
            {
                // If model state is invalid, return BadRequest result with invalid fields
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get identity type by id from repository
                var identityType = await _repositoryManager.IdentityTypeRepository.GetByIdAsync(id);
                if (identityType == null)
                {
                    return NotFound();
                }

                // Map DTO to existing identity type and update repository
                _mapper.Map(identityTypeDTO, identityType);
                await _repositoryManager.IdentityTypeRepository.UpdateAsync(identityType);
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

        // Action for deleting an existing identity type
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdentityType(int id)
        {
            try
            {
                // Get identity type by id from repository
                var identityType = await _repositoryManager.IdentityTypeRepository.GetByIdAsync(id);
                if (identityType == null)
                {
                    return NotFound();
                }

                // Delete identity type from repository
                await _repositoryManager.IdentityTypeRepository.DeleteAsync(identityType);
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