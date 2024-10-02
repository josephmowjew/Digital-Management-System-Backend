using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DataStore.Core.DTOs.SubcommitteeMembership;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLS_Digital_MGM.DataStore.Helpers;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SubcommitteeMembershipController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public SubcommitteeMembershipController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetSubcommitteeMembers(int pageNumber = 1, int pageSize = 10, int subcommitteeId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string userId = user.Id;

                var pagingParameters = new PagingParameters<SubcommitteeMembership>
                {
                    Predicate = sm => sm.SubcommitteeID == subcommitteeId && sm.Status != Lambda.Deleted && sm.MemberShipStatus == Lambda.Approved,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<SubcommitteeMembership, object>>[]{
                        sm => sm.MemberShip
                    }
                };

                var subcommitteeMembers = await _repositoryManager.SubcommitteeMembershipRepository.GetPagedAsync(pagingParameters);

                if (subcommitteeMembers == null || !subcommitteeMembers.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadSubcommitteeMembershipDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadSubcommitteeMembershipDTO>());
                }

                var mappedSubcommitteeMembers = _mapper.Map<List<ReadSubcommitteeMembershipDTO>>(subcommitteeMembers);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var totalRecords = await _repositoryManager.SubcommitteeMembershipRepository.CountAsync(pagingParameters);
            
                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = mappedSubcommitteeMembers
                    });
                }

                return Ok(mappedSubcommitteeMembers);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        //get subcommittee membership requests
        [HttpGet("requests")]
        public async Task<IActionResult> GetSubcommitteeMembershipRequests(int pageNumber = 1, int pageSize = 10, int subcommitteeId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string userId = user.Id;

                var pagingParameters = new PagingParameters<SubcommitteeMembership>
                {
                    Predicate = sm => sm.SubcommitteeID == subcommitteeId && sm.Status != Lambda.Deleted && sm.MemberShipStatus == Lambda.Pending,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<SubcommitteeMembership, object>>[]{
                        sm => sm.MemberShip
                    }
                };

                var subcommitteeMembers = await _repositoryManager.SubcommitteeMembershipRepository.GetPagedAsync(pagingParameters);

                if (subcommitteeMembers == null || !subcommitteeMembers.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadSubcommitteeMembershipDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadSubcommitteeMembershipDTO>());
                }

                var mappedSubcommitteeMembers = _mapper.Map<List<ReadSubcommitteeMembershipDTO>>(subcommitteeMembers);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var totalRecords = await _repositoryManager.SubcommitteeMembershipRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = mappedSubcommitteeMembers
                    });
                }

                return Ok(mappedSubcommitteeMembers);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddSubcommitteeMember([FromForm] CreateSubcommitteeMembershipDTO subcommitteeMemberDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var subcommitteeMember = _mapper.Map<SubcommitteeMembership>(subcommitteeMemberDTO);

                subcommitteeMember.JoinedDate = DateTime.Now;

                var existingMember = await _repositoryManager.SubcommitteeMembershipRepository.GetAsync(sm => sm.MemberShipId == subcommitteeMemberDTO.MembershipId && sm.SubcommitteeID == subcommitteeMember.SubcommitteeID);
                if (existingMember != null)
                {
                    ModelState.AddModelError(nameof(subcommitteeMemberDTO.MembershipId), "Member already exists in the subcommittee");
                    return BadRequest(ModelState);
                }else{
                    subcommitteeMember.MemberShipStatus = Lambda.Approved;
                }

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);

                if(!currentRole.Equals("member", StringComparison.OrdinalIgnoreCase)){
                    subcommitteeMember.MemberShipStatus = Lambda.Approved;
                }

                await _repositoryManager.SubcommitteeMembershipRepository.AddAsync(subcommitteeMember);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("join/{id}")]
        public async Task<IActionResult> Join(int id)
        {
            try
            {
                var subcommittee = await _repositoryManager.SubcommitteeRepository.GetByIdAsync(id);
                if (subcommittee == null)
                {
                    return NotFound();
                }
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                var member = await _repositoryManager.SubcommitteeMembershipRepository.GetAsync(sm => sm.MemberShipId == user.Id && sm.SubcommitteeID == subcommittee.Id && sm.MemberShipStatus != Lambda.Approved);
                if (member != null)
                {
                    if (member.MemberShipStatus != Lambda.Approved)
                    {
                        member.MemberShipStatus = Lambda.Pending;
                        await _repositoryManager.SubcommitteeMembershipRepository.UpdateAsync(member);
                        await _unitOfWork.CommitAsync();
                    }
                    else
                    {
                        return BadRequest("You are already a member of this subcommittee");
                    }
                }
                else
                {
                    member = new SubcommitteeMembership
                    {
                        SubcommitteeID = subcommittee.Id,
                        MemberShipId = user.Id,
                        MemberShipStatus = Lambda.Pending,
                        Role = "member",
                        JoinedDate = DateTime.Now
                    };

                    await _repositoryManager.SubcommitteeMembershipRepository.AddAsync(member);
                    await _unitOfWork.CommitAsync();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("approve/{id}")]
        public async Task<IActionResult> ApproveSubcommitteeMember(int id)
        {
            try
            {
                var subcommitteeMemberShip = await _repositoryManager.SubcommitteeMembershipRepository.GetByIdAsync(id);
                if (subcommitteeMemberShip == null)
                {
                    return BadRequest("You are not a member of this subcommittee");
                }
                else
                {
                    subcommitteeMemberShip.MemberShipStatus = Lambda.Approved;
                    subcommitteeMemberShip.JoinedDate = DateTime.Now;
                    await _repositoryManager.SubcommitteeMembershipRepository.UpdateAsync(subcommitteeMemberShip);
                    await _unitOfWork.CommitAsync();

                    var username = await _repositoryManager.UserRepository.GetSingleUser(subcommitteeMemberShip.MemberShipId);
                    BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(username.Email, "Subcommittee Membership Status", $"Your subcommittee membership for {subcommitteeMemberShip.Subcommittee.SubcommitteeName} has been approved", false));

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("reject/{id}")]
        public async Task<IActionResult> RejectSubcommitteeMember(int id)
        {
            try
            {
                var subcommittee = await _repositoryManager.SubcommitteeRepository.GetByIdAsync(id);
                if (subcommittee == null)
                {
                    return NotFound();
                }
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                var subcommitteeMembership = await _repositoryManager.SubcommitteeMembershipRepository.GetAsync(sm => sm.MemberShipId == user.Id && sm.SubcommitteeID == subcommittee.Id);
                if (subcommitteeMembership == null)
                {
                    return BadRequest("You are not a member of this subcommittee");
                }
                subcommitteeMembership.MemberShipStatus = Lambda.Revoked;
                subcommitteeMembership.Status = Lambda.Deleted;
                subcommitteeMembership.DeletedDate = DateTime.Now;
                await _repositoryManager.SubcommitteeMembershipRepository.UpdateAsync(subcommitteeMembership);
                await _unitOfWork.CommitAsync();

                BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(subcommitteeMembership.MemberShip.Email, "Subcommittee Membership Status", $"Your subcommittee membership for {subcommitteeMembership.Subcommittee.SubcommitteeName} has been rejected", false));
                
                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("exit/{id}")]
        public async Task<IActionResult> ExitSubcommitteeMember(int id)
        {
            try
            {
                var subcommittee = await _repositoryManager.SubcommitteeRepository.GetByIdAsync(id);
                if (subcommittee == null)
                {
                    return NotFound();
                }
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                var membership = await _repositoryManager.SubcommitteeMembershipRepository.GetAsync(sm => sm.MemberShipId == user.Id && sm.SubcommitteeID == subcommittee.Id);
                if (membership == null)
                {
                    return BadRequest("You are not a member of this subcommittee");
                }
                membership.MemberShipStatus = Lambda.Exited;
                await _repositoryManager.SubcommitteeMembershipRepository.UpdateAsync(membership);
                await _unitOfWork.CommitAsync();

                BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(membership.MemberShip.Email, "Subcommittee Membership Status", $"You have exited the subcommittee {membership.Subcommittee.SubcommitteeName}", false));
                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubcommitteeMember(int id, [FromBody] UpdateSubcommitteeMembershipDTO subcommitteeMemberDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var subcommitteeMember = await _repositoryManager.SubcommitteeMembershipRepository.GetByIdAsync(id);
                if (subcommitteeMember == null)
                {
                    return NotFound();
                }

                _mapper.Map(subcommitteeMemberDTO, subcommitteeMember);
                await _repositoryManager.SubcommitteeMembershipRepository.UpdateAsync(subcommitteeMember);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubcommitteeMember(int id)
        {
            try
            {
                var subcommitteeMember = await _repositoryManager.SubcommitteeMembershipRepository.GetByIdAsync(id);
                if (subcommitteeMember == null)
                {
                    return NotFound();
                }

                subcommitteeMember.MemberShipStatus = Lambda.Removed;
                await _repositoryManager.SubcommitteeMembershipRepository.UpdateAsync(subcommitteeMember);
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
