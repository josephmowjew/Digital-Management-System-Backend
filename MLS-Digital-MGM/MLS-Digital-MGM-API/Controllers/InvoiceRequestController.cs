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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DataStore.Core.DTOs.InvoiceRequest;
using MLS_Digital_MGM.DataStore.Helpers;
using DataStore.Helpers;
using System.Linq.Expressions;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InvoiceRequestController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<InvoiceRequestController> _logger;

        public InvoiceRequestController(
            IRepositoryManager repositoryManager,
            IErrorLogService errorLogService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<InvoiceRequestController> logger)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetInvoiceRequests(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                var pagingParameters = new PagingParameters<InvoiceRequest>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<InvoiceRequest, object>>[] {
                        p => p.CreatedBy
                    },
                };

                var invoiceRequestsPaged = await _repositoryManager.InvoiceRequestRepository.GetPagedAsync(pagingParameters);

                if (invoiceRequestsPaged == null || !invoiceRequestsPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadInvoiceRequestDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadInvoiceRequestDTO>());
                }

                var invoiceRequestDTOs = _mapper.Map<List<ReadInvoiceRequestDTO>>(invoiceRequestsPaged);

              

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = invoiceRequestDTOs.Count;
                    var totalRecords = await _repositoryManager.InvoiceRequestRepository.CountAsync(pagingParameters);

                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = invoiceRequestDTOs.ToList()
                    });
                }

                return Ok(invoiceRequestDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetInvoiceRequests");
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoiceRequest([FromBody] CreateInvoiceRequestDTO invoiceRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var invoiceRequest = _mapper.Map<InvoiceRequest>(invoiceRequestDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                invoiceRequest.CreatedById = user.Id;

                var existingInvoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetAsync(
                    d => d.ReferencedEntityType.Trim().Equals(invoiceRequest.ReferencedEntityType.Trim(), StringComparison.OrdinalIgnoreCase) && d.ReferencedEntityId == invoiceRequest.ReferencedEntityId && d.YearOfOperationId == invoiceRequest.YearOfOperationId);

                if (existingInvoiceRequest != null)
                {
                    ModelState.AddModelError(nameof(invoiceRequestDTO.CustomerId), "An invoice request with the same details already exists");
                    return BadRequest(ModelState);
                }

                await _repositoryManager.InvoiceRequestRepository.AddAsync(invoiceRequest);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetInvoiceRequestById", new { id = invoiceRequest.Id }, invoiceRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddInvoiceRequest");
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceRequestById(int id)
        {
            try
            {
                var invoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetByIdAsync(id);
                if (invoiceRequest == null)
                {
                    return NotFound();
                }



                var mappedInvoiceRequest = _mapper.Map<ReadInvoiceRequestDTO>(invoiceRequest);
                return Ok(mappedInvoiceRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetInvoiceRequestById");
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoiceRequest(int id, [FromBody] UpdateInvoiceRequestDTO invoiceRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var invoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetByIdAsync(id);
                if (invoiceRequest == null)
                    return NotFound();

                _mapper.Map(invoiceRequestDTO, invoiceRequest);
                await _repositoryManager.InvoiceRequestRepository.UpdateAsync(invoiceRequest);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in UpdateInvoiceRequest");
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoiceRequest(int id)
        {
            try
            {
                var invoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetByIdAsync(id);
                if (invoiceRequest == null)
                    return NotFound();

                await _repositoryManager.InvoiceRequestRepository.DeleteAsync(invoiceRequest);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteInvoiceRequest");
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
