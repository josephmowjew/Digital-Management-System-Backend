using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataStore.Core.DTOs.QBInvoicesDTOs;
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
using Hangfire;
using Newtonsoft.Json;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.IO;
using Microsoft.AspNetCore.Hosting;

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
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public InvoiceRequestController(
            IRepositoryManager repositoryManager,
            IErrorLogService errorLogService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<InvoiceRequestController> logger,
            IEmailService emailService,
            IWebHostEnvironment hostEnvironment)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _emailService = emailService;
            _hostEnvironment = hostEnvironment;
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
                        p => p.CreatedBy,
                        p => p.Customer,
                        p => p.QBInvoice,
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

                var invoiceRequestDTOs = _mapper.Map<List<ReadInvoiceRequestDTO>>(invoiceRequestsPaged.OrderByDescending(x => x.CreatedDate).ToList());

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

        [HttpGet("processed")]
        public async Task<IActionResult> GetProcessedInvoiceRequests(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                // Fetch the customer ID associated with the user from InvoiceRequest
                var customer = await _repositoryManager.MemberRepository.GetMemberByUserId(user.Id);

                if (customer == null)
                {
                    dataTableParams.LoadFromRequest(_httpContextAccessor);
                    var draw = dataTableParams.Draw;
                    //return NotFound("No customer found for this user.");
                    return Json(new
                    {
                        draw,
                        recordsFiltered = 0,
                        recordsTotal = 0,
                        data = Enumerable.Empty<ReadQBInvoiceDTO>()
                    });


                }

                var customerId = customer?.CustomerId;

                var pagingParameters = new PagingParameters<QBInvoice>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.CustomerId == customerId,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<QBInvoice, object>>[] {
                        p => p.Customer
                    },
                };

                var invoicesPaged = await _repositoryManager.QBInvoiceRepository.GetPagedAsync(pagingParameters);

                if (invoicesPaged == null || !invoicesPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadQBInvoiceDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadQBInvoiceDTO>());
                }

                var invoiceRequestDTOs = _mapper.Map<List<ReadQBInvoiceDTO>>(invoicesPaged);



                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = invoiceRequestDTOs.Count;
                    var totalRecords = await _repositoryManager.QBInvoiceRepository.CountAsync(pagingParameters);

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

        [HttpGet("processed/{id}")]
        public async Task<IActionResult> GetProcessedInvoiceRequests(int pageNumber = 1, int pageSize = 10, string id = "")
        {
            try
            {
                var invoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetQBInvoiceByIdAsync(id);
                if (invoiceRequest == null)
                {
                    return NotFound();
                }



                var mappedInvoiceRequest = _mapper.Map<ReadQBInvoiceDTO>(invoiceRequest);
                return Ok(mappedInvoiceRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetInvoiceRequestById");
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("cpdtrainings")]
        public async Task<IActionResult> GetCPDTrainings(int cpdTrainingId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                var pagingParameters = new PagingParameters<InvoiceRequest>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.ReferencedEntityType == "CPDTrainings" && u.ReferencedEntityId == cpdTrainingId.ToString(),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<InvoiceRequest, object>>[] {
                        p => p.CreatedBy,
                        p => p.Customer,
                        p => p.QBInvoice,

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
                //loop through the invoice requests and get the invoice and set the referenced entity to a CPDTraining
                foreach (var invoiceRequest in invoiceRequestDTOs)
                {

                    invoiceRequest.ReferencedEntity = await _repositoryManager.CPDTrainingRepository.GetAsync(ir => ir.Id == int.Parse(invoiceRequest.ReferencedEntityId));
                }


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
                _logger.LogError(ex, "Error occurred in GetCPDTrainings");
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("paidCpdTrainings")]
        public async Task<IActionResult> GetPaidCPDTrainings(int cpdTrainingId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                var pagingParameters = new PagingParameters<InvoiceRequest>
                {
                    Predicate = u => u.Status == Lambda.Paid && u.ReferencedEntityType == "CPDTrainings" && u.ReferencedEntityId == cpdTrainingId.ToString(),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<InvoiceRequest, object>>[] {
                        p => p.CreatedBy,
                        p => p.Customer,
                        p => p.QBInvoice,

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
                //loop through the invoice requests and get the invoice and set the referenced entity to a CPDTraining
                foreach (var invoiceRequest in invoiceRequestDTOs)
                {

                    invoiceRequest.ReferencedEntity = await _repositoryManager.CPDTrainingRepository.GetAsync(ir => ir.Id == int.Parse(invoiceRequest.ReferencedEntityId));
                }


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
                _logger.LogError(ex, "Error occurred in GetCPDTrainings");
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoiceRequest([FromForm] CreateInvoiceRequestDTO invoiceRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);



                var invoiceRequest = _mapper.Map<InvoiceRequest>(invoiceRequestDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                //set created by
                invoiceRequest.CreatedById = user.Id;

                //get the current year of operation
                var currentYearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();

                //set current year of operation when available
                if (currentYearOfOperation != null)
                {
                    invoiceRequest.YearOfOperationId = currentYearOfOperation.Id;
                }

                //get the member account from the user
                var memberAccount = await _repositoryManager.MemberRepository.GetMemberByUserId(user.Id);


                if (memberAccount != null)
                {
                    //set the member account id

                    invoiceRequest.CustomerId = memberAccount.CustomerId ?? null;
                }


                var existingInvoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetAsync(
                    d => d.ReferencedEntityType.Trim().Equals(invoiceRequest.ReferencedEntityType.Trim(), StringComparison.OrdinalIgnoreCase) && d.ReferencedEntityId == invoiceRequest.ReferencedEntityId && d.YearOfOperationId == invoiceRequest.YearOfOperationId && d.CreatedById == invoiceRequest.CreatedById);

                if (existingInvoiceRequest != null)
                {
                    ModelState.AddModelError("", "An invoice request with the same details already exists");
                    return BadRequest(ModelState);
                }

                //add a description of the invoice request

                await _repositoryManager.InvoiceRequestRepository.AddAsync(invoiceRequest);
                await _unitOfWork.CommitAsync();

                invoiceRequest.Description = $"MLS-{invoiceRequest.Id}";

                await _repositoryManager.InvoiceRequestRepository.UpdateAsync(invoiceRequest);

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

        [HttpGet("byMember")]
        public async Task<IActionResult> GetInvoiceRequestsByMemberId(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                var pagingParameters = new PagingParameters<InvoiceRequest>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.CreatedById == CreatedById,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<InvoiceRequest, object>>[] {
                        p => p.CreatedBy,
                        p => p.Customer,
                        p => p.QBInvoice,
                        p => p.Attachment,
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

                var invoiceRequestDTOs = _mapper.Map<List<ReadInvoiceRequestDTO>>(invoiceRequestsPaged.OrderByDescending(x => x.CreatedDate).ToList());

                foreach (var invoiceRequest in invoiceRequestDTOs)
                {
                    if(invoiceRequest.Attachment != null){

                        string newfilePath = Path.Combine("Uploads/Invoices/", invoiceRequest.Attachment.FileName);

                        invoiceRequest.Attachment.FilePath = newfilePath;
                    }

                }

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

        [HttpPost("MarkAsGenerated/{id}")]
        public async Task<IActionResult> MarkGenerated(int id, [FromForm] UpdateInvoiceRequestDTO invoiceRequestDTO)
        {
            try
            {
                var invoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetByIdAsync(id);

                if (invoiceRequest == null)
                {
                    return NotFound();
                }

                if (invoiceRequestDTO.FileUpload == null)
                {
                    return BadRequest("File upload is required");
                }

                if (string.IsNullOrEmpty(invoiceRequestDTO.InvoiceNumber))
                {
                    return BadRequest("Invoice number is required");
                }

                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Invoice")
                                    ?? new AttachmentType { Name = "Invoice" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                var hostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
                var webRootPath = hostEnvironment.WebRootPath;
                var uploadsPath = Path.Combine(webRootPath, "Uploads", "Invoices");

                // Ensure directory exists
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Generate unique filename with invoice number prefix
                var uniqueFileName = $"{invoiceRequestDTO.InvoiceNumber}_{FileNameGenerator.GenerateUniqueFileName(invoiceRequestDTO.FileUpload.FileName)}";
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                // Save file
                using (var stream = System.IO.File.Create(filePath))
                {
                    await invoiceRequestDTO.FileUpload.CopyToAsync(stream);
                }

                // Create attachment record
                var attachment = new Attachment
                {
                    FileName = uniqueFileName,
                    FilePath = filePath,
                    AttachmentTypeId = attachmentType.Id,
                    PropertyName = invoiceRequestDTO.FileUpload.Name
                };

                invoiceRequest.Status = Lambda.MarkAsGenerated;
                invoiceRequest.InvoiceNumber = invoiceRequestDTO.InvoiceNumber;
                invoiceRequest.Attachment = attachment;

                await _repositoryManager.InvoiceRequestRepository.UpdateAsync(invoiceRequest);
                await _unitOfWork.CommitAsync();

                BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(invoiceRequest.CreatedBy.Email, "Invoice Request Status", "Your invoice for a CPD has been generated", false));

                return Json(new { message = "Invoice Request marked as generated", filePath });
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("MarkAsPaid/{id}")]
        public async Task<IActionResult> MarkPaid(int id, UpdateInvoiceRequestDTO invoiceRequestDTO)
        {
            try
            {
                var invoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetByIdAsync(id);
                if (invoiceRequest == null)
                {
                    return NotFound();
                }
                else
                {
                    invoiceRequest.Status = Lambda.MarkAsPaid;
                    await _repositoryManager.InvoiceRequestRepository.UpdateAsync(invoiceRequest);
                    await _unitOfWork.CommitAsync();

                    BackgroundJob.Enqueue(() => _emailService.SendMailWithKeyVarReturn(invoiceRequest.CreatedBy.Email, "Invoice Request Status", "Your invoice for a CPD has been paid", false));

                    return Json(new { message = "Invoice Request marked as paid" });
                }

            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("cpdInvoicesByMember")]
        public async Task<IActionResult> GetCPDTrainingsByMemberId(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                var pagingParameters = new PagingParameters<InvoiceRequest>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.ReferencedEntityType == "CPDTrainings" && u.CreatedById == CreatedById.ToString(),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<InvoiceRequest, object>>[] {
                        p => p.CreatedBy,
                        p => p.Customer,
                        p => p.QBInvoice,

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
                //loop through the invoice requests and get the invoice and set the referenced entity to a CPDTraining
                foreach (var invoiceRequest in invoiceRequestDTOs)
                {

                    invoiceRequest.ReferencedEntity = await _repositoryManager.CPDTrainingRepository.GetAsync(ir => ir.Id == int.Parse(invoiceRequest.ReferencedEntityId));
                }


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
                _logger.LogError(ex, "Error occurred in GetCPDTrainings");
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> count()
        {
            try
            {
                var count = await _repositoryManager.InvoiceRequestRepository.GetPendingInvoiceRequestsCountAsync();

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
