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
using DataStore.Core.DTOs.SubcommitteeMessage;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SubcommitteeMessagesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubcommitteeMessagesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetSubcommitteeMessages(int pageNumber = 1, int pageSize = 10, int subcommitteeId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<SubcommitteeMessage>
                {   
                    Predicate = m => m.Status != Lambda.Deleted && (m.SubcommitteeID == 0 || m.SubcommitteeID == subcommitteeId),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<SubcommitteeMessage, object>>[] {
                        m => m.Subcommittee,
                        m => m.CreatedBy,
                        m => m.SubcommitteeThread
                    }
                };

                var subcommitteeMessagesPaged = await _repositoryManager.SubcommitteeMessageRepository.GetPagedAsync(pagingParameters);

                if (subcommitteeMessagesPaged == null || !subcommitteeMessagesPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadSubcommitteeMessageDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadSubcommitteeMessageDTO>());
                }

                var subcommitteeMessageDTOs = _mapper.Map<List<ReadSubcommitteeMessageDTO>>(subcommitteeMessagesPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = subcommitteeMessageDTOs.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltered,
                        recordsTotal = resultTotalFiltered,
                        data = subcommitteeMessageDTOs.ToList()
                    });
                }

                return Ok(subcommitteeMessageDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSubcommitteeMessage([FromForm] CreateSubcommitteeMessageDTO subcommitteeMessageDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var subcommitteeMessage = _mapper.Map<SubcommitteeMessage>(subcommitteeMessageDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                subcommitteeMessage.CreatedById = user.Id;
                subcommitteeMessage.Timestamp = DateTime.Now;

                await _repositoryManager.SubcommitteeMessageRepository.AddAsync(subcommitteeMessage);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetSubcommitteeMessageById", new { id = subcommitteeMessage.Id }, subcommitteeMessage);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetSubcommitteeMessageById/{id}")]
        public async Task<IActionResult> GetSubcommitteeMessageById(int id)
        {
            try
            {
                var subcommitteeMessage = await _repositoryManager.SubcommitteeMessageRepository.GetByIdAsync(id);
                if (subcommitteeMessage == null)
                {
                    return NotFound();
                }

                var mappedSubcommitteeMessage = _mapper.Map<ReadSubcommitteeMessageDTO>(subcommitteeMessage);
                return Ok(mappedSubcommitteeMessage);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubcommitteeMessage(int id, [FromForm] UpdateSubcommitteeMessageDTO subcommitteeMessageDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var subcommitteeMessage = await _repositoryManager.SubcommitteeMessageRepository.GetByIdAsync(id);
                if (subcommitteeMessage == null)
                    return NotFound();

                _mapper.Map(subcommitteeMessageDTO, subcommitteeMessage);
                await _repositoryManager.SubcommitteeMessageRepository.UpdateAsync(subcommitteeMessage);
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
        public async Task<IActionResult> DeleteSubcommitteeMessage(int id)
        {
            try
            {
                var subcommitteeMessage = await _repositoryManager.SubcommitteeMessageRepository.GetByIdAsync(id);
                if (subcommitteeMessage == null)
                    return NotFound();

                await _repositoryManager.SubcommitteeMessageRepository.DeleteAsync(subcommitteeMessage);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetSubcommitteeMessageByRange/{threadId}/{numberOfMessages}/{skip}")]
        public async Task<IActionResult> GetSubcommitteeMessageByRange(int threadId, int numberOfMessages, int skip)
        {
            try
            {
                var subcommitteeMessages = await _repositoryManager.SubcommitteeMessageRepository.GetAllAsync(
                    m => m.SubcommitteeThreadId == threadId,
                    m => m.OrderBy(y => y.Id),
                    "DESC",
                    numberOfMessages,
                    skip,
                    new Expression<Func<SubcommitteeMessage, object>>[] {
                        m => m.Subcommittee,
                        m => m.CreatedBy,
                        m => m.SubcommitteeThread
                    });

                if (subcommitteeMessages == null)
                {
                    return NotFound();
                }

                var subcommitteeMessageDTOs = _mapper.Map<List<ReadSubcommitteeMessageDTO>>(subcommitteeMessages);
                return Ok(subcommitteeMessageDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
