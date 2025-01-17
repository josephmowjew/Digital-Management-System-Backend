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
using DataStore.Core.DTOs.License;
using Microsoft.Extensions.Hosting;
using DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Hangfire;
using DataStore.Core.DTOs.Attachment;
using Newtonsoft.Json;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LicenseController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public LicenseController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _configuration = configuration;
        }

        // GET api/license/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetLicenses(int pageNumber = 1, int pageSize = 10, int memberId = 0)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);

                var pagingParameters = new PagingParameters<License>
                {
                    Predicate = l => l.Status != Lambda.Deleted && (memberId > 0 ? l.MemberId == memberId : true),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<License, object>>[] {
                        l => l.Member,
                        l => l.YearOfOperation,
                        l => l.LicenseApplication
                    }
                };

                var licensesPaged = await _repositoryManager.LicenseRepository.GetPagedAsync(pagingParameters);

                if (licensesPaged == null || !licensesPaged.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        return Json(new { draw = dataTableParams.Draw, recordsFiltered = 0, recordsTotal = 0, data = Enumerable.Empty<ReadLicenseDTO>() });
                    }
                    return Ok(Enumerable.Empty<ReadLicenseDTO>());
                }

                var licenseDTOs = _mapper.Map<List<ReadLicenseDTO>>(licensesPaged);

                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var totalRecords = await _repositoryManager.LicenseRepository.CountAsync(pagingParameters);
                    return Json(new { draw = dataTableParams.Draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = licenseDTOs });
                }

                return Ok(licenseDTOs);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/license/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicenseById(int id)
        {
            try
            {
                var license = await _repositoryManager.LicenseRepository.GetLicenseById(id);
                if (license == null)
                {
                    return NotFound();
                }
                var licenseDTO = _mapper.Map<ReadLicenseDTO>(license);

                List<ReadAttachmentDTO> signatures = new List<ReadAttachmentDTO>();

                //get the honorary sec signatue 
                var honorarySignature = await _repositoryManager.SignatureRepository.GetSignatureByNameAsync(Lambda.HonorarySecretarySignature);

                if (honorarySignature != null)
                {
                    var attachment = honorarySignature.Attachments.FirstOrDefault();
                    if (attachment != null)
                    {
                        var attachmentDTO = _mapper.Map<ReadAttachmentDTO>(attachment);
                        attachmentDTO.FilePath = Path.Combine($"{Lambda.http}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.SignatureFolderName}", attachment.FileName);
                        signatures.Add(attachmentDTO);
                    }
                }

                //get the chairman's signature
                var seal = await _repositoryManager.StampRepository.GetStampByNameAsync(Lambda.Seal);
                if (seal != null)
                {
                    var attachment = seal.Attachments.FirstOrDefault();
                    if (attachment != null)
                    {
                        var attachmentDTO = _mapper.Map<ReadAttachmentDTO>(attachment);
                        attachmentDTO.FilePath = Path.Combine($"{Lambda.http}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.StampFolderName}", attachment.FileName);
                        signatures.Add(attachmentDTO);
                    }
                }

                //get the president signature
                var presidentSignature = await _repositoryManager.SignatureRepository.GetSignatureByNameAsync(Lambda.PresidentSignature);
                if (presidentSignature != null)
                {
                    var attachment = presidentSignature.Attachments.FirstOrDefault();
                    if (attachment != null)
                    {
                        var attachmentDTO = _mapper.Map<ReadAttachmentDTO>(attachment);
                        attachmentDTO.FilePath = Path.Combine($"{Lambda.http}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.SignatureFolderName}", attachment.FileName);
                        signatures.Add(attachmentDTO);
                    }
                }

                //get the chairman's signature
                /*var chairmanSignature = await _repositoryManager.SignatureRepository.GetSignatureByNameAsync(Lambda.ChairmanSignature);
                if (chairmanSignature != null)
                {
                    var attachment = chairmanSignature.Attachments.FirstOrDefault();
                    if (attachment != null)
                    {
                        var attachmentDTO = _mapper.Map<ReadAttachmentDTO>(attachment);
                        attachmentDTO.FilePath = Path.Combine($"{Lambda.http}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.SignatureFolderName}", attachment.FileName);
                        signatures.Add(attachmentDTO);
                    }
                }*/

                licenseDTO.Attachments.AddRange(signatures);

                return Ok(licenseDTO);



            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/license/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicense(int id)
        {
            try
            {
                var license = await _repositoryManager.LicenseRepository.GetByIdAsync(id);
                if (license == null)
                {
                    return NotFound();
                }

                await _repositoryManager.LicenseRepository.DeleteAsync(license);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/license/count
        [HttpGet("count")]
        public async Task<IActionResult> GetLicenseCount()
        {
            try
            {
                var count = await _repositoryManager.LicenseRepository.CountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        // GET api/licenseByLicenseNumber/{licenseNumber}
        [HttpGet("licenseByLicenseNumber/{licenseNumber}")]
        public async Task<IActionResult> GetLicenseByLicenseNumber(string licenseNumber)
        {
            try
            {
                var license = await _repositoryManager.LicenseRepository.GetLicenseByLicenseNumber(licenseNumber);
                if (license == null)
                {
                    return NotFound();
                }

                var licenseDTO = _mapper.Map<ReadLicenseDTO>(license);

                /*foreach (var attachment in licenseDTO.LicenseApplication.Attachments)
                {
                    string attachmentTypeName = attachment.AttachmentType.Name;

                    string newFilePath = Path.Combine($"{Lambda.https}://{HttpContext.Request.Host}{_configuration["APISettings:API_Prefix"]}/Uploads/{Lambda.LicenseApplicationFolderName}", attachment.FileName);

                    attachment.FilePath = newFilePath;
                }*/

                return Ok(licenseDTO);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("MarkAsLicensed")]
        public async Task<IActionResult> MarkAsLicensed([FromForm] string memberIds)
        {
            try
            {
                var yearOfOperation = await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();

                if (yearOfOperation == null) throw new ArgumentException("No valid year of operation found");

                //iterate through the member ids and mark them as licensed
                var memberIdsArray = JsonConvert.DeserializeObject<string[]>(memberIds);
                foreach (var memberId in memberIdsArray)
                {
                    var member = await _repositoryManager.MemberRepository.GetMemberByUserId(memberId);
                    if (member == null) throw new ArgumentException("No valid member found");

                    // Check if the member already has a license for the current year of operation
                    var existingLicense = await _repositoryManager.LicenseRepository
                    .GetAsync(l => l.MemberId == member.Id && l.YearOfOperationId == yearOfOperation.Id);

                    if (existingLicense != null) continue; // Skip if license already exists

                    // Generate a new license number
                    var licenseNumber = await GenerateLicenseNumber(yearOfOperation);

                    // Create a new license object
                    var license = new License
                    {
                        MemberId = member.Id,
                        YearOfOperationId = yearOfOperation.Id,
                        LicenseNumber = licenseNumber,
                        ExpiryDate = yearOfOperation.EndDate,
                        Status = "Active"
                    };

                    // Add the new license to the repository and commit the transaction
                    await _repositoryManager.LicenseRepository.AddAsync(license);
                    await _unitOfWork.CommitAsync();

                    // Send email notification to the member
                    await _emailService.SendMailWithKeyVarReturn(
                        member.User.Email,
                        "License Generated",
                        $"Your license for {yearOfOperation.StartDate.Year}-{yearOfOperation.EndDate.Year} has been generated. License Number: {licenseNumber}",
                        false
                    );
                }
                return Ok("License generated successfully");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }

        }

        private async Task<string> GenerateLicenseNumber(YearOfOperation yearOfOperation)
        {
            var lastLicense = await _repositoryManager.LicenseRepository.GetLastLicenseNumber(yearOfOperation.Id);

            // If no licenses exist, start with the first license number
            if (lastLicense == null)
            {
                return $"{yearOfOperation.StartDate.Year}/{yearOfOperation.EndDate.Year}MLS0001";
            }

            // Extract the last number and increment it for the new license
            var lastNumber = int.Parse(lastLicense.LicenseNumber.Substring(lastLicense.LicenseNumber.Length - 4));
            return $"{yearOfOperation.StartDate.Year}{yearOfOperation.EndDate.Year}MLS{(lastNumber + 1).ToString("D4")}";
        }

    }
}
