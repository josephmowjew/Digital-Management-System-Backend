using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Persistence.Interfaces;
using DataStore.Core.DTOs.Communication;
using Microsoft.EntityFrameworkCore;
using DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Linq.Expressions;
using AutoMapper;
using MLS_Digital_Management_System_Front_End.Core.DTOs.Communication;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CommunicationsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CommunicationsController(
            IRepositoryManager repositoryManager,
            IErrorLogService errorLogService,
            IEmailService emailService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetCommunications(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string createdById = user.Id;

                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);
                
                var pagingParameters = new PagingParameters<CommunicationMessage>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<CommunicationMessage, object>>[] {
                        p => p.SentByUser
                    }
                };

                var communicationsPaged = await _repositoryManager.CommunicationMessageRepository.GetPagedAsync(pagingParameters);

                if (communicationsPaged == null || !communicationsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadCommunicationMessageDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadCommunicationMessageDTO>());
                }

                var communicationDTOs = _mapper.Map<List<ReadCommunicationMessageDTO>>(communicationsPaged);

                // Remove this foreach loop as it's no longer needed
                // The TargetedRoles and TargetedDepartments are already populated by AutoMapper
                /*
                foreach (var communication in communicationDTOs)
                {
                    communication.TargetedRoles = communication.GetTargetedRoles();
                    communication.TargetedDepartments = communication.GetTargetedDepartments();
                }
                */

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = communicationDTOs.Count;
                    var totalRecords = await _repositoryManager.CommunicationMessageRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = communicationDTOs.ToList()
                    });
                }

                return Ok(communicationDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO messageDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var recipients = await GetRecipients(messageDto);

                if (!recipients.Any())
                {
                    return BadRequest("No recipients found based on the provided criteria.");
                }

                var currentUser = await GetCurrentUser();
                if (currentUser == null)
                {
                    return Unauthorized();
                }

                var targetedDepartments = new List<string>();
                if (messageDto.DepartmentIds.Any())
                {
                    var departments = await _repositoryManager.DepartmentRepository.GetAllAsync(d => messageDto.DepartmentIds.Contains(d.Id));
                    targetedDepartments = departments.Select(d => d.Name).ToList();
                }

                var communicationMessage = new CommunicationMessage
                {
                    Subject = messageDto.Subject,
                    Body = messageDto.Body,
                    SentByUserId = currentUser.Id,
                    SentDate = DateTime.UtcNow,
                    Status = "Sent",
                    SentToAllUsers = messageDto.SendToAllUsers,
                    CreatedDate = DateTime.UtcNow
                };

                communicationMessage.SetTargetedRoles(messageDto.RoleNames);
                communicationMessage.SetTargetedDepartments(targetedDepartments);

                await _repositoryManager.CommunicationMessageRepository.AddAsync(communicationMessage);
                await _repositoryManager.UnitOfWork.CommitAsync();

                foreach (var recipient in recipients)
                {
                    await _emailService.SendMailWithKeyVarReturn(recipient.Email, messageDto.Subject, messageDto.Body);
                }

                return Json(new { message = $"Message sent successfully to {recipients.Count} recipients and saved with ID {communicationMessage.Id}." });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "An error occurred while sending the message.");
            }
        }

       private async Task<List<ApplicationUser>> GetRecipients(SendMessageDTO messageDto)
        {
            if (messageDto.SendToAllUsers)
            {
                return (await _repositoryManager.UserRepository.GetAllAsync()).Where(u => u.EmailConfirmed).ToList();
            }

            var recipients = new HashSet<ApplicationUser>();

            if (messageDto.DepartmentIds.Any())
            {
                var usersInDepartments = await _repositoryManager.UserRepository.GetAllAsync(u => messageDto.DepartmentIds.Contains(u.DepartmentId));
                recipients.UnionWith(usersInDepartments);
            }

            if (messageDto.RoleNames.Any())
            {
                foreach (var roleName in messageDto.RoleNames)
                {
                    var usersInRole = await _repositoryManager.UserManager.GetUsersInRoleAsync(roleName);
                    recipients.UnionWith(usersInRole);
                }
            }

            return recipients.Where(u => u.EmailConfirmed).ToList();
        }


        private async Task<ApplicationUser> GetCurrentUser()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            return await _repositoryManager.UserRepository.FindByEmailAsync(username);
        }
    }
}