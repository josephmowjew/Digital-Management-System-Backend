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
using DataStore.Core.DTOs.Subcommittee;
using DataStore.Core.DTOs.SubcommitteeMembership;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SubcommitteesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubcommitteesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetSubcommittees(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<Subcommittee>
                {   
                    Predicate = u => u.Status != Lambda.Deleted, 
                    //&& u.ParentCommittee.Id == parentCommitteeId 
                    //&& u.ParentCommittee.CommitteeMemberships.Any(cm => cm.MemberShipId == currentUserId),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Subcommittee, object>>[] {
                        p => p.Chairperson,
                        p => p.Chairperson.User,
                        p => p.Threads,
                        p => p.ParentCommittee,
                        p => p.SubcommitteeMemberships
                    }
                };

                var subcommitteesPaged = await _repositoryManager.SubcommitteeRepository.GetPagedAsync(pagingParameters);

                if (subcommitteesPaged == null || !subcommitteesPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadSubcommitteeDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadSubcommitteeDTO>());
                }

                var subcommitteeDTOs = _mapper.Map<List<ReadSubcommitteeDTO>>(subcommitteesPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = subcommitteeDTOs.Count;
                    var totalRecords = await _repositoryManager.SubcommitteeRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = subcommitteeDTOs.ToList()
                    });
                }

                return Ok(subcommitteeDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSubcommittee([FromForm] CreateSubcommitteeDTO subcommitteeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var subcommittee = _mapper.Map<Subcommittee>(subcommitteeDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                subcommittee.CreatedById = user.Id;

                await _repositoryManager.SubcommitteeRepository.AddAsync(subcommittee);
                await _unitOfWork.CommitAsync();

                if (subcommittee.ChairpersonId.HasValue)
                {
                    var chairperson = await _repositoryManager.MemberRepository.GetByIdAsync(subcommittee.ChairpersonId.Value);
                    if (chairperson != null)
                    {
                        var chairpersonMembershipDTO = new CreateSubcommitteeMembershipDTO
                        {
                            SubcommitteeID = subcommittee.Id,
                            MembershipId = chairperson.UserId,
                            JoinedDate = DateTime.Now,
                            Role = "Chairperson"
                        };

                        var subcommitteeMember = _mapper.Map<SubcommitteeMembership>(chairpersonMembershipDTO);

                        subcommitteeMember.JoinedDate = DateTime.Now;

                        var existingMember = await _repositoryManager.SubcommitteeMembershipRepository.GetAsync(sm => sm.MemberShipId == chairpersonMembershipDTO.MembershipId && sm.SubcommitteeID == subcommittee.Id);
                        if (existingMember != null)
                        {
                            ModelState.AddModelError(nameof(chairpersonMembershipDTO.MembershipId), "Member already exists in the subcommittee");
                            return BadRequest(ModelState);
                        }

                        string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);

                        if (!currentRole.Equals("member", StringComparison.OrdinalIgnoreCase))
                        {
                            subcommitteeMember.MemberShipStatus = Lambda.Approved;
                        }

                        await _repositoryManager.SubcommitteeMembershipRepository.AddAsync(subcommitteeMember);
                        await _unitOfWork.CommitAsync();
                    }
                }

                return CreatedAtAction("GetSubcommitteeById", new { id = subcommittee.Id }, subcommittee);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetSubcommitteeById/{id}")]
        public async Task<IActionResult> GetSubcommitteeById(int id)
        {
            try
            {
                var subcommittee = await _repositoryManager.SubcommitteeRepository.GetByIdAsync(id);
                if (subcommittee == null)
                {
                    return NotFound();
                }

                var mappedSubcommittee = _mapper.Map<ReadSubcommitteeDTO>(subcommittee);
                return Ok(mappedSubcommittee);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubcommittee(int id, [FromForm] UpdateSubcommitteeDTO subcommitteeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var subcommittee = await _repositoryManager.SubcommitteeRepository.GetByIdAsync(id);
                if (subcommittee == null)
                    return NotFound();

                _mapper.Map(subcommitteeDTO, subcommittee);
                await _repositoryManager.SubcommitteeRepository.UpdateAsync(subcommittee);
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
        public async Task<IActionResult> DeleteSubcommittee(int id)
        {
            try
            {
                var subcommittee = await _repositoryManager.SubcommitteeRepository.GetByIdAsync(id);
                if (subcommittee == null)
                    return NotFound();

                await _repositoryManager.SubcommitteeRepository.DeleteAsync(subcommittee);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            try
            {
                var count = await _repositoryManager.SubcommitteeRepository.GetSubcommitteeCount();

                return Ok(count);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
