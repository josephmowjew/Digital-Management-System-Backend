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

namespace YourNamespaceHere.Controllers
{
    [Route("api/[controller]")]
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
       
        public async Task<IActionResult> GetUsers(int pageNumber = 1, int pageSize = 10)
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
                SortColumn = dataTableParams.LoadFromRequest(_httpContextAccessor) ? dataTableParams.SortColumn : null
            };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetPagedAsync(pagingParameters);

        
                // Check if users exist
                if (users == null || !users.Any())
                {
                    return Ok(); // Return 404 Not Found if no users are found
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

        [HttpGet("unconfirmedUsers")]
        public async Task<IActionResult> UnconfirmedUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                
                // Create PagingParameters object
                var pagingParameters = new PagingParameters<ApplicationUser>{
                    Predicate = u => u.Status != Lambda.Deleted && u.EmailConfirmed == false,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                    //SearchTerm = null
                };

                // Fetch paginated users using the UserRepository
                var users = await _repositoryManager.UserRepository.GetPagedAsync(pagingParameters);

        
                // Check if users exist
                if (users == null || !users.Any())
                {
                    return Ok(); // Return 404 Not Found if no users are found
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
                    return Ok(); // Return 404 Not Found if no users are found
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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _repositoryManager.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _mapper.Map(userDTO, user);
                await _repositoryManager.UserRepository.UpdateAsync(user);
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
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                await _repositoryManager.UserRepository.DeleteAsync(user);
                await _unitOfWork.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.LogErrorAsync(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("activate/{userId}")]
        public async Task<IActionResult> ActivateAccount(string userId)
        {
            try
            {
                var user = await _repositoryManager.UserRepository.GetSingleUser(userId);
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

    }
}