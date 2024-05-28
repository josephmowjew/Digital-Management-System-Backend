using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataStore.Core.DTOs;
using DataStore.Core.Services;
using DataStore.Persistence.Interfaces;
using DataStore.Core.Services.Interfaces;
using DataStore.Core.Models;
using Microsoft.Extensions.Hosting;
using DataStore.Helpers;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MLS_Digital_MGM.DataStore.Helpers;
using DataStore.Core.DTOs.CPDUnitsEarned;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CPDUnitsEarnedController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CPDUnitsEarnedController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetCPDUnitsEarned(int memberId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                var pagingParameters = new PagingParameters<CPDUnitsEarned>
                {
                    Predicate = u => u.MemberId == memberId,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<CPDUnitsEarned, object>>[] {
                        p => p.CPDTraining,
                        p => p.YearOfOperation,
                        p => p.Member
                    }
                };

                var cpdUnitsEarnedPaged = await _repositoryManager.CPDUnitsEarnedRepository.GetPagedAsync(pagingParameters);

                if (cpdUnitsEarnedPaged == null || !cpdUnitsEarnedPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadCPDUnitsEarnedDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadCPDUnitsEarnedDTO>());
                }

                var cpdUnitEarnedDTOs = _mapper.Map<List<ReadCPDUnitsEarnedDTO>>(cpdUnitsEarnedPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = cpdUnitEarnedDTOs.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltered,
                        recordsTotal = resultTotalFiltered,
                        data = cpdUnitEarnedDTOs.ToList()
                    });
                }

                return Ok(cpdUnitEarnedDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCPDUnitEarned([FromForm] CreateCPDUnitsEarnedDTO cpdUnitEarnedDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cpdUnitEarned = _mapper.Map<CPDUnitsEarned>(cpdUnitEarnedDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                

                await _repositoryManager.CPDUnitsEarnedRepository.AddAsync(cpdUnitEarned);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetCPDUnitEarnedById", new { id = cpdUnitEarned.Id }, cpdUnitEarned);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetCPDUnitEarnedById/{id}")]
        public async Task<IActionResult> GetCPDUnitEarnedById(int id)
        {
            try
            {
                var cpdUnitEarned = await _repositoryManager.CPDUnitsEarnedRepository.GetByIdAsync(id);
                if (cpdUnitEarned == null)
                {
                    return NotFound();
                }

                var mappedCPDUnitEarned = _mapper.Map<ReadCPDUnitsEarnedDTO>(cpdUnitEarned);
                return Ok(mappedCPDUnitEarned);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCPDUnitEarned(int id, [FromForm] UpdateCPDUnitsEarnedDTO cpdUnitEarnedDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cpdUnitEarned = await _repositoryManager.CPDUnitsEarnedRepository.GetByIdAsync(id);
                if (cpdUnitEarned == null)
                    return NotFound();

                _mapper.Map(cpdUnitEarnedDTO, cpdUnitEarned);
                await _repositoryManager.CPDUnitsEarnedRepository.UpdateAsync(cpdUnitEarned);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCPDUnitEarned(int id)
        {
            try
            {
                var cpdUnitEarned = await _repositoryManager.CPDUnitsEarnedRepository.GetByIdAsync(id);
                if (cpdUnitEarned == null)
                    return NotFound();

                await _repositoryManager.CPDUnitsEarnedRepository.DeleteAsync(cpdUnitEarned);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetSummedCPDUnitsEarnedByMemberId/{memberId}")]
        public async Task<IActionResult> GetSummedCPDUnitsEarnedByMemberId(int memberId)
        {
            try
            {
                int cpdUnitEarned = 0;
                //get current operating year
                var currentYear = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();
                if (currentYear == null)
                {
                    return BadRequest("No current operating year found");
                }
                 cpdUnitEarned = await _repositoryManager.CPDUnitsEarnedRepository.GetSummedCPDUnitsEarnedByMemberId(memberId);
                if (cpdUnitEarned == null)
                {
                   cpdUnitEarned = 0;
                }

                return Ok(cpdUnitEarned);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
            
        }
    }
}
