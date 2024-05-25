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
using DataStore.Core.DTOs.LicenseApprovalHistory;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LicenseApprovalHistoriesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LicenseApprovalHistoriesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetLicenseApprovalHistories(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);

                var pagingParameters = new PagingParameters<LicenseApprovalHistory>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<LicenseApprovalHistory, object>>[] {
                        p => p.LicenseApplication,
                        p => p.ApprovalLevel,
                        p => p.ChangedBy
                    },
                    CreatedById = string.Equals(currentRole, "member", StringComparison.OrdinalIgnoreCase) ? CreatedById : null,
                };

                var licenseApprovalHistoriesPaged = await _repositoryManager.LicenseApprovalHistoryRepository.GetPagedAsync(pagingParameters);

                if (licenseApprovalHistoriesPaged == null || !licenseApprovalHistoriesPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadLicenseApprovalHistoryDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadLicenseApprovalHistoryDTO>());
                }

                var licenseApprovalHistoryDTOs = _mapper.Map<List<ReadLicenseApprovalHistoryDTO>>(licenseApprovalHistoriesPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = licenseApprovalHistoryDTOs.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltered,
                        recordsTotal = resultTotalFiltered,
                        data = licenseApprovalHistoryDTOs.ToList()
                    });
                }

                return Ok(licenseApprovalHistoryDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddLicenseApprovalHistory([FromForm] CreateLicenseApprovalHistoryDTO licenseApprovalHistoryDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var licenseApprovalHistory = _mapper.Map<LicenseApprovalHistory>(licenseApprovalHistoryDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                licenseApprovalHistory.ChangedById = user.Id;

                await _repositoryManager.LicenseApprovalHistoryRepository.AddAsync(licenseApprovalHistory);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetLicenseApprovalHistoryById", new { id = licenseApprovalHistory.Id }, licenseApprovalHistory);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetLicenseApprovalHistoryById/{id}")]
        public async Task<IActionResult> GetLicenseApprovalHistoryById(int id)
        {
            try
            {
                var licenseApprovalHistory = await _repositoryManager.LicenseApprovalHistoryRepository.GetByIdAsync(id);
                if (licenseApprovalHistory == null)
                {
                    return NotFound();
                }

                var mappedLicenseApprovalHistory = _mapper.Map<ReadLicenseApprovalHistoryDTO>(licenseApprovalHistory);
                return Ok(mappedLicenseApprovalHistory);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicenseApprovalHistory(int id, [FromForm] UpdateLicenseApprovalHistoryDTO licenseApprovalHistoryDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var licenseApprovalHistory = await _repositoryManager.LicenseApprovalHistoryRepository.GetByIdAsync(id);
                if (licenseApprovalHistory == null)
                    return NotFound();

                _mapper.Map(licenseApprovalHistoryDTO, licenseApprovalHistory);
                await _repositoryManager.LicenseApprovalHistoryRepository.UpdateAsync(licenseApprovalHistory);
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
        public async Task<IActionResult> DeleteLicenseApprovalHistory(int id)
        {
            try
            {
                var licenseApprovalHistory = await _repositoryManager.LicenseApprovalHistoryRepository.GetByIdAsync(id);
                if (licenseApprovalHistory == null)
                    return NotFound();

                await _repositoryManager.LicenseApprovalHistoryRepository.DeleteAsync(licenseApprovalHistory);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetLicenseApprovalHistoryByLicenseApplicationId/{id}")]
        public async Task<IActionResult> GetLicenseApprovalHistoryByLicenseApplicationId(int id)
        {
            try
            {
                var licenseApprovalHistories =  _repositoryManager.LicenseApprovalHistoryRepository.FindByConditionAsync(x => x.LicenseApplicationId == id);
                if (licenseApprovalHistories == null)
                {
                    return NotFound();
                }

                var mappedLicenseApprovalHistories = _mapper.Map<List<ReadLicenseApprovalHistoryDTO>>(licenseApprovalHistories);
                return Ok(mappedLicenseApprovalHistories);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
