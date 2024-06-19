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
using DataStore.Core.DTOs.MemberQualification;
using DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;
using System.Composition;

namespace MLS_Digital_MGM_API.Controllers 
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MemberQualificationsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public MemberQualificationsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetMemberQualifications(int pageNumber = 1, int pageSize = 10,int memberId = 0)
        {
            try
            {

                 var dataTableParams = new DataTablesParameters();
                 var pagingParameters = new PagingParameters<MemberQualification>();

                 if (memberId != 0)
                {
                    pagingParameters.Predicate = u => u.Status != Lambda.Deleted && u.MemberId == memberId;
                }
                else
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadMemberQualificationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadMemberQualificationDTO>());
                }

               
                    
                    pagingParameters.PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber;
                    pagingParameters.PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize;
                    pagingParameters.SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null;
                    pagingParameters.SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null;
                    pagingParameters.SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null;
                    pagingParameters.Includes = new Expression<Func<MemberQualification, object>>[]{
                        q => q.Member,
                        q=> q.QualificationType,
                        q => q.Attachments
                       
                        
                    };

                var memberQualifications = await _repositoryManager.MemberQualificationRepository.GetPagedAsync(pagingParameters);

                if (memberQualifications == null || !memberQualifications.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadMemberQualificationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadMemberQualificationDTO>());
                }

                var mappedMemberQualifications = _mapper.Map<List<ReadMemberQualificationDTO>>(memberQualifications);

                 foreach (var report in mappedMemberQualifications)
                {
                    foreach (var attachment in report.Attachments)
                    {
                        string attachmentTypeName = attachment.AttachmentType.Name;


                        string newfilePath = Path.Combine("/uploads/QualificationAttachments/", attachment.FileName);

                        attachment.FilePath = newfilePath;
                    }
                }

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedMemberQualifications.Count;
                    var totalRecords = await _repositoryManager.MemberQualificationRepository.CountAsync(pagingParameters);

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = totalRecords, 
                        recordsTotal = totalRecords, 
                        data = mappedMemberQualifications.ToList() 
                    });
                }

                return Ok(mappedMemberQualifications);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMemberQualification([FromForm] CreateMemberQualificationDTO memberQualificationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var memberQualification = _mapper.Map<MemberQualification>(memberQualificationDTO);

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

                var existingMemberQualification = await _repositoryManager.MemberQualificationRepository.GetAsync(m => m.MemberId == memberQualification.MemberId && m.Name.Trim().ToLower() == memberQualification.Name.Trim().ToLower() && m.QualificationTypeId == memberQualification.QualificationTypeId);
                if (existingMemberQualification != null)
                {
                    ModelState.AddModelError(nameof(memberQualificationDTO.MemberId), "A member qualification with the same member and qualification already exists");
                    return BadRequest(ModelState);
                }

                            // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "MemberQualification") 
                                ?? new AttachmentType { Name = "MemberQualification" };
                 // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }
                if (memberQualificationDTO.Attachments != null && memberQualificationDTO.Attachments.Count > 0)
                {
                    memberQualification.Attachments = await SaveAttachmentsAsync(memberQualificationDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.MemberQualificationRepository.AddAsync(memberQualification);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetMemberQualification", new { id = (memberQualification.Id) }, memberQualification);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMemberQualification(int id, [FromForm] UpdateMemberQualificationDTO memberQualificationDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var memberQualification = await _repositoryManager.MemberQualificationRepository.GetByIdAsync(id);
                if (memberQualification == null)
                {
                    return NotFound();
                }

                _mapper.Map(memberQualificationDTO, memberQualification);

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "MemberQualification") 
                                ?? new AttachmentType { Name = "MemberQualification" };
    
                if (memberQualificationDTO.Attachments?.Any() == true)
                {
                    memberQualification.Attachments = await SaveAttachmentsAsync(memberQualificationDTO.Attachments, attachmentType.Id);
                }

                await _repositoryManager.MemberQualificationRepository.UpdateAsync(memberQualification);
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
        public async Task<IActionResult> DeleteMemberQualification(int id)
        {
            try
            {
                var memberQualification = await _repositoryManager.MemberQualificationRepository.GetByIdAsync(id);
                if (memberQualification == null)
                {
                    return NotFound();
                }

                await _repositoryManager.MemberQualificationRepository.DeleteAsync(memberQualification);
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
        public async Task<IActionResult> GetMemberQualification(int id)
        {
            try
            {
                var memberQualification = await _repositoryManager.MemberQualificationRepository.GetMemberQualificationByMemberId(id);
                if (memberQualification == null)
                {
                    return NotFound();
                }


                foreach (var attachment in memberQualification.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;


                    string newfilePath = Path.Combine("/uploads/QualificationAttachments/", attachment.FileName);

                    attachment.FilePath = newfilePath;
                }

                var mappedMemberQualification = _mapper.Map<ReadMemberQualificationDTO>(memberQualification);

                return Ok(mappedMemberQualification);
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

            var AttachmentsPath = Path.Combine(webRootPath, "Uploads/QualificationAttachments" );

           
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