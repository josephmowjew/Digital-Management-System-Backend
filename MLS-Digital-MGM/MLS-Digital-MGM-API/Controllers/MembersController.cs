using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataStore.Core.DTOs; 
using DataStore.Core.Services;
using DataStore.Persistence.Interfaces;
using DataStore.Core.Services.Interfaces;
using DataStore.Core.Models;
using DataStore.Core.DTOs.Member;
using DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace MLS_Digital_MGM_API.Controllers 
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MembersController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public MembersController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

       [HttpGet("paged")]
        public async Task<IActionResult> GetMembers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                var pagingParameters = new PagingParameters<Member>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                };

                var members = await _repositoryManager.MemberRepository.GetPagedAsync(pagingParameters);

                if (members == null || !members.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadMemberDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadMemberDTO>());
                }

                var mappedMembers = _mapper.Map<List<ReadMemberDTO>>(members);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedMembers.Count;

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = resultTotalFiltred, 
                        recordsTotal = resultTotalFiltred, 
                        data = mappedMembers.ToList() 
                    });
                }

                return Ok(mappedMembers);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] CreateMemberDTO memberDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

              
                var member = _mapper.Map<Member>(memberDTO);

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);


                var existingMember = await _repositoryManager.MemberRepository.GetAsync(m => m.UserId == user.Id);
                if (existingMember != null)
                {
                    ModelState.AddModelError(nameof(memberDTO.UserId), $"A member already exists associated with the user @{user.Email}.");
                    return BadRequest(ModelState);
                }

                await _repositoryManager.MemberRepository.AddAsync(member);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetMember", new { id = member.Id }, member);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] UpdateMemberDTO memberDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var member = await _repositoryManager.MemberRepository.GetByIdAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                _mapper.Map(memberDTO, member);

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

                await _repositoryManager.MemberRepository.UpdateAsync(member);
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
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                var member = await _repositoryManager.MemberRepository.GetByIdAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                await _repositoryManager.MemberRepository.DeleteAsync(member);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            try
            {
                var member = await _repositoryManager.MemberRepository.GetByIdAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                var mappedMember = _mapper.Map<ReadMemberDTO>(member);
                return Ok(mappedMember);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getByUserId/{id}")]
        public async Task<IActionResult> GetMemberByUserId(string id)
        {
            try
            {
                var member = await _repositoryManager.MemberRepository.GetMemberByUserId(id);
                if (member == null)
                {
                    return NotFound();
                }

                var mappedMember = _mapper.Map<ReadMemberDTO>(member);
                return Ok(mappedMember);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

    
    }
}