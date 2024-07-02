using AutoMapper;
using DataStore.Core.DTOs.Firms;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class FirmsController : Controller
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IErrorLogService _errorLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _emailService;


    public FirmsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
    {
        _repositoryManager = repositoryManager;
        _errorLogService = errorLogService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetFirms(int pageNumber = 1, int pageSize = 10)
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

            var pagingParameters = new PagingParameters<Firm>
            {
                Predicate = u => u.Status != Lambda.Deleted && (string.Equals(currentRole, "administrator", StringComparison.OrdinalIgnoreCase) || string.Equals(currentRole, "Finance Officer", StringComparison.OrdinalIgnoreCase) || u.CreatedById == user.Id),
                PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
            };

            // Fetch paginated firms using the FirmRepository
            var pagedFirms = await _repositoryManager.FirmRepository.GetPagedAsync(pagingParameters);

            // Check if roles exist
            if (pagedFirms == null || !pagedFirms.Any())
            {
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    return Json(new
                    {
                        draw,
                        recordsFiltered = 0,
                        recordsTotal = 0,
                        data = Enumerable.Empty<ReadFirmDTO>()
                    });
                }
                return Ok(Enumerable.Empty<ReadFirmDTO>()); // Return empty list
            }

            // Map the Roles to a list of ReadFirmDTOs
            var mappedFirms = _mapper.Map<List<ReadFirmDTO>>(pagedFirms);

            // Return datatable JSON if the request came from a datatable
            if (dataTableParams.LoadFromRequest(_httpContextAccessor))
            {
                var draw = dataTableParams.Draw;
                var resultTotalFiltred = mappedFirms.Count;
                var totalRecords = await _repositoryManager.FirmRepository.CountAsync(pagingParameters);


                return Json(new
                {
                    draw,
                    recordsFiltered = totalRecords,
                    recordsTotal = totalRecords,
                    data = mappedFirms.ToList() // Materialize the enumerable
                });
            }


            // Return an Ok result with the mapped Roles
            return Ok(mappedFirms);

        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllFirms()
    {
        try
        {

            // Get paged list of firms from repository
            var firms = await _repositoryManager.FirmRepository.GetAllAsync(p => p.Status == Lambda.Active);

            // If no firms found, return NotFound result
            if (firms == null || !firms.Any())
            {
                return NotFound();
            }

            // Map firms to DTOs and return as Ok result
            var mappedFirms= _mapper.Map<IEnumerable<ReadFirmDTO>>(firms);

            return Ok(mappedFirms);
        }
        catch (Exception ex)
        {
            // Log error and return Internal Server Error result
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddFirm([FromBody] CreateFirmDTO firmDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             string username = _httpContextAccessor.HttpContext.User.Identity.Name;


            var firm = _mapper.Map<Firm>(firmDTO);

            
            var existingFirm = await _repositoryManager.FirmRepository.GetAsync(f => f.Name.Trim().Equals(firm.Name.Trim(), StringComparison.OrdinalIgnoreCase));
            if (existingFirm != null)
            {
                ModelState.AddModelError(nameof(firmDTO.Name), "A firm with the same name already exists");
                return BadRequest(ModelState);
            }

             //get user id from username
            var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
            firm.CreatedById = user.Id;

             string currentRole  = Lambda.GetCurrentUserRole(_repositoryManager,user.Id);


            // Check if the user is secretariat and approve the application if so
            if (!string.Equals(currentRole, "administrator", StringComparison.OrdinalIgnoreCase))
            {
                firm.Status = Lambda.Pending;
            }
            

            await _repositoryManager.FirmRepository.AddAsync(firm);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction("GetFirms", new { id = firm.Id }, firm);
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFirm(int id, [FromBody] UpdateFirmDTO firmDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firm = await _repositoryManager.FirmRepository.GetByIdAsync(id);
            if (firm == null)
            {
                return NotFound();
            }

            _mapper.Map(firmDTO, firm);
            await _repositoryManager.FirmRepository.UpdateAsync(firm);
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
    public async Task<IActionResult> DeleteFirm(int id)
    {
        try
        {
            var firm = await _repositoryManager.FirmRepository.GetByIdAsync(id);
            if (firm == null)
            {
                return NotFound();
            }

            await _repositoryManager.FirmRepository.DeleteAsync(firm);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("getfirm/{id}")]
    public async Task<IActionResult> GetFirmById(int id)
    {
        try
        {
            var firm = await _repositoryManager.FirmRepository.GetByIdAsync(id);
            if (firm == null)
            {
                return NotFound();
            }
            var firmDTO = _mapper.Map<ReadFirmDTO>(firm);
            return Ok(firmDTO);
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpGet("activate/{id}")]
    public async Task<IActionResult> Activate(int id)
    {
        try
        {
            // Fetch  firm using the UserRepository
            var firm = await _repositoryManager.FirmRepository.GetByIdAsync(id);

            if(firm != null)
            {
                firm.Status = Lambda.Active; 
               
                await _repositoryManager.FirmRepository.UpdateAsync(firm);
                await _unitOfWork.CommitAsync();


                if(string.IsNullOrEmpty(firm.CreatedById))
                {
                    //get user id from username
                    var user = await _repositoryManager.UserRepository.GetSingleUser(firm.CreatedById);
                    
                    // Send status details email
                    string emailBody = $"The firm that you added {firm.Name} has been approved. Thank you for choosing us.";
                    var passwordEmailResult = await _emailService.SendMailWithKeyVarReturn(user.Email, "Firm Application Status", emailBody);
                }

              
                return Ok();
            }
            return BadRequest("firm not found");

        }
        catch (Exception ex)
        {

            // Log the exception using ErrorLogService
            await _errorLogService.LogErrorAsync(ex);

            // Return 500 Internal Server Error
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpPost("deny")]
    public async Task<IActionResult> DenyFirmApplication(DenyFirmApplicationDTO denyFirmDTO)
        {
            try
            {
                var firm = await _repositoryManager.FirmRepository.GetByIdAsync(denyFirmDTO.FirmId);
                firm.Status = Lambda.Denied;
                firm.DenialReason = denyFirmDTO.Reason;

                await _repositoryManager.FirmRepository.UpdateAsync(firm);
                await _unitOfWork.CommitAsync();

                //send email to the user who created the probono application

                if(string.IsNullOrEmpty(firm.CreatedById))
                {
                    //get user id from username
                    var user = await _repositoryManager.UserRepository.GetSingleUser(firm.CreatedById);
                    // Send status details email
                    string emailBody = $"Your application for the firm {firm.Name} has been denied. <br/> Reason: {denyFirmDTO.Reason}";
                    var passwordEmailResult = await _emailService.SendMailWithKeyVarReturn(user.Email, "Firm Application Status", emailBody);
                }

             
                


                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

    [HttpGet("count")]
        public async Task<IActionResult> count()
        {
            try
            {
                var count = await _repositoryManager.FirmRepository.GetFirmsCountAsync();

                return Ok(count);
            }
            catch (Exception ex)
            {

                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
}