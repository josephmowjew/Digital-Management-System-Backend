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
using DataStore.Core.DTOs.ProBono;


namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    public class ProBonosController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
    
        public ProBonosController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    
        [HttpGet("paged")]
        public async Task<IActionResult> GetProBonos(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var probonos = await _repositoryManager.ProBonoRepository.GetPagedAsync(c => true, pageNumber, pageSize);
    
                if (probonos == null || !probonos.Any())
                {
                    return NotFound();
                }
    
                var mappedProbonos = _mapper.Map<IEnumerable<ReadProBonoDTO>>(probonos);
    
                return Ok(mappedProbonos);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    
        [HttpPost]
        public async Task<IActionResult> AddProBono([FromBody] CreateProBonoDTO probonoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                 if (probonoDTO.ProBonoApplicationId < 1)
                {
                    ModelState.AddModelError(nameof(probonoDTO.ProBonoApplicationId), "invalid application Id kindly check the value and resubmit");
                    return BadRequest(ModelState);
                }

                
                //check if there is no probono associated with the CreateProBonoDTO.ProBonoApplicationId
                var existingProbono = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(probonoDTO.ProBonoApplicationId);
                if (existingProbono != null)
                {
                    ModelState.AddModelError(nameof(probonoDTO.ProBonoApplicationId), "a probono with this application Id already exists");
                    return BadRequest(ModelState);
                }

                //get proBonoApplication from CreateProBonoDTO.proBonoApplicationId
                var probonoApplication = await _repositoryManager.ProBonoApplicationRepository.GetByIdAsync(probonoDTO.ProBonoApplicationId);
                if (probonoApplication == null)
                {
                    return NotFound();
                }

                var probono = _mapper.Map<ProBono>(probonoApplication);
               
                string fileNumber = await GenerateUniqueFileNumber();

                probono.FileNumber = fileNumber;
    
                await _repositoryManager.ProBonoRepository.AddAsync(probono);
                await _unitOfWork.CommitAsync();
    
                return CreatedAtAction("GetProBonos", new { id = probono.Id }, probono);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProBono(int id, [FromBody] UpdateProBonoDTO probonoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
    
                var probono = await _repositoryManager.ProBonoRepository.GetByIdAsync(id);
                if (probono == null)
                {
                    return NotFound();
                }
    
                _mapper.Map(probonoDTO, probono);
                await _repositoryManager.ProBonoRepository.UpdateAsync(probono);
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
        public async Task<IActionResult> DeleteProBono(int id)
        {
            try
            {
                var probono = await _repositoryManager.ProBonoRepository.GetByIdAsync(id);
                if (probono == null)
                {
                    return NotFound();
                } 
    
                await _repositoryManager.ProBonoRepository.DeleteAsync(probono);
                await _unitOfWork.CommitAsync();
    
                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        //create a function to generate a unique file number
        private async Task<string> GenerateUniqueFileNumber()
        {
            var existingFileNumbers = await _repositoryManager.ProBonoRepository.GetAllAsync();
            existingFileNumbers = existingFileNumbers.ToList();
            string fileNumber;
            do
            {
                var random = new Random();
                var datePart = DateTime.Now.ToString("yyMMdd");
                var randomPart = random.Next(1000, 10000).ToString("D4");
                fileNumber = $"{datePart}{randomPart}";
            } while (existingFileNumbers.Any(c => c.FileNumber == fileNumber));

            return fileNumber;
        }

    }
    

}