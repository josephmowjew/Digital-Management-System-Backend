using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DataStore.Core.DTOs.CommitteeMemberShip;

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
    public class CommitteeMembersController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
         private readonly IEmailService _emailService; 

        public CommitteeMembersController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

       [HttpGet("paged")]
public async Task<IActionResult> GetCommitteeMembers(int pageNumber = 1, int pageSize = 10, int committeeId = 0)
{
    try
    {
        var dataTableParams = new DataTablesParameters(); // Create DataTables parameters instance

        string username = _httpContextAccessor.HttpContext.User.Identity.Name;
        var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
        string userId = user.Id;

        Expression<Func<CommitteeMembership, bool>> predicate;

        
       

        var pagingParameters = new PagingParameters<CommitteeMembership>
        {
            Predicate = cm => cm.CommitteeID == committeeId && cm.Status != Lambda.Deleted,
            PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
            PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
            SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
            SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
            // Includes should be specified in your PagingParameters
            Includes = new Expression<Func<CommitteeMembership, object>>[]{
                cm => cm.MemberShip
            }
        };

        var committeeMembers = await _repositoryManager.CommitteeMemberRepository.GetPagedAsync(pagingParameters);

        if (committeeMembers == null || !committeeMembers.Any())
        {
            if (dataTableParams.LoadFromRequest(_httpContextAccessor))
            {
                var draw = dataTableParams.Draw;
                return Json(new
                {
                    draw,
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = Enumerable.Empty<ReadCommitteeMemberShipDTO>()
                });
            }
            return Ok(Enumerable.Empty<ReadCommitteeMemberShipDTO>());
        }

        var mappedCommitteeMembers = _mapper.Map<List<ReadCommitteeMemberShipDTO>>(committeeMembers);

        if (dataTableParams.LoadFromRequest(_httpContextAccessor))
        {
            var draw = dataTableParams.Draw;
            var totalRecords = await _repositoryManager.CommitteeMemberRepository.CountAsync(pagingParameters);
            
            return Json(new
            {
                draw,
                recordsFiltered = totalRecords,
                recordsTotal = totalRecords,
                data = mappedCommitteeMembers
            });
        }

        return Ok(mappedCommitteeMembers);
    }
    catch (Exception ex)
    {
        await _errorLogService.LogErrorAsync(ex);
        return StatusCode(500, "Internal server error");
    }
}

        [HttpPost]
        public async Task<IActionResult> AddCommitteeMember([FromForm] CreateCommitteeMemberShipDTO committeeMemberDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var committeeMember = _mapper.Map<CommitteeMembership>(committeeMemberDTO);

                committeeMember.JoinedDate = DateTime.Now;

                //check if the member already exists in the committee
                var existingMember = await _repositoryManager.CommitteeMemberRepository.GetAsync(cm => cm.MemberShipId == committeeMemberDTO.MemberShipId && cm.CommitteeID == committeeMember.CommitteeID);
                if (existingMember != null)
                {
                    //add the error to model state 
                    ModelState.AddModelError(nameof(committeeMemberDTO.MemberShipId), "Member already exists in the committee");
                    return BadRequest(ModelState);
                }

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);

                if(!currentRole.Equals("member", StringComparison.OrdinalIgnoreCase)){

                    //autommatically approve the member if the current user is not a member
                    committeeMember.MemberShipStatus = "approved";
                }

                await _repositoryManager.CommitteeMemberRepository.AddAsync(committeeMember);
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
                var committee = await _repositoryManager.CommitteeRepository.GetByIdAsync(id);
                if (committee == null)
                {
                    return NotFound();
                }
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                var member = await _repositoryManager.CommitteeMemberRepository.GetAsync(cm => cm.MemberShipId == user.Id && cm.CommitteeID == committee.Id);
                if (member != null)
                {
                    return BadRequest("You are already a member of this committee");
                }
                member = new CommitteeMembership
                {
                    CommitteeID = committee.Id,
                    MemberShipId = user.Id,
                    MemberShipStatus = Lambda.Pending,
                    Role = "member",
                    JoinedDate = DateTime.Now
                };
                await _repositoryManager.CommitteeMemberRepository.AddAsync(member);
                await _unitOfWork.CommitAsync();
                return Ok();

                //return Ok("You have successfully joined the committee");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }


        }
        [HttpGet("approve/{id}")]
        public async Task<IActionResult> ApproveCommitteeMember(int id)
        {
            try
            {
                var committeeMemberShip = await _repositoryManager.CommitteeMemberRepository.GetAsync(c => c.Id  == id);
                if (committeeMemberShip == null)
                {
                    return BadRequest("You are not a member of this committee");
                }

                

                committeeMemberShip.MemberShipStatus = Lambda.Approved;
                committeeMemberShip.JoinedDate = DateTime.Now;
                await _repositoryManager.CommitteeMemberRepository.UpdateAsync(committeeMemberShip);
                
                await _unitOfWork.CommitAsync();

                //send email to the user that he has been approved
               
                
                BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(committeeMemberShip.MemberShip.Email, "Committe Membership Status", $"Your committee membership for {committeeMemberShip.Committee.CommitteeName} has been approved"));
                
                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("reject/{id}")]
        public async Task<IActionResult> RejectCommitteeMember(int id)
        {
            try
            {
                var committee = await _repositoryManager.CommitteeRepository.GetByIdAsync(id);
                if (committee == null)
                {
                    return NotFound();
                }
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                var committeeMembership = await _repositoryManager.CommitteeMemberRepository.GetAsync(cm => cm.MemberShipId == user.Id && cm.CommitteeID == committee.Id);
                if (committeeMembership == null)
                {
                    return BadRequest("You not a member of this committee");
                }
                committeeMembership.MemberShipStatus = Lambda.Revoked;
                committeeMembership.Status = Lambda.Deleted;
                committeeMembership.DeletedDate = DateTime.Now;
                await _repositoryManager.CommitteeMemberRepository.UpdateAsync(committeeMembership);
                await _unitOfWork.CommitAsync();

                 
                BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(committeeMembership.MemberShip.Email, "Committe Membership Status", $"Your committee membership for {committeeMembership.Committee.CommitteeName} has been rejected"));
                
                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("exit/{id}")]
        public async Task<IActionResult> ExitCommitteeMember(int id)
        {
            try
            {
                var committee = await _repositoryManager.CommitteeRepository.GetByIdAsync(id);
                if (committee == null)
                {
                    return NotFound();
                }
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                var membership = await _repositoryManager.CommitteeMemberRepository.GetAsync(cm => cm.MemberShipId == user.Id && cm.CommitteeID == committee.Id);
                if (membership == null)
                {
                    return BadRequest("You are already a member of this committee");
                }
                membership.MemberShipStatus = Lambda.Exited;
                await _repositoryManager.CommitteeMemberRepository.UpdateAsync(membership);
                await _unitOfWork.CommitAsync();

                //send an email to the user that he has exited the committee
                
                BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(membership.MemberShip.Email, "Committe Membership Status", $"You have exited the committee {membership.Committee.CommitteeName}"));
                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommitteeMember(int id, [FromBody] UpdateCommitteeMemberShipDTO committeeMemberDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var committeeMember = await _repositoryManager.CommitteeMemberRepository.GetByIdAsync(id);
                if (committeeMember == null)
                {
                    return NotFound();
                }

                _mapper.Map(committeeMemberDTO, committeeMember);
                await _repositoryManager.CommitteeMemberRepository.UpdateAsync(committeeMember);
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
        public async Task<IActionResult> DeleteCommitteeMember(int id)
        {
            try
            {
                var committeeMember = await _repositoryManager.CommitteeMemberRepository.GetByIdAsync(id);
                if (committeeMember == null)
                {
                    return NotFound();
                }

                await _repositoryManager.CommitteeMemberRepository.DeleteAsync(committeeMember);
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
