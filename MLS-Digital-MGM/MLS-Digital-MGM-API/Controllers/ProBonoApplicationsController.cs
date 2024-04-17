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
using DataStore.Core.DTOs.ProBonoApplication;
using Microsoft.Extensions.Hosting;

namespace MLS_Digital_MGM_API.Controllers 
{
    [Route("api/[controller]")]
    public class ProBonoApplicationsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public ProBonoApplicationsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    
        [HttpGet("paged")]
        public async Task<IActionResult> GetProBonoApplications(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var proBonoApplications = await _repositoryManager.ProBonoApplicationRepository.GetPagedAsync(d => true, pageNumber, pageSize);
    
                if (proBonoApplications == null || !proBonoApplications.Any())
                {
                    return NotFound();
                }
    
                var mappedProBonoApplications = _mapper.Map<IEnumerable<ReadProBonoApplicationDTO>>(proBonoApplications);
    
                return Ok(mappedProBonoApplications);
    
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    
        [HttpPost]
        public async Task<IActionResult> AddProBonoApplication([FromForm] CreateProBonoApplicationDTO proBonoApplicationDTO)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var proBonoApplication = _mapper.Map<ProBonoApplication>(proBonoApplicationDTO);

                // Check if a ProBonoApplication with the same nature of dispute already exists
                var existingProBonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetAsync(
                    d => d.NatureOfDispute.Trim().Equals(proBonoApplication.NatureOfDispute.Trim(), StringComparison.OrdinalIgnoreCase) && 
                    d.ApprovedDate == null);

                if (existingProBonoApplication != null)
                {
                    ModelState.AddModelError(nameof(proBonoApplicationDTO.NatureOfDispute), "A pro bono application with the same nature of dispute already exists");
                    return BadRequest(ModelState);
                }

                // Get or create attachment type
                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "ProBonoApplication") 
                                    ?? new AttachmentType { Name = "ProBonoApplication" };

                // Add attachment type if it doesn't exist
                if (attachmentType.Id == 0)
                {
                    await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
                    await _unitOfWork.CommitAsync();
                }

                // Save attachments if any
                if (proBonoApplicationDTO.Attachments?.Any() == true)
                {
                    proBonoApplication.Attachments = await SaveAttachmentsAsync(proBonoApplicationDTO.Attachments, attachmentType.Id);
                }

                // Add ProBonoApplication to repository
                await _repositoryManager.ProBonoApplicationRepository.AddAsync(proBonoApplication);
                await _unitOfWork.CommitAsync();

                // Return created ProBonoApplication
                return CreatedAtAction("GetProBonoApplications", new { id = proBonoApplication.Id }, proBonoApplication);
            }
            catch (Exception ex)
            {
                // Log error and return internal server error
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProBonoApplication(int id, [FromBody] UpdateProBonoApplicationDTO proBonoApplicationDTO, IEnumerable<IFormFile> attachments)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var proBonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(id);
                if (proBonoApplication == null)
                {
                    return NotFound();
                }

                var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "ProBonoApplication") 
                                    ?? new AttachmentType { Name = "ProBonoApplication" };

                if (attachments?.Any() == true)
                {
                    proBonoApplication.Attachments = await SaveAttachmentsAsync(attachments, attachmentType.Id);
                }

                _mapper.Map(proBonoApplicationDTO, proBonoApplication);
                await _repositoryManager.ProBonoApplicationRepository.UpdateAsync(proBonoApplication);
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
        public async Task<IActionResult> DeleteProBonoApplication(int id)
        {
            try
            {
                var proBonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(id);
                if (proBonoApplication == null)
                {
                    return NotFound();
                }
    
                await _repositoryManager.ProBonoApplicationRepository.DeleteAsync(proBonoApplication);
                await _unitOfWork.CommitAsync();
    
                return NoContent();
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
            var proBonoApplicationAttachmentsPath = Path.Combine(webRootPath, "Uploads/ProBonoApplicationsAttachments");

            Directory.CreateDirectory(proBonoApplicationAttachmentsPath);

            foreach (var attachment in attachments)
            {
                var filePath = Path.Combine(proBonoApplicationAttachmentsPath, attachment.FileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await attachment.CopyToAsync(stream);
                }

                attachmentsList.Add(new Attachment
                {
                    FileName = attachment.FileName,
                    FilePath = filePath,
                    AttachmentTypeId = attachmentTypeId
                });
            }

            return attachmentsList;
        }

    }

}