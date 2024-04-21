using AutoMapper;
using DataStore.Core.DTOs.LicenseApprovalLevelDTO;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class LicenseApprovalLevelsController : Controller
    {
        // Repository manager for working with the database
        private readonly IRepositoryManager _repositoryManager;

        // Service for logging errors
        private readonly IErrorLogService _errorLogService;

        // Unit of work for database operations
        private readonly IUnitOfWork _unitOfWork;

        // AutoMapper for mapping objects
        private readonly IMapper _mapper;

        // Constructor for dependency injection
        public LicenseApprovalLevelsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // HTTP GET endpoint to retrieve paged license approval levels
        [HttpGet("paged")]
        public async Task<IActionResult> GetLicenseApprovalLevels(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                     // Create PagingParameters object
                var pagingParameters = new PagingParameters<LicenseApprovalLevel>{
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    //SearchTerm = null

                };
                // Retrieve paged license approval levels from the repository
                var licenseApprovalLevels = await _repositoryManager.LicenseApprovalLevelRepository.GetPagedAsync(pagingParameters);

                // If no license approval levels were found, return a 404 Not Found response
                if (licenseApprovalLevels == null || !licenseApprovalLevels.Any())
                {
                    return NotFound();
                }

                // Map the license approval levels to the read DTO and return them
                var mappedLicenseApprovalLevels = _mapper.Map<IEnumerable<ReadLicenseApprovalLevelDTO>>(licenseApprovalLevels);
                return Ok(mappedLicenseApprovalLevels);
            }
            catch (Exception ex)
            {
                // Log and return any errors that occur
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // HTTP POST endpoint to add a new license approval level
        [HttpPost]
        public async Task<IActionResult> AddLicenseApprovalLevel([FromBody] CreateLicenseApprovalLevelDTO licenseApprovalLevelDTO)
        {
            try
            {
                // If the model state is not valid, return a 400 Bad Request response
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map the create DTO to a new license approval level and check if a license approval level with the same name and department already exists
                var licenseApprovalLevel = _mapper.Map<LicenseApprovalLevel>(licenseApprovalLevelDTO);
                var existingLicenseApprovalLevel = await _repositoryManager.LicenseApprovalLevelRepository.GetAsync(d => d.Level == licenseApprovalLevel.Level && d.DepartmentId == licenseApprovalLevel.DepartmentId);
                if (existingLicenseApprovalLevel != null)
                {
                    ModelState.AddModelError(nameof(licenseApprovalLevelDTO.Level), "A License Approval Level with the same name associated with the Department already exists");
                    return BadRequest(ModelState);
                }

                // Add the new license approval level to the repository and commit the changes
                await _repositoryManager.LicenseApprovalLevelRepository.AddAsync(licenseApprovalLevel);
                await _unitOfWork.CommitAsync();

                // Return a 201 Created response with the newly created license approval level
                return CreatedAtAction("GetLicenseApprovalLevels", new { id = licenseApprovalLevel.Id }, licenseApprovalLevel);
            }
            catch (Exception ex)
            {
                // Log and return any errors that occur
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // HTTP PUT endpoint to update an existing license approval level
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicenseApprovalLevel(int id, [FromBody] UpdateLicenseApprovalLevelDTO licenseApprovalLevelDTO)
        {
            try
            {
                // If the model state is not valid, return a 400 Bad Request response
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Retrieve the existing license approval level from the repository
                var licenseApprovalLevel = await _repositoryManager.LicenseApprovalLevelRepository.GetByIdAsync(id);

                // If the license approval level was not found, return a 404 Not Found response
                if (licenseApprovalLevel == null)
                {
                    return NotFound();
                }

                // Map the update DTO to the existing license approval level
                _mapper.Map(licenseApprovalLevelDTO, licenseApprovalLevel);

                // Update the license approval level in the repository and commit the changes
                await _repositoryManager.LicenseApprovalLevelRepository.UpdateAsync(licenseApprovalLevel);
                await _unitOfWork.CommitAsync();

                // Return a 204 No Content response
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log and return any errors that occur
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // HTTP DELETE endpoint to delete an existing license approval level
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicenseApprovalLevel(int id)
        {
            try
            {
                // Retrieve the existing license approval level from the repository
                var licenseApprovalLevel = await _repositoryManager.LicenseApprovalLevelRepository.GetByIdAsync(id);

                // If the license approval level was not found, return a 404 Not Found response
                if (licenseApprovalLevel == null)
                {
                    return NotFound();
                }

                // Delete the license approval level from the repository and commit the changes
                await _repositoryManager.LicenseApprovalLevelRepository.DeleteAsync(licenseApprovalLevel);
                await _unitOfWork.CommitAsync();

                // Return a 204 No Content response
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log and return any errors that occur
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}