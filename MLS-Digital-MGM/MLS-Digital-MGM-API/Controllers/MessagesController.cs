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
using DataStore.Core.DTOs.Message;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MessagesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MessagesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetMessages(int pageNumber = 1, int pageSize = 10, int threadId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Message>
                {   
                    Predicate = m => m.Status != Lambda.Deleted && (m.ThreadId == 0 || m.ThreadId == threadId),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Message, object>>[] {
                        m => m.Thread,
                        m => m.CreatedBy,
                        
                    }
                };

                var messagesPaged = await _repositoryManager.MessageRepository.GetPagedAsync(pagingParameters);

                if (messagesPaged == null || !messagesPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadMessageDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadMessageDTO>());
                }

                var messageDTOs = _mapper.Map<List<ReadMessageDTO>>(messagesPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = messageDTOs.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltered,
                        recordsTotal = resultTotalFiltered,
                        data = messageDTOs.ToList()
                    });
                }

                return Ok(messageDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromForm] CreateMessageDTO messageDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var message = _mapper.Map<Message>(messageDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                message.CreatedById = user.Id;
                message.Timestamp = DateTime.Now;

                await _repositoryManager.MessageRepository.AddAsync(message);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetMessageById", new { id = message.Id }, message);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetMessageById/{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            try
            {
                var message = await _repositoryManager.MessageRepository.GetByIdAsync(id);
                if (message == null)
                {
                    return NotFound();
                }

                var mappedMessage = _mapper.Map<ReadMessageDTO>(message);
                return Ok(mappedMessage);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromForm] UpdateMessageDTO messageDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var message = await _repositoryManager.MessageRepository.GetByIdAsync(id);
                if (message == null)
                    return NotFound();

                _mapper.Map(messageDTO, message);
                await _repositoryManager.MessageRepository.UpdateAsync(message);
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
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                var message = await _repositoryManager.MessageRepository.GetByIdAsync(id);
                if (message == null)
                    return NotFound();

                await _repositoryManager.MessageRepository.DeleteAsync(message);
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
