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
using DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace MLS_Digital_MGM_API.Controllers 
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProBonoApplicationsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProBonoApplicationsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
    
        [HttpGet("paged")]
        public async Task<IActionResult> GetProBonoApplications(int pageNumber = 1, int pageSize = 10)
        {
            try
            {   // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<ProBonoApplication>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<ProBonoApplication, object>>[] {
                        p => p.YearOfOperation,
                        p => p.ProbonoClient
                    }
 
                };

                var proBonoApplicationspaged = await _repositoryManager.ProBonoApplicationRepository.GetPagedAsync(pagingParameters);
    
                if (proBonoApplicationspaged == null || !proBonoApplicationspaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadProBonoApplicationDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadProBonoApplicationDTO>()); // Return empty list

                }
    
                    // Map the Roles to a list of ReadFirmDTOs
                var probonoapplicationFirms = _mapper.Map<List<ReadProBonoApplicationDTO>>(proBonoApplicationspaged);

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = probonoapplicationFirms.Count;

                    return Json(new
                    {
                        draw,
                        recordsFiltered = resultTotalFiltred,
                        recordsTotal = resultTotalFiltred,
                        data = probonoapplicationFirms.ToList() // Materialize the enumerable
                    });
                }


                // Return an Ok result with the mapped Roles
                return Ok(probonoapplicationFirms);
    
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

                //update the created by field
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                proBonoApplication.CreatedById = user.Id;


                await _repositoryManager.ProBonoApplicationRepository.AddAsync(proBonoApplication);

                //update status of the pro bono application to approved since it is created by the secretariat

                proBonoApplication.ApplicationStatus = Lambda.Approved;

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
                //update the probono application approved date
                proBonoApplication.ApprovedDate = DateTime.Now;
                // Add ProBonoApplication to repository
                await _repositoryManager.ProBonoApplicationRepository.AddAsync(proBonoApplication);
                await _unitOfWork.CommitAsync();

                //added a record of an actual pro bono itself as well once the application has been saved

                var probono = this._mapper.Map<ProBono>(proBonoApplication);

                //generate a unique file number
                string fileNumber = await GenerateUniqueFileNumber();
                probono.ProBonoApplicationId = proBonoApplication.Id;
                probono.FileNumber = fileNumber;
               

                await _repositoryManager.ProBonoRepository.AddAsync(probono);
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

        private async Task<string> GenerateUniqueFileNumber()
        {
            var lastProBono = await _repositoryManager.ProBonoRepository.GetLastProBonoAsync();

            int id = lastProBono?.Id + 1 ?? 1;

            return $"MLS-ProBono{id}";
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

        [HttpGet("getprobonoapplication/{id}")]
        public async Task<IActionResult> GetProBonoApplicationById(int id)
        {
            try
            {
                var proBonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(id);
                if (proBonoApplication == null)
                {
                    return NotFound();
                }
                var proBonoApplicationDTO = _mapper.Map<ReadProBonoApplicationDTO>(proBonoApplication);
                return Ok(proBonoApplicationDTO);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }

}