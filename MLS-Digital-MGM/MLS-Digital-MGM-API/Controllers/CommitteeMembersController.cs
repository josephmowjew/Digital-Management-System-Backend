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

        public CommitteeMembersController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
