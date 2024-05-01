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
using DataStore.Core.DTOs.QualificationType;
using DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace MLS_Digital_MGM_API.Controllers 
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class QualificationTypesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public QualificationTypesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

       [HttpGet("paged")]
        public async Task<IActionResult> GetQualificationTypes(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                var pagingParameters = new PagingParameters<QualificationType>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                };

                var qualificationTypes = await _repositoryManager.QualificationTypeRepository.GetPagedAsync(pagingParameters);

                if (qualificationTypes == null || !qualificationTypes.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadQualificationTypeDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadQualificationTypeDTO>());
                }

                var mappedQualificationTypes = _mapper.Map<List<ReadQualificationTypeDTO>>(qualificationTypes);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedQualificationTypes.Count;

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = resultTotalFiltred, 
                        recordsTotal = resultTotalFiltred, 
                        data = mappedQualificationTypes.ToList() 
                    });
                }

                return Ok(mappedQualificationTypes);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

       [HttpGet("getAll")]
        public async Task<IActionResult> GetAllQualificationTypes()
        {
            try
            {
                var qualificationTypes = await _repositoryManager.QualificationTypeRepository.GetAllAsync(p => p.Status == Lambda.Active);

                if (qualificationTypes == null || !qualificationTypes.Any())
                {
                    return NotFound();
                }

                var mappedQualificationTypes = _mapper.Map<IEnumerable<ReadQualificationTypeDTO>>(qualificationTypes);

                return Ok(mappedQualificationTypes);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    [HttpPost]
    public async Task<IActionResult> AddQualificationType([FromBody] CreateQualificationTypeDTO qualificationTypeDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var qualificationType = _mapper.Map<QualificationType>(qualificationTypeDTO);

            var user = await _repositoryManager.UserRepository.FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);


            var existingQualificationType = await _repositoryManager.QualificationTypeRepository.GetAsync(q => q.Name.Trim().Equals(qualificationType.Name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existingQualificationType != null)
            {
                ModelState.AddModelError(nameof(qualificationTypeDTO.Name), "A qualification type with the same name already exists");
                return BadRequest(ModelState);
            }

            await _repositoryManager.QualificationTypeRepository.AddAsync(qualificationType);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction("GetQualificationTypes", new { id = qualificationType.Id }, qualificationType);
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

       [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQualificationType(int id, [FromBody] UpdateQualificationTypeDTO qualificationTypeDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var qualificationType = await _repositoryManager.QualificationTypeRepository.GetByIdAsync(id);
            if (qualificationType == null)
            {
                return NotFound();
            }

            _mapper.Map(qualificationTypeDTO, qualificationType);

            await _repositoryManager.QualificationTypeRepository.UpdateAsync(qualificationType);
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
        public async Task<IActionResult> DeleteQualificationType(int id)
        {
            try
            {
                var qualificationType = await _repositoryManager.QualificationTypeRepository.GetByIdAsync(id);
                if (qualificationType == null)
                {
                    return NotFound();
                }

                await _repositoryManager.QualificationTypeRepository.DeleteAsync(qualificationType);
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
        public async Task<IActionResult> GetQualificationType(int id)
        {
            try
            {
                var qualificationType = await _repositoryManager.QualificationTypeRepository.GetByIdAsync(id);
                if (qualificationType == null)
                {
                    return NotFound();
                }

                var mappedQualificationType = _mapper.Map<ReadQualificationTypeDTO>(qualificationType);
                return Ok(mappedQualificationType);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("custom_select")]
        public async Task<JsonResult> GetQualificationTypesForSelect(int page = 1, int pageSize = 20, string searchValue = "")
        {
            try
            {
                var pagingParameters = new PagingParameters<QualificationType>
                {
                    Predicate = u => u.Status == Lambda.Active,
                    PageNumber = page,
                    PageSize = pageSize,
                    SearchTerm = searchValue,
                };

                var qualificationTypes = await _repositoryManager.QualificationTypeRepository.GetPagedAsync(pagingParameters);

                List<DynamicSelect> dynamicSelect = new List<DynamicSelect>();

                if (qualificationTypes.Any())
                {
                    foreach (var item in qualificationTypes)
                    {
                        dynamicSelect.Add(new DynamicSelect { Id = item.Id.ToString(), Name = item.Name });
                    }
                }

                return Json(dynamicSelect);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return Json(new DynamicSelect());
            }
        }
    }
}