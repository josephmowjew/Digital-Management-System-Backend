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
using DataStore.Helpers;
using System.Linq.Expressions;
using MLS_Digital_MGM.DataStore.Helpers;
using Microsoft.AspNetCore.Authorization;
using StackExchange.Redis;


namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProBonosController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProBonosController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetProBonos(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                string CreatedById = user.Id;


                string currentRole = Lambda.GetCurrentUserRole(_repositoryManager, user.Id);
                // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<ProBono>
                {
                    Predicate = u => u.Status != Lambda.Deleted &&
                                    ((string.Equals(currentRole, "secretariat", StringComparison.OrdinalIgnoreCase) ||
                                      string.Equals(currentRole, "ceo", StringComparison.OrdinalIgnoreCase)) ||
                                     u.CreatedById == CreatedById ||
                                     u.Members.Any(m => m.UserId == CreatedById)),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<ProBono, object>>[]{
                        p => p.YearOfOperation,
                        p => p.ProBonoApplication,
                        p => p.ProbonoClient,
                        p => p.ProBonoReports,
                        p => p.Members,
                    }
                };


                var probonos = await _repositoryManager.ProBonoRepository.GetPagedAsync(pagingParameters);
                if (probonos == null || !probonos.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadProBonoDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadProBonoDTO>()); // Return empty list
                }

                var mappedProbonos = _mapper.Map<List<ReadProBonoDTO>>(probonos);

                mappedProbonos.ForEach(p =>
                {
                    //add all probono hours in each probono
                    double hours = p.ProBonoReports.Sum(pr => pr.ProBonoHours);

                    p.ProBonoHoursAccoumulated = hours;
                });

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedProbonos.Count;
                    var totalRecords = await _repositoryManager.ProBonoRepository.CountAsync(pagingParameters);


                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = mappedProbonos.ToList() // Materialize the enumerable
                    });
                }
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
                var existingProbono = await _repositoryManager.ProBonoRepository.GetByIdAsync(probonoDTO.ProBonoApplicationId);
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

                probono.ProBonoApplicationId = probonoDTO.ProBonoApplicationId;

                string fileNumber = await GenerateUniqueFileNumber();

                probono.FileNumber = fileNumber;

                await _repositoryManager.ProBonoRepository.AddAsync(probono);
                //approve the probono application automatically
                probonoApplication.ApplicationStatus = Lambda.Approved;

                probonoApplication.ApprovedDate = DateTime.UtcNow;

                await _repositoryManager.ProBonoApplicationRepository.UpdateAsync(probonoApplication);

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

        [HttpGet("count")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var count = await _repositoryManager.ProBonoRepository.GetProBonosCountAsync();

                return Ok(count);
            }
            catch (Exception ex)
            {

                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getFreeCases")]
        public async Task<IActionResult> GetCases(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get user id from username
                //var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);
                //string CreatedById = user.Id;

                // Get users with the 'secretariat' role
                var secretariatUsers = await _repositoryManager.UserRepository.GetUsersByRoleAsync("secretariat");

                // Get the IDs of secretariat users
                var secretariatUserIds = secretariatUsers.Select(u => u.Id).ToList();
                // Create a new DataTablesParameters object
                var dataTableParams = new DataTablesParameters();

                var pagingParameters = new PagingParameters<ProBono>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.Members.Count == 0 && secretariatUserIds.Contains(u.CreatedById),
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null,
                    Includes = new Expression<Func<ProBono, object>>[]{
                        p => p.YearOfOperation,
                        p => p.ProBonoApplication,
                        p => p.ProbonoClient,
                        p => p.ProBonoReports,
                        p => p.Members,
                    },
                    // No need for CreatedById filter as it's already handled in the predicate
                };


                var probonos = await _repositoryManager.ProBonoRepository.GetPagedAsync(pagingParameters);
                if (probonos == null || !probonos.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new
                        {
                            draw,
                            recordsFiltered = 0,
                            recordsTotal = 0,
                            data = Enumerable.Empty<ReadProBonoDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadProBonoDTO>()); // Return empty list
                }

                var mappedProbonos = _mapper.Map<List<ReadProBonoDTO>>(probonos);

                mappedProbonos.ForEach(p =>
                {
                    //add all probono hours in each probono
                    double hours = p.ProBonoReports.Sum(pr => pr.ProBonoHours);

                    p.ProBonoHoursAccoumulated = hours;
                });

                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = mappedProbonos.Count;
                    var totalRecords = await _repositoryManager.ProBonoRepository.CountAsync(pagingParameters);


                    return Json(new
                    {
                        draw,
                        recordsFiltered = totalRecords,
                        recordsTotal = totalRecords,
                        data = mappedProbonos.ToList() // Materialize the enumerable
                    });
                }


                return Ok(mappedProbonos);
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("applyForCase/{id}")]
        public async Task<IActionResult> ApplyForCase(int id)
        {
            try
            {
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;

                var user = await _repositoryManager.UserRepository.FindByEmailAsync(username);

                var probono = await _repositoryManager.ProBonoRepository.GetByIdAsync(id);
                if (probono == null)
                {
                    return NotFound();
                }
                else
                {
                    //assign the probono to member
                    var member = await _repositoryManager.MemberRepository.GetMemberByUserId(user.Id);
                    if (member == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        probono.Members.Add(member);
                        await _repositoryManager.ProBonoRepository.UpdateAsync(probono);
                        await _unitOfWork.CommitAsync();
                        return Ok();
                    }
                }
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }

}