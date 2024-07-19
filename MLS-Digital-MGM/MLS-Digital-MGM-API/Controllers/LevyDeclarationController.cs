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
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;


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
                    Includes = new Expression<Func<LevyDeclaration, object>>[] {
                        p => p.Attachments,
                        p => p.InvoiceRequest,
                    },
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
        public async Task<IActionResult> AddLevyDeclaration([FromForm] CreateLevyDeclarationDTO levyDeclarationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                var levyDeclaration = _mapper.Map<LevyDeclaration>(levyDeclarationDTO);

                var currentLevyPercent = await _repositoryManager.LevyPercentRepository.GetCurrentLevyPercentageAsync();

                if (currentLevyPercent!= null)
                {
                    levyDeclaration.Percentage = Convert.ToDecimal(currentLevyPercent.PercentageValue);
                    
                    levyDeclaration.LevyAmount = levyDeclaration.Percentage / 100m * levyDeclaration.Revenue;
                }

                //get firm with the id passed 
                var firm = await _repositoryManager.FirmRepository.GetByIdAsync(levyDeclaration.FirmId);

                if(firm != null)
                {
                    levyDeclaration.Firm = firm;
                }

                 // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "LevyDeclaration")
                                    ?? new AttachmentType { Name = "LevyDeclaration" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }
                if (levyDeclarationDTO.Attachments != null && levyDeclarationDTO.Attachments.Count > 0)
                {
                    levyDeclaration.Attachments = await SaveAttachmentsAsync(levyDeclarationDTO.Attachments, attachmentType.Id);
                }

                //generate an invoice request for the levy declaration

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                var currentYearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();

                InvoiceRequest levyInvoice = new InvoiceRequest()
                {
                    CreatedById = user.Id,
                    YearOfOperationId = currentYearOfOperation.Id,
                    CreatedDate = DateTime.UtcNow,
                    Status = Lambda.Pending,
                    UpdatedDate = DateTime.UtcNow,
                    Amount = Convert.ToDouble(levyDeclaration.LevyAmount),
                    CustomerId = firm.CustomerId,
                    ReferencedEntityId = "N/A",
                    ReferencedEntityType = "Levy",
                    Description = "MLS",
                };

                await _repositoryManager.LevyDeclarationRepository.AddAsync(levyDeclaration);
                await _unitOfWork.CommitAsync();

                levyInvoice.Description = $"MLS-{levyInvoice.Id}";
                await _repositoryManager.InvoiceRequestRepository.UpdateAsync(levyInvoice);
                await _unitOfWork.CommitAsync();

                levyDeclaration.InvoiceRequestId = levyInvoice.Id;
                await _repositoryManager.LevyDeclarationRepository.UpdateAsync(levyDeclaration);
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


        private async Task<List<Attachment>> SaveAttachmentsAsync(IEnumerable<IFormFile> attachments, int attachmentTypeId)
        {
            var attachmentsList = new List<Attachment>();
            var hostEnvironment = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var webRootPath = hostEnvironment.WebRootPath;

            // Check if webRootPath is null or empty
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null or empty");
            }

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/LevyDeclarationAttachments" );

        

            // Ensure the directory exists
            if (!Directory.Exists(AttachmentsPath))
            {
                Directory.CreateDirectory(AttachmentsPath);
            
            }

            foreach (var attachment in attachments)
            {
                if (attachment == null || string.IsNullOrWhiteSpace(attachment.FileName))
                {
                
                    continue;
                }

                var uniqueFileName = FileNameGenerator.GenerateUniqueFileName(attachment.FileName);
                var filePath = Path.Combine(AttachmentsPath, uniqueFileName);

            

                try
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await attachment.CopyToAsync(stream);
                    }

                    attachmentsList.Add(new Attachment
                    {
                        FileName = uniqueFileName,
                        FilePath = filePath,
                        AttachmentTypeId = attachmentTypeId,
                        PropertyName = attachment.Name
                    });
                }
                catch (Exception ex)
                {
                
                    throw;
                }
            }

            return attachmentsList;
        }

    }
}
