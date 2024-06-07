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
using DataStore.Core.DTOs.Thread;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ThreadsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ThreadsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetThreads(int pageNumber = 1, int pageSize = 10, int committeeId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<DataStore.Core.Models.Thread>
                {   
                    Predicate = t => t.Status != Lambda.Deleted && (t.CommitteeId == 0 || t.CommitteeId == committeeId),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<DataStore.Core.Models.Thread, object>>[] {
                        t => t.Committee,
                        t => t.CreatedBy,
                        
                    }
                };

                var threadsPaged = await _repositoryManager.ThreadRepository.GetPagedAsync(pagingParameters);

                if (threadsPaged == null || !threadsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadThreadDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadThreadDTO>());
                }

                var threadDTOs = _mapper.Map<List<ReadThreadDTO>>(threadsPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = threadDTOs.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltered,
                        recordsTotal = resultTotalFiltered,
                        data = threadDTOs.ToList()
                    });
                }

                return Ok(threadDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddThread([FromForm] CreateThreadDTO threadDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var thread = _mapper.Map<DataStore.Core.Models.Thread>(threadDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                thread.CreatedById = user.Id;

                await _repositoryManager.ThreadRepository.AddAsync(thread);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetThreadById", new { id = thread.Id }, thread);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetThreadById/{id}")]
        public async Task<IActionResult> GetThreadById(int id)
        {
            try
            {
                var thread = await _repositoryManager.ThreadRepository.GetByIdAsync(id);
                if (thread == null)
                {
                    return NotFound();
                }

                var mappedThread = _mapper.Map<ReadThreadDTO>(thread);
                return Ok(mappedThread);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateThread(int id, [FromForm] UpdateThreadDTO threadDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var thread = await _repositoryManager.ThreadRepository.GetByIdAsync(id);
                if (thread == null)
                    return NotFound();

                _mapper.Map(threadDTO, thread);
                await _repositoryManager.ThreadRepository.UpdateAsync(thread);
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
        public async Task<IActionResult> DeleteThread(int id)
        {
            try
            {
                var thread = await _repositoryManager.ThreadRepository.GetByIdAsync(id);
                if (thread == null)
                    return NotFound();

                await _repositoryManager.ThreadRepository.DeleteAsync(thread);
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
