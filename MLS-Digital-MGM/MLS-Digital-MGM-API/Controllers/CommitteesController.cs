using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataStore.Core.DTOs;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Persistence.Interfaces;
using MLS_Digital_MGM.DataStore.Helpers;
using DataStore.Helpers;
using DataStore.Core.DTOs.Committee;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CommitteesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommitteesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetCommittees(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Committee>
                {   Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Committee, object>>[] {
                        p => p.Chairperson,
                        p => p.Chairperson.User,
                        p => p.Threads,
                        p => p.YearOfOperation,
                        p => p.CommitteeMemberships
                    }
                };

                var committeesPaged = await _repositoryManager.CommitteeRepository.GetPagedAsync(pagingParameters);

                if (committeesPaged == null || !committeesPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadCommitteeDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadCommitteeDTO>());
                }

                var committeeDTOs = _mapper.Map<List<ReadCommitteeDTO>>(committeesPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = committeeDTOs.Count;
                    var totalRecords = await _repositoryManager.CommitteeRepository.CountAsync(pagingParameters);


                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = committeeDTOs.ToList()
                    });
                }

                return Ok(committeeDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCommittee([FromForm] CreateCommitteeDTO committeeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var committee = _mapper.Map<Committee>(committeeDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                committee.CreatedById = user.Id;
                

                await _repositoryManager.CommitteeRepository.AddAsync(committee);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetCommitteeById", new { id = committee.Id }, committee);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetCommitteeById/{id}")]
        public async Task<IActionResult> GetCommitteeById(int id)
        {
            try
            {
                var committee = await _repositoryManager.CommitteeRepository.GetByIdAsync(id);
                if (committee == null)
                {
                    return NotFound();
                }

                var mappedCommittee = _mapper.Map<ReadCommitteeDTO>(committee);
                return Ok(mappedCommittee);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommittee(int id, [FromForm] UpdateCommitteeDTO committeeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var committee = await _repositoryManager.CommitteeRepository.GetByIdAsync(id);
                if (committee == null)
                    return NotFound();

                _mapper.Map(committeeDTO, committee);
                await _repositoryManager.CommitteeRepository.UpdateAsync(committee);
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
        public async Task<IActionResult> DeleteCommittee(int id)
        {
            try
            {
                var committee = await _repositoryManager.CommitteeRepository.GetByIdAsync(id);
                if (committee == null)
                    return NotFound();

                await _repositoryManager.CommitteeRepository.DeleteAsync(committee);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
