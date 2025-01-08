using Microsoft.AspNetCore.Mvc;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataStore.Core.DTOs.User;
using DataStore.Helpers;
using System.Linq.Expressions;
using MLS_Digital_MGM.DataStore.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using DataStore.Core.Services;
using DataStore.Core.DTOs.ApplicationUserChangeRequest;
using System.Security.Claims;
using Hangfire;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ApplicationUserChangeRequestController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ApplicationUserChangeRequestController(
            IRepositoryManager repositoryManager,
            IErrorLogService errorLogService,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetApplicationUserChangeRequests(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                var pagingParameters = new PagingParameters<ApplicationUserChangeRequest>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.Status != Lambda.Approved,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<ApplicationUserChangeRequest, object>>[] {
                        p => p.User
                    }
                };

                var requestsPaged = await _repositoryManager.ApplicationUserChangeRequestRepository.GetPagedAsync(pagingParameters);

                if (requestsPaged == null || !requestsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadApplicationUserChangeRequestDto>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadApplicationUserChangeRequestDto>());
                }

                var requestDTOs = _mapper.Map<List<ReadApplicationUserChangeRequestDto>>(requestsPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var totalRecords = await _repositoryManager.ApplicationUserChangeRequestRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = requestDTOs.ToList()
                    });
                }

                return Ok(requestDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var request = await _repositoryManager.ApplicationUserChangeRequestRepository.GetByIdAsync(id);
                if (request == null)
                    return NotFound();

                return Ok(_mapper.Map<ReadApplicationUserChangeRequestDto>(request));
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            try
            {
                var requests = await _repositoryManager.ApplicationUserChangeRequestRepository.GetByUserIdAsync(userId);
                if (requests == null || !requests.Any())
                {
                    return NotFound();
                }

                var requestDTOs = _mapper.Map<IEnumerable<ReadApplicationUserChangeRequestDto>>(requests);
                return Ok(requestDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{userId}/pending")]
        public async Task<IActionResult> GetPendingByUserId(string userId)
        {
            var request = await _repositoryManager.ApplicationUserChangeRequestRepository.GetPendingRequestByUserIdAsync(userId);
            if (request == null)
                return NotFound();

            return Ok(_mapper.Map<ReadApplicationUserChangeRequestDto>(request));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApplicationUserChangeRequestDto createDto)
        {
            try
            {
                // Check if user already has a pending request
                var pendingRequest = await _repositoryManager.ApplicationUserChangeRequestRepository.GetPendingRequestByUserIdAsync(createDto.UserId);
                if (pendingRequest != null)
                    return BadRequest("User already has a pending change request");

                var request = _mapper.Map<ApplicationUserChangeRequest>(createDto);
                request.CreatedById = request.CreatedById;

                await _repositoryManager.ApplicationUserChangeRequestRepository.AddAsync(request);
                await _unitOfWork.CommitAsync();
                return CreatedAtAction("GetById", new { id = request.Id }, request);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateApplicationUserChangeRequestDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest();

            var existingRequest = await _repositoryManager.ApplicationUserChangeRequestRepository.GetByIdAsync(id);
            if (existingRequest == null)
                return NotFound();

            _mapper.Map(updateDto, existingRequest);

            await _repositoryManager.ApplicationUserChangeRequestRepository.UpdateAsync(existingRequest);
            return Ok(_mapper.Map<ReadApplicationUserChangeRequestDto>(existingRequest));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _repositoryManager.ApplicationUserChangeRequestRepository.GetByIdAsync(id);
            if (request == null)
                return NotFound();

            await _repositoryManager.ApplicationUserChangeRequestRepository.DeleteAsync(request);
            return NoContent();
        }

        [HttpGet("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                // Fetch  firm using the UserRepository
                var changeRequest = await _repositoryManager.ApplicationUserChangeRequestRepository.GetByIdAsync(id);

                if (changeRequest != null)
                {
                    changeRequest.Status = Lambda.Approved;
                    string oldEmail = changeRequest.User.Email ?? string.Empty;

                    //check if the email from the change request is empty
                    if (!string.IsNullOrEmpty(changeRequest.Email))
                    {
                        //check if the email already exists in the system
                        var user = await _repositoryManager.UserRepository.FindByEmailAsync(changeRequest.Email);
                        if (user != null)
                        {
                            changeRequest.Status = Lambda.Rejected;
                            changeRequest.RejectionReason = "Email already exists in the system";
                            await _repositoryManager.ApplicationUserChangeRequestRepository.UpdateAsync(changeRequest);
                            await _unitOfWork.CommitAsync();
                            return BadRequest("Email already exists in the system");
                        }

                        changeRequest.User.Email = changeRequest.Email;
                        changeRequest.User.NormalizedEmail = changeRequest.Email.ToUpper();
                        changeRequest.User.UserName = changeRequest.Email;
                        changeRequest.User.NormalizedUserName = changeRequest.Email.ToUpper();
                    }
                    if (!string.IsNullOrEmpty(changeRequest.PhoneNumber))
                    {
                        changeRequest.User.PhoneNumber = changeRequest.PhoneNumber;
                    }

                    await _repositoryManager.ApplicationUserChangeRequestRepository.UpdateAsync(changeRequest);
                    await _repositoryManager.UserRepository.UpdateAsync(changeRequest.User);
                    await _unitOfWork.CommitAsync();

                    // Prepare email body
                    string emailBody;
                    if (!string.IsNullOrEmpty(changeRequest.Email))
                    {
                        emailBody = $"Your change request has been approved. To log into your account, please use the new email: {changeRequest.Email}";
                        // Send email to both old and new email addresses
                        BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string> { changeRequest.Email, oldEmail }, emailBody, "Change Request Status"));
                    }
                    else
                    {
                        emailBody = $"Your change request has been approved.";
                        // If no email change, just notify the old email
                        BackgroundJob.Enqueue(() => this._emailService.SendCPDStatusEmailsAsync(new List<string> { oldEmail }, emailBody, "Change Request Status"));
                    }

                    return Ok("Change request approved successfully");
                }
                else
                {
                    return BadRequest("There's already a pending change request for this user");
                }
            }
            catch (Exception ex)
            {

                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("denyChangeRequest/{id}")]
        public async Task<IActionResult> Deny(int id)
        {
            try
            {
                // Fetch  firm using the UserRepository
                var changeRequest = await _repositoryManager.ApplicationUserChangeRequestRepository.GetByIdAsync(id);

                if (changeRequest != null)
                {
                    changeRequest.Status = Lambda.Denied;

                    await _repositoryManager.ApplicationUserChangeRequestRepository.UpdateAsync(changeRequest);
                    await _unitOfWork.CommitAsync();


                    return Ok("Change request denied successfully");
                }
                else
                {
                    return BadRequest("There's already a pending change request for this user");
                }
            }
            catch (Exception ex)
            {

                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }
    }
}