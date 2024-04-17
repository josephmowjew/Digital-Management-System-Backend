using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataStore.Core.DTOs.ProBonoReport;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MLS_Digital_MGM_API.Controllers
{
[Route("api/[controller]")]
public class ProBonoReportsController : Controller
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IErrorLogService _errorLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProBonoReportsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _errorLogService = errorLogService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetProBonoReports(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var reports = await _repositoryManager.ProBonoReportRepository.GetPagedAsync(r => true, pageNumber, pageSize,u => u.Attachments);

            if (reports == null || !reports.Any())
            {
                return NotFound();
            }

            var mappedReports = _mapper.Map<IEnumerable<ReadProBonoReportDTO>>(reports);

            return Ok(mappedReports);
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

 [HttpPost]
 public async Task<IActionResult> AddProBonoReport([FromForm] CreateProBonoReportDTO reportDTO)
 {
     try
     {
         if (!ModelState.IsValid)
         {
             return BadRequest(ModelState);
         }
 
         var report = _mapper.Map<ProBonoReport>(reportDTO);
 
         var existingReport = await _repositoryManager.ProBonoReportRepository.GetAsync(r => r.Description.Trim().Equals(report.Description.Trim(), StringComparison.OrdinalIgnoreCase) && r.ProBonoId == report.ProBonoId);
         if (existingReport != null)
         {
             ModelState.AddModelError(nameof(reportDTO.Description), "A report with the same description and ProBono Report already exists");
             return BadRequest(ModelState);
         }

        
        // Get or create attachment type
        var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "ProBonoReport") 
                            ?? new AttachmentType { Name = "ProBonoReport" };

        // Add attachment type if it doesn't exist
        if (attachmentType.Id == 0)
        {
            await _repositoryManager.AttachmentTypeRepository.AddAsync(attachmentType);
            await _unitOfWork.CommitAsync();
        }
         if (reportDTO.Attachments != null && reportDTO.Attachments.Count > 0)
         {
             report.Attachments = await SaveAttachmentsAsync(reportDTO.Attachments, attachmentType.Id);
         }
 
         await _repositoryManager.ProBonoReportRepository.AddAsync(report);
         await _unitOfWork.CommitAsync();
 
         return CreatedAtAction("GetProBonoReports", new { id = report.Id }, report);
     }
     catch (Exception ex)
     {
         await _errorLogService.LogErrorAsync(ex);
         return StatusCode(500, "Internal server error");
     }
 }
 


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProBonoReport(int id, [FromForm] UpdateProBonoReportDTO reportDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
    
            var report = await _repositoryManager.ProBonoReportRepository.GetByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }
    
            var attachmentType = await _repositoryManager.AttachmentTypeRepository.GetAsync(d => d.Name == "ProBonoReport") 
                                ?? new AttachmentType { Name = "ProBonoReport" };
    
            if (reportDTO.Attachments?.Any() == true)
            {
                report.Attachments = await SaveAttachmentsAsync(reportDTO.Attachments, attachmentType.Id);
            }
    
            _mapper.Map(reportDTO, report);
            await _repositoryManager.ProBonoReportRepository.UpdateAsync(report);
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
    public async Task<IActionResult> DeleteProBonoReport(int id)
    {
        try
        {
            var report = await _repositoryManager.ProBonoReportRepository.GetByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            await _repositoryManager.ProBonoReportRepository.DeleteAsync(report);
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
     var proBonoReportAttachmentsPath = Path.Combine(webRootPath, "Uploads/ProBonoReportAttachments");
 
     Directory.CreateDirectory(proBonoReportAttachmentsPath);
 
     foreach (var attachment in attachments)
     {
         var filePath = Path.Combine(proBonoReportAttachmentsPath, attachment.FileName);
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
