// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using AutoMapper;
// using DataStore.Core.DTOs.InvoiceRequestType;
// using DataStore.Core.Services;
// using DataStore.Persistence.Interfaces;
// using DataStore.Core.Services.Interfaces;
// using DataStore.Core.Models;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using MLS_Digital_MGM.DataStore.Helpers;
// using DataStore.Helpers;

// namespace MLS_Digital_MGM_API.Controllers
// {
//     [Route("api/[controller]")]
//     [Authorize(AuthenticationSchemes = "Bearer")]
//     public class InvoiceRequestTypeController : Controller
//     {
//         private readonly IRepositoryManager _repositoryManager;
//         private readonly IErrorLogService _errorLogService;
//         private readonly IUnitOfWork _unitOfWork;
//         private readonly IMapper _mapper;
//         private readonly IHttpContextAccessor _httpContextAccessor;
//         private readonly ILogger<InvoiceRequestTypeController> _logger;

//         public InvoiceRequestTypeController(
//             IRepositoryManager repositoryManager,
//             IErrorLogService errorLogService,
//             IUnitOfWork unitOfWork,
//             IMapper mapper,
//             IHttpContextAccessor httpContextAccessor,
//             ILogger<InvoiceRequestTypeController> logger)
//         {
//             _repositoryManager = repositoryManager;
//             _errorLogService = errorLogService;
//             _unitOfWork = unitOfWork;
//             _mapper = mapper;
//             _httpContextAccessor = httpContextAccessor;
//             _logger = logger;
//         }

//         [HttpGet("paged")]
//         public async Task<IActionResult> GetInvoiceRequestTypes(int pageNumber = 1, int pageSize = 10)
//         {
//             try
//             {
//                 var dataTableParams = new DataTablesParameters();
//                 string username = _httpContextAccessor.HttpContext.User.Identity.Name;
//                 var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
//                 string CreatedById = user.Id;

//                 var pagingParameters = new PagingParameters<InvoiceRequestType>
//                 {
//                     Predicate = u => u.Status != Lambda.Deleted,
//                     PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
//                     PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
//                     SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
//                     SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
//                     SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
//                 };

//                 var invoiceRequestTypesPaged = await _repositoryManager.InvoiceRequestTypeRepository.GetPagedAsync(pagingParameters);

//                 if (invoiceRequestTypesPaged == null || !invoiceRequestTypesPaged.Any())
//                 {
//                     if (dataTableParams.LoadFromRequest(_httpContextAccessor))
//                     {
//                         var draw = dataTableParams.Draw;
//                         return Json(new
//                         {
//                             draw,
//                             recordsFiltered = 0,
//                             recordsTotal = 0,
//                             data = Enumerable.Empty<ReadInvoiceRequestTypeDTO>()
//                         });
//                     }
//                     return Ok(Enumerable.Empty<ReadInvoiceRequestTypeDTO>());
//                 }

//                 var invoiceRequestTypeDTOs = _mapper.Map<List<ReadInvoiceRequestTypeDTO>>(invoiceRequestTypesPaged);

//                 if (dataTableParams.LoadFromRequest(_httpContextAccessor))
//                 {
//                     var draw = dataTableParams.Draw;
//                     var resultTotalFiltered = invoiceRequestTypeDTOs.Count;
//                     var totalRecords = await _repositoryManager.InvoiceRequestTypeRepository.CountAsync(pagingParameters);

//                     return Json(new
//                     {
//                         draw,
//                         recordsFiltered = totalRecords,
//                         recordsTotal = totalRecords,
//                         data = invoiceRequestTypeDTOs.ToList()
//                     });
//                 }

//                 return Ok(invoiceRequestTypeDTOs);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error occurred in GetInvoiceRequestTypes");
//                 await _errorLogService.LogErrorAsync(ex);
//                 return StatusCode(500, "Internal server error");
//             }
//         }

//         [HttpPost]
//         public async Task<IActionResult> AddInvoiceRequestType([FromForm] CreateInvoiceRequestTypeDTO invoiceRequestTypeDTO)
//         {
//             try
//             {
//                 if (!ModelState.IsValid)
//                     return BadRequest(ModelState);

//                 var invoiceRequestType = _mapper.Map<InvoiceRequestType>(invoiceRequestTypeDTO);
//                 string username = _httpContextAccessor.HttpContext.User.Identity.Name;
//                 var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                

//                 var existingInvoiceRequestType = await _repositoryManager.InvoiceRequestTypeRepository.GetAsync(
//                     d => d.Name.Trim().Equals(invoiceRequestType.Name.Trim(), StringComparison.OrdinalIgnoreCase));

//                 if (existingInvoiceRequestType != null)
//                 {
//                     ModelState.AddModelError(nameof(invoiceRequestTypeDTO.Name), "An invoice request type with the same name already exists");
//                     return BadRequest(ModelState);
//                 }

//                 await _repositoryManager.InvoiceRequestTypeRepository.AddAsync(invoiceRequestType);
//                 await _unitOfWork.CommitAsync();

//                 return CreatedAtAction("GetInvoiceRequestTypeById", new { id = invoiceRequestType.Id }, invoiceRequestType);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error occurred in AddInvoiceRequestType");
//                 await _errorLogService.LogErrorAsync(ex);
//                 return StatusCode(500, "Internal server error");
//             }
//         }

//         [HttpGet("{id}")]
//         public async Task<IActionResult> GetInvoiceRequestTypeById(int id)
//         {
//             try
//             {
//                 var invoiceRequestType = await _repositoryManager.InvoiceRequestTypeRepository.GetByIdAsync(id);
//                 if (invoiceRequestType == null)
//                 {
//                     return NotFound();
//                 }

//                 var mappedInvoiceRequestType = _mapper.Map<ReadInvoiceRequestTypeDTO>(invoiceRequestType);
//                 return Ok(mappedInvoiceRequestType);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error occurred in GetInvoiceRequestTypeById");
//                 await _errorLogService.LogErrorAsync(ex);
//                 return StatusCode(500, "Internal server error");
//             }
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateInvoiceRequestType(int id, [FromForm] UpdateInvoiceRequestTypeDTO invoiceRequestTypeDTO)
//         {
//             try
//             {
//                 if (!ModelState.IsValid)
//                     return BadRequest(ModelState);

//                 var invoiceRequestType = await _repositoryManager.InvoiceRequestTypeRepository.GetByIdAsync(id);
//                 if (invoiceRequestType == null)
//                     return NotFound();

//                 _mapper.Map(invoiceRequestTypeDTO, invoiceRequestType);
//                 await _repositoryManager.InvoiceRequestTypeRepository.UpdateAsync(invoiceRequestType);
//                 await _unitOfWork.CommitAsync();

//                 return Ok();
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error occurred in UpdateInvoiceRequestType");
//                 await _errorLogService.LogErrorAsync(ex);
//                 return StatusCode(500, "Internal server error");
//             }
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteInvoiceRequestType(int id)
//         {
//             try
//             {
//                 var invoiceRequestType = await _repositoryManager.InvoiceRequestTypeRepository.GetByIdAsync(id);
//                 if (invoiceRequestType == null)
//                     return NotFound();

//                 await _repositoryManager.InvoiceRequestTypeRepository.DeleteAsync(invoiceRequestType);
//                 await _unitOfWork.CommitAsync();

//                 return Ok();
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Error occurred in DeleteInvoiceRequestType");
//                 await _errorLogService.LogErrorAsync(ex);
//                 return StatusCode(500, "Internal server error");
//             }
//         }
//     }
// }
