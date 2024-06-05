using Microsoft.AspNetCore.Mvc;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataStore.Core.DTOs.User;
using DataStore.Helpers;
using System.Linq.Expressions;
using MLS_Digital_MGM.DataStore.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class UsersController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetActiveUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
            
                var pagingParameters = new PagingParameters<ApplicationUser>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.EmailConfirmed == true,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetPagedAsync(pagingParameters);

            
               // Check if users exist
                if (users == null || !users.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadUserDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadUserDTO>()); // Return empty list
                }
        
                // Map User entities to ReadUserDTOs
                var mappedUsers = _mapper.Map<IEnumerable<ReadUserDTO>>(users);

                //get the user role of the user
                var usersWithRoles = new List<ReadUserDTO>();

                mappedUsers.ToList().ForEach(user =>
                {
                   var userRole =  this._repositoryManager.UserRepository.GetUserRoleByUserId(user.Id);
                   string roleName = this._repositoryManager.UserRepository.GetRoleName(userRole.RoleId);
                   user.RoleName = FormatRoleName(roleName);
                   usersWithRoles.Add(user);
                
                });

               
                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = usersWithRoles.Count;

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = resultTotalFiltred, 
                        recordsTotal = resultTotalFiltred, 
                        data = usersWithRoles.ToList() // Materialize the enumerable
                    });
                }

                return Ok(mappedUsers); // Return paginated users
        
            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);
        
                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("suspended")]
        public async Task<IActionResult> GetSuspendedUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
            
                var pagingParameters = new PagingParameters<ApplicationUser>
                {
                    Predicate = u => u.Status == Lambda.Deleted,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetPagedAsync(pagingParameters);

            
               // Check if users exist
                if (users == null || !users.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadUserDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadUserDTO>()); // Return empty list
                }
        
                // Map User entities to ReadUserDTOs
                var mappedUsers = _mapper.Map<IEnumerable<ReadUserDTO>>(users);

                //get the user role of the user
                var usersWithRoles = new List<ReadUserDTO>();

                mappedUsers.ToList().ForEach(user =>
                {
                   var userRole =  this._repositoryManager.UserRepository.GetUserRoleByUserId(user.Id);
                   string roleName = this._repositoryManager.UserRepository.GetRoleName(userRole.RoleId);
                   user.RoleName = FormatRoleName(roleName);
                   usersWithRoles.Add(user);
                
                });

               
                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = usersWithRoles.Count;

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = resultTotalFiltred, 
                        recordsTotal = resultTotalFiltred, 
                        data = usersWithRoles.ToList() // Materialize the enumerable
                    });
                }

                return Ok(mappedUsers); // Return paginated users
        
            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);
        
                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("unconfirmedUsers")]
        public async Task<IActionResult> UnconfirmedUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var dataTableParams = new DataTablesParameters();
            
                var pagingParameters = new PagingParameters<ApplicationUser>
                {
                    Predicate = u => u.Status != Lambda.Deleted && u.EmailConfirmed == false,
                    PageNumber = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageNumber : pageNumber,
                    PageSize = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.PageSize : pageSize,
                    SearchTerm = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SearchValue : null,
                    SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null,
                    SortDirection = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumnAscDesc : null
                };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetPagedAsync(pagingParameters);

            
               // Check if users exist
                if (users == null || !users.Any())
                {
                    if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                    {
                        var draw = dataTableParams.Draw;
                        return Json(new 
                        { 
                            draw, 
                            recordsFiltered = 0, 
                            recordsTotal = 0, 
                            data = Enumerable.Empty<ReadUserDTO>()
                        });
                    }
                    return Ok(Enumerable.Empty<ReadUserDTO>()); // Return empty list
                }
        
                // Map User entities to ReadUserDTOs
                var mappedUsers = _mapper.Map<IEnumerable<ReadUserDTO>>(users);

                //get the user role of the user
                var usersWithRoles = new List<ReadUserDTO>();

                mappedUsers.ToList().ForEach(user =>
                {
                   var userRole =  this._repositoryManager.UserRepository.GetUserRoleByUserId(user.Id);
                   string roleName = this._repositoryManager.UserRepository.GetRoleName(userRole.RoleId);
                   user.RoleName = FormatRoleName(roleName);
                   usersWithRoles.Add(user);
                
                });

               
                // Return datatable JSON if the request came from a datatable
                if (dataTableParams.LoadFromRequest(_httpContextAccessor))
                {
                    var draw = dataTableParams.Draw;
                    var resultTotalFiltred = usersWithRoles.Count;

                    return Json(new 
                    { 
                        draw, 
                        recordsFiltered = resultTotalFiltred, 
                        recordsTotal = resultTotalFiltred, 
                        data = usersWithRoles.ToList() // Materialize the enumerable
                    });
                }

                return Ok(mappedUsers); // Return paginated users
        
            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);
        
                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("getDeletedUsers")]
        public async Task<IActionResult> DeletedUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                
                // Create PagingParameters object
                var pagingParameters = new PagingParameters<ApplicationUser>{
                    Predicate = u => u.Status == Lambda.Deleted,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                    //SearchTerm = null
                };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetPagedAsync(pagingParameters);

        
                // Check if users exist
                if (users == null || !users.Any())
                {
                    return Ok(Enumerable.Empty<ReadUserDTO>()); 
                }
        
                // Map User entities to ReadUserDTOs
                var mappedUsers = _mapper.Map<IEnumerable<ReadUserDTO>>(users);

                //get the user role of the user

               
                return Ok(mappedUsers); // Return paginated users
        
            }
            catch (Exception ex)
            {
                // Log the exception using ErrorLogService
                await _errorLogService.LogErrorAsync(ex);
        
                // Return 500 Internal Server Error
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                // Fetch paginated users using the UserRepository
                var user = await _repositoryManager.UserRepository.GetSingleUser(id);

                if(user != null)
                {
                    var mappedData = _mapper.Map<ReadUserDTO>(user);

                    var userRole = this._repositoryManager.UserRepository.GetUserRoleByUserId(user.Id);
                    string roleName = this._repositoryManager.UserRepository.GetRoleName(userRole.RoleId);

                    //update role name

                    mappedData.RoleName = roleName;
                    return Ok(_mapper.Map<ReadUserDTO>(mappedData));
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

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody]UpdateUserDTO userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _repositoryManager.UserRepository.GetSingleUser(id);
                if (user == null)
                {
                    return NotFound();
                }

                if(userDTO.DepartmentId == 0 || userDTO.DepartmentId == null)
                {
                    //get the department of the user from the database
                    userDTO.DepartmentId = user.DepartmentId;
                }
                _mapper.Map(userDTO, user);
                await _repositoryManager.UserRepository.UpdateAsync(user);

                if(userDTO.RoleName != null)
                {
                    var roleResult = await _repositoryManager.UserRepository.AddUserToRoleAsync(user, userDTO.RoleName);

                      if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("Role", "Failed to associate the user with the specified role.");
                        return BadRequest(ModelState);
                    }
                    
                }


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
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetSingleUser(id);
                if (user == null)
                {
                    return NotFound();
                }

                await _repositoryManager.UserRepository.DeleteAsync(user);
                await _unitOfWork.CommitAsync();

                return Ok(_mapper.Map<ReadUserDTO>(user));
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("activate/{userId}")]
        public async Task<IActionResult> activate(string userId)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetSingleUserNoFilter(userId);
                if (user == null)
                {
                    return NotFound();
                }
               
                _repositoryManager.UserRepository.ActivateAccount(user);
                await _repositoryManager.UserRepository.UpdateAsync(user);
                await _unitOfWork.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

       private string FormatRoleName(string roleName)
        {
            var firstChar = char.ToUpper(roleName[0]);
            var restOfName = new string(roleName.Skip(1).SelectMany(c => char.IsUpper(c) ? new[] { ' ', c } : new[] { c }).ToArray());
            return firstChar + restOfName;
        }
    }
}