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
using DataStore.Core.DTOs.SubcommitteeThread;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SubcommitteeThreadsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubcommitteeThreadsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetSubcommitteeThreads(int pageNumber = 1, int pageSize = 10, int subcommitteeId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<SubcommitteeThread>
                {   
                    Predicate = t => t.Status != Lambda.Deleted && (t.SubcommitteeId == 0 || t.SubcommitteeId == subcommitteeId),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<SubcommitteeThread, object>>[] {
                        t => t.Subcommittee,
                        t => t.CreatedBy,
                        t => t.SubcommitteeMessages
                    }
                };

                var subcommitteeThreadsPaged = await _repositoryManager.SubcommitteeThreadRepository.GetPagedAsync(pagingParameters);

                if (subcommitteeThreadsPaged == null || !subcommitteeThreadsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadSubcommitteeThreadDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadSubcommitteeThreadDTO>());
                }

                var subcommitteeThreadDTOs = _mapper.Map<List<ReadSubcommitteeThreadDTO>>(subcommitteeThreadsPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = subcommitteeThreadDTOs.Count;
                    var totalRecords = await _repositoryManager.SubcommitteeThreadRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = subcommitteeThreadDTOs.ToList()
                    });
                }

                return Ok(subcommitteeThreadDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSubcommitteeThread([FromForm] CreateSubcommitteeThreadDTO subcommitteeThreadDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var subcommitteeThread = _mapper.Map<SubcommitteeThread>(subcommitteeThreadDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                subcommitteeThread.CreatedById = user.Id;

                await _repositoryManager.SubcommitteeThreadRepository.AddAsync(subcommitteeThread);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetSubcommitteeThreadById", new { id = subcommitteeThread.Id }, subcommitteeThread);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetSubcommitteeThreadById/{id}")]
        public async Task<IActionResult> GetSubcommitteeThreadById(int id)
        {
            try
            {
                var subcommitteeThread = await _repositoryManager.SubcommitteeThreadRepository.GetByIdAsync(id);
                if (subcommitteeThread == null)
                {
                    return NotFound();
                }

                var mappedSubcommitteeThread = _mapper.Map<ReadSubcommitteeThreadDTO>(subcommitteeThread);
                return Ok(mappedSubcommitteeThread);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubcommitteeThread(int id, [FromForm] UpdateSubcommitteeThreadDTO subcommitteeThreadDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var subcommitteeThread = await _repositoryManager.SubcommitteeThreadRepository.GetByIdAsync(id);
                if (subcommitteeThread == null)
                    return NotFound();

                _mapper.Map(subcommitteeThreadDTO, subcommitteeThread);
                await _repositoryManager.SubcommitteeThreadRepository.UpdateAsync(subcommitteeThread);
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
        public async Task<IActionResult> DeleteSubcommitteeThread(int id)
        {
            try
            {
                var subcommitteeThread = await _repositoryManager.SubcommitteeThreadRepository.GetByIdAsync(id);
                if (subcommitteeThread == null)
                    return NotFound();

                await _repositoryManager.SubcommitteeThreadRepository.DeleteAsync(subcommitteeThread);
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
