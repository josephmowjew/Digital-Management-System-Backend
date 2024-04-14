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

namespace MLS_Digital_MGM_API.Controllers // Update with your actual namespace
{
    [Route("api/[controller]")]
    public class ProBonoClientsController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public ProBonoClientsController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetProBonoClients(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var clients = await _repositoryManager.ProBonoClientRepository.GetPagedAsync(c => true, pageNumber, pageSize);

                if (clients == null || !clients.Any())
                {
                    return NotFound();
                }

                var mappedClients = _mapper.Map<IEnumerable<ReadProBonoClientDTO>>(clients);

                return Ok(mappedClients);
            }
            catch (Exception ex)
            {
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
    }
}