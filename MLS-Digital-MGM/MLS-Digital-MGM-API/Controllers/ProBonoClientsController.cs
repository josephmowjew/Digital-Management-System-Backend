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
using DataStore.Core.DTOs.ProBonoClient;
using DataStore.Helpers;
using MLS_Digital_MGM.DataStore.Helpers;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MLS_Digital_MGM_API.Controllers // Update with your actual namespace
{
    [Route("api/[controller]")]
    public class ProBonoClientsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public ProBonoClientsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetProBonoClients(int pageNumber = 1, int pageSize = 10)
        {
            try
            {

                // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();
            
                var pagingParameters = new PagingParameters<ProbonoClient>
                {
                    Predicate = u => u.Status != Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                };

                var clients = await _repositoryManager.ProBonoClientRepository.GetPagedAsync(pagingParameters);

                if (clients == null || !clients.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadProBonoClientDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadProBonoClientDTO>()); // Return empty list
                }

                var mappedClients = _mapper.Map<List<ReadProBonoClientDTO>>(clients);
                 // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedClients.Count;

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = resultTotalFiltred, 
                        recordsTotal = resultTotalFiltred, 
                        data = mappedClients.ToList() // Materialize the enumerable
                    });
                }


                return Ok(mappedClients);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

         [HttpGet("getAll")]
        public async Task<IActionResult> GetAllProbonoClients()
        {
            try
            {

                // Get paged list of identity types from repository
                var probonoClients = await _repositoryManager.ProBonoClientRepository.GetAllAsync();

                // If no identity types found, return NotFound result
                if (probonoClients == null || !probonoClients.Any())
                {
                    return NotFound();
                }

                // Map pro bono clients types to DTOs and return as Ok result
                var mappedProbonoClients = _mapper.Map<IEnumerable<ReadProBonoClientDTO>>(probonoClients);

                return Ok(mappedProbonoClients);
            }
            catch (Exception ex)
            {
                // Log error and return Internal Server Error result
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddProBonoClient([FromBody] CreateProBonoClientDTO clientDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var client = _mapper.Map<ProbonoClient>(clientDTO);

                var existingClient = await _repositoryManager.ProBonoClientRepository.GetAsync(c => c.Name.Trim().Equals(client.Name.Trim(), StringComparison.OrdinalIgnoreCase));
                if (existingClient != null)
                {
                    ModelState.AddModelError(nameof(clientDTO.Name), "A client with the same name already exists");
                    return BadRequest(ModelState);
                }

                await _repositoryManager.ProBonoClientRepository.AddAsync(client);
                await _unitOfWork.CommitAsync();

                return CreatedAtAction("GetProBonoClients", new { id = client.Id }, client);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProBonoClient(int id, [FromBody] UpdateProBonoClientDTO clientDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var client = await _repositoryManager.ProBonoClientRepository.GetByIdAsync(id);
                if (client == null)
                {
                    return NotFound();
                }

                _mapper.Map(clientDTO, client);
                await _repositoryManager.ProBonoClientRepository.UpdateAsync(client);
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
        public async Task<IActionResult> DeleteProBonoClient(int id)
        {
            try
            {
                var client = await _repositoryManager.ProBonoClientRepository.GetByIdAsync(id);
                if (client == null)
                {
                    return NotFound();
                }

                await _repositoryManager.ProBonoClientRepository.DeleteAsync(client);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getclient/{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
             try
            {
                // Fetch  clients using the UserRepository
                var client = await _repositoryManager.ProBonoClientRepository.GetByIdAsync(id);

                if(client != null)
                {
                    var mappedData = _mapper.Map<ReadProBonoClientDTO>(client);
                    return Ok(mappedData);
                }
                return BadRequest("user not found");

            }
            catch (Exception ex)
            {

                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);

                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("custom_select")]
        public async Task<JsonResult> GetClients(int page = 1, int pageSize = 20, string searchValue = "")
        {

            
             var pagingParameters = new PagingParameters<ProbonoClient>
            {
                Predicate = u => u.Status != Lambda.Deleted,
                PageNumber = page,
                PageSize =  pageSize,
                SearchTerm = searchValue,
               
            };

            var clients = await _repositoryManager.ProBonoClientRepository.GetPagedAsync(pagingParameters);


            

            List<DynamicSelect> dynamicSelect = new List<DynamicSelect>();

            if (clients.Any())
            {
                foreach (var item in clients)
                {
                    dynamicSelect.Add(new DynamicSelect { Id = item.Id.ToString(), Name = item.Name + " (" + item.NationalId +
                    ")" + " -- " + item.Occupation,
                        
                    });
                }
            }



            return Json(dynamicSelect);
        }
    
    }
}