using AutoMapper;
using DataStore.Core.DTOs.LevyDeclaration;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using MLS_Digital_MGM.DataStore.Helpers;


namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LevyDeclarationController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LevyDeclarationController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetLevyDeclarations(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                 var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<LevyDeclaration>
                {
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                };

                var levyDeclarationsPaged = await _repositoryManager.LevyDeclarationRepository.GetPagedAsync(pagingParameters);

                if (levyDeclarationsPaged == null || !levyDeclarationsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadLevyDeclarationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadLevyDeclarationDTO>());
                }

                var levyDeclarationsDTOs = _mapper.Map<List<ReadLevyDeclarationDTO>>(levyDeclarationsPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = levyDeclarationsDTOs.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltered,
                        recordsTotal = resultTotalFiltered,
                        data = levyDeclarationsDTOs.ToList()
                    });
                }

                return Ok(levyDeclarationsDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddLevyDeclaration([FromBody] CreateLevyDeclarationDTO levyDeclarationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var levyDeclaration = _mapper.Map<LevyDeclaration>(levyDeclarationDTO);

                await _repositoryManager.LevyDeclarationRepository.AddAsync(levyDeclaration);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetLevyDeclarationById", new { id = levyDeclaration.Id }, levyDeclaration);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetLevyDeclarationById/{id}")]
        public async Task<IActionResult> GetLevyDeclarationById(int id)
        {
            try
            {
                var levyDeclaration = await _repositoryManager.LevyDeclarationRepository.GetByIdAsync(id);
                if (levyDeclaration == null)
                {
                    return NotFound();
                }

                var mappedLevyDeclaration = _mapper.Map<ReadLevyDeclarationDTO>(levyDeclaration);
                return Ok(mappedLevyDeclaration);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLevyDeclaration(int id, [FromBody] UpdateLevyDeclarationDTO levyDeclarationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var levyDeclaration = await _repositoryManager.LevyDeclarationRepository.GetByIdAsync(id);
                if (levyDeclaration == null)
                    return NotFound();

                _mapper.Map(levyDeclarationDTO, levyDeclaration);

                await _repositoryManager.LevyDeclarationRepository.UpdateAsync(levyDeclaration);
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
                var levyDeclarationRecords = await _repositoryManager.LevyDeclarationRepository.GetAllAsync();

                var readLevyDeclarationRecordsMapped = _mapper.Map<List<ReadLevyDeclarationDTO>>(levyDeclarationRecords);

                return Ok(readLevyDeclarationRecordsMapped);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevyDeclaration(int id)
        {
            try
            {
                var levyDeclaration = await _repositoryManager.LevyDeclarationRepository.GetByIdAsync(id);
                if (levyDeclaration == null)
                    return NotFound();

                await _repositoryManager.LevyDeclarationRepository.DeleteAsync(levyDeclaration);
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
