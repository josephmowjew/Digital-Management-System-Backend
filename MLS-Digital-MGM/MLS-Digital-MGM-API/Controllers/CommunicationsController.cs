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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommunicationMessage(int id)
        {
            try
            {
                var communicationMessage = await _repositoryManager.CommunicationMessageRepository.GetByIdWithRecipientsAsync(id);

                if (communicationMessage == null)
                {
                    return NotFound();
                }

                var communicationDTO = _mapper.Map<ReadCommunicationMessageDTO>(communicationMessage);

                return Ok(communicationDTO);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageDTO messageDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var recipients = await GetRecipientsAsync(messageDto);

                if (!recipients.Any())
                {
                    return BadRequest("No recipients found based on the provided criteria.");
                }

                var currentUser = await GetCurrentUser();
                if (currentUser == null)
                {
                    return Unauthorized();
                }

                var targetedDepartments = await GetTargetedDepartmentsAsync(messageDto.DepartmentIds);

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "CommunicationMessage")
                                    ?? new AttachmentType { Name = "CommunicationMessage" };

                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _repositoryManager.UnitOfWork.CommitAsync();
                }

                var attachments = new List<Attachment>();
                if (messageDto.Attachments?.Any() == true)
                {
                    attachments = await SaveAttachmentsAsync(messageDto.Attachments, attachmentType.Id);
                }

                var communicationMessage = new CommunicationMessage
                {
                    Subject = messageDto.Subject,
                    Body = messageDto.Body,
                    SentByUserId = currentUser.Id,
                    SentDate = DateTime.Now,
                    Status = "Sent",
                    SentToAllUsers = messageDto.SendToAllUsers,
                    CreatedDate = DateTime.Now,
                    Attachments = attachments // Handle case when attachments are null
                };

                communicationMessage.SetTargetedRoles(messageDto.RoleNames);
                communicationMessage.SetTargetedDepartments(targetedDepartments);

                await _repositoryManager.CommunicationMessageRepository.AddAsync(communicationMessage);
                await _repositoryManager.UnitOfWork.CommitAsync();

                var emailTasks = recipients.Select(async recipient =>
                {
                    using var scope = HttpContext.RequestServices.CreateScope();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    return await emailService.SendMailFromCommunicationMessage(
                        recipient.Email,
                        communicationMessage,
                        currentUser.Id,
                        messageDto.IncludeSignature
                    );
                });
                var results = await Task.WhenAll(emailTasks);

                return Json(new { message = $"Message sent successfully to {recipients.Count} recipients and saved with ID {communicationMessage.Id}." });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "An error occurred while sending the message.");
            }
        }

        private async Task<List<ApplicationUser>> GetRecipientsAsync(SendMessageDTO messageDto)
        {
            var recipients = new List<ApplicationUser>();

            if (messageDto.SendToAllUsers)
            {
                recipients = (await _repositoryManager.UserRepository.GetAllAsync(u => u.EmailConfirmed)).ToList();
            }
            else
            {
                var tasks = new List<Task<IEnumerable<ApplicationUser>>>();

                if (messageDto.DepartmentIds.Any())
                {
                    tasks.Add(_repositoryManager.UserRepository.GetAllAsync(u => messageDto.DepartmentIds.Contains(u.DepartmentId)));
                }

                if (messageDto.RoleNames.Any())
                {
                    var roleTasks = messageDto.RoleNames.Select(async roleName => (await _repositoryManager.UserManager.GetUsersInRoleAsync(roleName)).AsEnumerable());
                    tasks.AddRange(roleTasks);
                }

                var results = await Task.WhenAll(tasks);

                foreach (var result in results)
                {
                    recipients.AddRange(result.Where(u => u.EmailConfirmed));
                }
            }

            return recipients;
        }


        private async Task<List<string>> GetTargetedDepartmentsAsync(IEnumerable<int> departmentIds)
        {
            if (!departmentIds.Any())
            {
                return new List<string>();
            }

            var departments = await _repositoryManager.DepartmentRepository.GetAllAsync(d => departmentIds.Contains(d.Id));
            return departments.Select(d => d.Name).ToList();
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            return await _repositoryManager.UserRepository.FindByEmailAsync(username);
        }

        private async Task<List<Attachment>> SaveAttachmentsAsync(IEnumerable<IFormFile> attachments, int attachmentTypeId)
        {
            var attachmentsList = new List<Attachment>();
            var hostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var webRootPath = hostEnvironment.WebRootPath;


            // Check if webRootPath is null or empty
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null or empty");
            }

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/CommunicationMessageAttachments");



            // Ensure the directory exists
            if (!Directory.Exists(AttachmentsPath))
            {
                Directory.CreateDirectory(AttachmentsPath);

            }

            foreach (var attachment in attachments)
            {
                if (attachment == null || string.IsNullOrWhiteSpace(attachment.FileName))
                {

                    continue;
                }

                var uniqueFileName = FileNameGenerator.GenerateUniqueFileName(attachment.FileName);
                var filePath = Path.Combine(AttachmentsPath, uniqueFileName);



                try
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await attachment.CopyToAsync(stream);
                    }

                    attachmentsList.Add(new Attachment
                    {
                        FileName = uniqueFileName,
                        FilePath = filePath,
                        AttachmentTypeId = attachmentTypeId,
                        PropertyName = attachment.Name
                    });
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            return attachmentsList;
        }
    }}