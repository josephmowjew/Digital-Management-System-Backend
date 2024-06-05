using AutoMapper;
using DataStore.Core.DTOs.Country;
using DataStore.Core.DTOs.Title;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class TitlesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager; // Interface to the data layer
        private readonly IErrorLogService _errorLogService; // Interface to the error logging service
        private readonly IUnitOfWork _unitOfWork; // Interface to the unit of work pattern
        private readonly IMapper _mapper; // Interface to the object-to-object mapping service

        public TitlesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetTitles(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                  // Create PagingParameters object
                var pagingParameters = new PagingParameters<Title>{
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    //SearchTerm = null

                };
                var titles = await _repositoryManager.TitleRepository.GetPagedAsync(pagingParameters);

                if (titles == null || !titles.Any())
                {
                    return NotFound();
                }

                var mappedTitles = _mapper.Map<IEnumerable<ReadTitleDTO>>(titles);

                return Ok(mappedTitles);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllTitles()
        {
            try
            {
                var titles = await _repositoryManager.TitleRepository.GetAllAsync();

                if (titles == null || !titles.Any())
                {
                    return NotFound();
                }

                var mappedTitles = _mapper.Map<IEnumerable<ReadTitleDTO>>(titles);

                return Ok(mappedTitles);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTitle([FromBody] CreateTitleDTO titleDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var title = _mapper.Map<Title>(titleDTO);

                var existingTitle = await _repositoryManager.TitleRepository.GetAsync(c => c.Name.Trim().Equals(title.Name.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existingTitle != null)
                {
                    ModelState.AddModelError(nameof(titleDTO.Name), "A title with the same name already exists");
                    return BadRequest(ModelState);
                }

                await _repositoryManager.TitleRepository.AddAsync(title);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetTitles", new { id = title.Id }, title);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // Implement UpdateTitle and DeleteTitle methods similarly to UpdateCountry and DeleteCountry in the CountriesController
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTitle(int id, [FromBody] UpdateTitleDTO titleDTO)
        {
            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get the title from the data layer
                var title = await _repositoryManager.TitleRepository.GetByIdAsync(id);
                if (title == null)
                {
                    return NotFound();
                }

                // Map the DTO to the title
                _mapper.Map(titleDTO, title);

                // Update the title in the data layer
                await _repositoryManager.TitleRepository.UpdateAsync(title);
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
        public async Task<IActionResult> DeleteTitle(int id)
        {
            try
            {
                // Get the title from the data layer
                var title = await _repositoryManager.TitleRepository.GetByIdAsync(id);
                if (title == null)
                {
                    return NotFound();
                }

                // Delete the title from the data layer
                await _repositoryManager.TitleRepository.DeleteAsync(title);
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