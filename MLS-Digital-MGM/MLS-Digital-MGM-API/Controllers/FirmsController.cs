using AutoMapper;
using DataStore.Core.DTOs.Firms;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
public class FirmsController : Controller
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IErrorLogService _errorLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FirmsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _errorLogService = errorLogService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetFirms(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
                    // Create PagingParameters object
                var pagingParameters = new PagingParameters<Firm>{
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    //SearchTerm = null

                };
            var firms = await _repositoryManager.FirmRepository.GetPagedAsync(pagingParameters);

            if (firms == null || !firms.Any())
            {
                return NotFound();
            }

            var mappedFirms = _mapper.Map<IEnumerable<ReadFirmDTO>>(firms);

            return Ok(mappedFirms);

        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddFirm([FromBody] CreateFirmDTO firmDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firm = _mapper.Map<Firm>(firmDTO);

            var existingFirm = await _repositoryManager.FirmRepository.GetAsync(f => f.Name.Trim().Equals(firm.Name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existingFirm != null)
            {
                ModelState.AddModelError(nameof(firmDTO.Name), "A firm with the same name already exists");
                return BadRequest(ModelState);
            }

            await _repositoryManager.FirmRepository.AddAsync(firm);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction("GetFirms", new { id = firm.Id }, firm);
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFirm(int id, [FromBody] UpdateFirmDTO firmDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firm = await _repositoryManager.FirmRepository.GetByIdAsync(id);
            if (firm == null)
            {
                return NotFound();
            }

            _mapper.Map(firmDTO, firm);
            await _repositoryManager.FirmRepository.UpdateAsync(firm);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFirm(int id)
    {
        try
        {
            var firm = await _repositoryManager.FirmRepository.GetByIdAsync(id);
            if (firm == null)
            {
                return NotFound();
            }

            await _repositoryManager.FirmRepository.DeleteAsync(firm);
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