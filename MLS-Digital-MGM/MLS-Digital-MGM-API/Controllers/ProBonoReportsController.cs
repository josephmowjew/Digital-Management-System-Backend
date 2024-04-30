using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DataStore.Core.DTOs.ProBonoReport;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Web;

namespace MLS_Digital_MGM_API.Controllers
{
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ProBonoReportsController : Controller
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IErrorLogService _errorLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ProBonoReportsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
    {
        _repositoryManager = repositoryManager;
        _errorLogService = errorLogService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetProBonoReports(int pageNumber = 1, int pageSize = 10, int probonoId = 0)
    {
        try
        {
            string username = _httpContextAccessor.HttpContext.User.Identity.Name;

            //get user id from username
            var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
            string CreatedById = user.Id;


                string currentRole  = Lambda.GetCurrentUserRole(_repositoryManager,user.Id);
                // Create a new DataTablesParameters object
            var dataTableParams = new DataTablesParameters();
            var pagingParameters = new PagingParameters<ProBonoReport>();

            if (probonoId != 0)
            {
                pagingParameters.Predicate = u => u.Status != Lambda.Deleted && u.ProBonoId == probonoId;
            }
            else
            {
                pagingParameters.Predicate = u => u.Status != Lambda.Deleted;
            }

            pagingParameters.PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber;
            pagingParameters.PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize;
            pagingParameters.SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null;
            pagingParameters.SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null;
            pagingParameters.SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null;
            pagingParameters.Includes = new Expression<Func<ProBonoReport, object>>[]{
                p => p.ProBono,
                p => p.Attachments
                
            };

            var reports = await _repositoryManager.ProBonoReportRepository.GetPagedAsync(pagingParameters);

            if (reports == null || !reports.Any())
            {
                   if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadProBonoReportDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadProBonoReportDTO>()); // Return empty list
            }

            var mappedReports = _mapper.Map<List<ReadProBonoReportDTO>>(reports);

           
            foreach (var report in mappedReports)
            {
                foreach (var attachment in report.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;


                      string newfilePath = Path.Combine("/uploads/ProBonoReportAttachments/", attachment.FileName);

                    attachment.FilePath = newfilePath;
                }
            }
            // Return datatable JSON if the request came from a datatable
            if (dataTableParams.LoadFromRequest(_httpContextAccessor))
            {
                var draw = dataTableParams.Draw;
                var resultTotalFiltred = mappedReports.Count;

                return Json(new 
                { 
                    draw, 
                    recordsFiltered = resultTotalFiltred, 
                    recordsTotal = resultTotalFiltred, 
                    data = mappedReports.ToList() // Materialize the enumerable
                });
            }

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
