
using AutoMapper;
using DataStore.Core.DTOs.Signature;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SignaturesController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public SignaturesController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> AddSignature([FromForm] CreateSignatureDTO signatureDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var signature = _mapper.Map<Signature>(signatureDTO);
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                signature.CreatedById = user.Id;

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Signature") 
                                    ?? new AttachmentType { Name = "Signature" };

                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                if (signatureDTO.Attachments != null && signatureDTO.Attachments.Count > 0)
                {
                    signature.Attachments = await SaveAttachmentsAsync(signatureDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.SignatureRepository.AddAsync(signature);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetSignatures", new { id = signature.Id }, signature);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetSignatures(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                // get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;

                var pagingParameters = new PagingParameters<Signature>
                {
                    Predicate = s => s.Status != Lambda.Deleted && s.CreatedById == CreatedById,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<Signature, object>>[] {
                        p => p.YearOfOperation,
                        p => p.CreatedBy
                    },
                };

                var pagedSignatures = await _repositoryManager.SignatureRepository.GetPagedAsync(pagingParameters);

                if (pagedSignatures == null || !pagedSignatures.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadSignatureDTO>(),
                        });
                    }
                    return Ok(Enumerable.Empty<ReadSignatureDTO>());
                }

                var mappedSignatures = _mapper.Map<List<ReadSignatureDTO>>(pagedSignatures);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltered = mappedSignatures.Count;
                    var totalRecords = await _repositoryManager.SignatureRepository.CountAsync(pagingParameters);

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = totalRecords, 
                        recordsTotal = totalRecords, 
                        data = mappedSignatures.ToList()
                    });
                }

                return Ok(mappedSignatures);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSignature(int id)
        {
            try
            {   
                var signature = await _repositoryManager.SignatureRepository.GetSignatureByIdAsync(id);
                if (signature == null)
                {
                    return NotFound();
                }

                await _repositoryManager.SignatureRepository.DeleteAsync(signature);
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
        public async Task<IActionResult> EditSignature(int id, [FromForm] UpdateSignatureDTO signatureDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                var signature = await _repositoryManager.SignatureRepository.GetSignatureByIdAsync(id);
                if (signature == null)
                {
                    return NotFound();
                }

                _mapper.Map(signatureDTO, signature);

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "Signature") 
                                    ?? new AttachmentType { Name = "Signature" };

                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                if (signatureDTO.Attachments != null && signatureDTO.Attachments.Count > 0)
                {
                    signature.Attachments = await SaveAttachmentsAsync(signatureDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.SignatureRepository.UpdateAsync(signature);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("singleSignature/{id}")]
        public async Task<IActionResult> GetSignatureById(int id)
        {
            try
            {
                var signature = await _repositoryManager.SignatureRepository.GetSignatureByIdAsync(id);
                if (signature == null)
                {
                    return NotFound();
                }

                foreach (var attachment in signature.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;
                    string newFilePath = Path.Combine($"http://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.SignatureFolderName}", attachment.FileName);
                    attachment.FilePath = newFilePath;
                }

                var readSignatureDTO = _mapper.Map<ReadSignatureDTO>(signature);

                return Ok(readSignatureDTO);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("signature/{name}")]
        public async Task<IActionResult> GetSignatureByName(string name)
        {
            try
            {
                var signature = await _repositoryManager.SignatureRepository.GetSignatureByNameAsync(name);
                if (signature == null)
                {
                    return NotFound();
                }

                var signatureDTO = _mapper.Map<ReadSignatureDTO>(signature);

                foreach (var attachment in signatureDTO.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;
                    string newFilePath = Path.Combine($"http://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.SignatureFolderName}", attachment.FileName);
                    attachment.FilePath = newFilePath;
                }

                return Ok(signatureDTO);
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

            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                throw new ArgumentNullException(nameof(webRootPath), "Web root path cannot be null or empty");
            }

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/SignatureAttachments");

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
                catch (Exception)
                {
                    throw;
                }
            }

            return attachmentsList;
        }
    }
}
