using AutoMapper;
using DataStore.Core.DTOs.Member;
using DataStore.Core.DTOs.Penalty;

//using DataStore.Core.DTOs.Penalty;
using DataStore.Core.DTOs.PenaltyType;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Linq.Expressions;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PenaltyTypeController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PenaltyTypeController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPenaltyTypes(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<PenaltyType>
                {
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    
                };

                var penaltyTypesPaged = await _repositoryManager.PenaltyTypeRepository.GetPagedAsync(pagingParameters);

                if (penaltyTypesPaged == null || !penaltyTypesPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadPenaltyTypeDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadPenaltyTypeDTO>());
                }

                var penaltyTypesDTOs = _mapper.Map<List<ReadPenaltyTypeDTO>>(penaltyTypesPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = penaltyTypesDTOs.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltered,
                        recordsTotal = resultTotalFiltered,
                        data = penaltyTypesDTOs.ToList()
                    });
                }

                return Ok(penaltyTypesDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPenaltyType([FromForm] CreatePenaltyTypeDTO penaltyTypeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var penaltyType = _mapper.Map<PenaltyType>(penaltyTypeDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                //penaltyType.CreatedById = user.Id;

                await _repositoryManager.PenaltyTypeRepository.AddAsync(penaltyType);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetPenaltyTypeId", new { id = penaltyType.Id }, penaltyType);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetPenaltyTypeById/{id}")]
        public async Task<IActionResult> GetPenaltyTypeById(int id)
        {
            try
            {
                var penaltyType = await _repositoryManager.PenaltyTypeRepository.GetByIdAsync(id);
                if (penaltyType == null)
                {
                    return NotFound();
                }

                var mappedPenaltyType = _mapper.Map<ReadPenaltyTypeDTO>(penaltyType);
                return Ok(mappedPenaltyType);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePenaltyType(int id, [FromForm] UpdatePenaltyTypeDTO penaltyTypeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var penaltyType = await _repositoryManager.PenaltyTypeRepository.GetByIdAsync(id);
                if (penaltyType == null)
                    return NotFound();

                _mapper.Map(penaltyTypeDTO, penaltyType);

                await _repositoryManager.PenaltyTypeRepository.UpdateAsync(penaltyType);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var penaltyTypeRecords = await this._repositoryManager.PenaltyTypeRepository.GetAllAsync();

                var readPenaltyTypeRecordsMapped = this._mapper.Map<List<ReadPenaltyTypeDTO>>(penaltyTypeRecords);

                return Ok(readPenaltyTypeRecordsMapped);
            }
            catch (Exception ex)
            {

                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePenaltyType(int id)
        {
            try
            {
                var penaltyType = await _repositoryManager.PenaltyTypeRepository.GetByIdAsync(id);
                if (penaltyType == null)
                    return NotFound();

                await _repositoryManager.PenaltyTypeRepository.DeleteAsync(penaltyType);
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
