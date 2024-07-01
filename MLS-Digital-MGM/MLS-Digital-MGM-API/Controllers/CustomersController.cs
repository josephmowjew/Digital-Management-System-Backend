using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
//[Authorize(AuthenticationSchemes = "Bearer")]
public class CustomersController : Controller
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IErrorLogService _errorLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomersController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _repositoryManager = repositoryManager;
        _errorLogService = errorLogService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<JsonResult> GetAllCustomersJson(int page = 1, int pageSize = 20, string searchValue = "")
    {
        try
        {
            var skip = (page - 1) * pageSize;

            var customers = await _repositoryManager.QBCustomerRepository.GetAllCustomersAsync(new CursorParams
            {
                Take = pageSize,
                Skip = skip,
                SearchTerm = searchValue
            });

            var dynamicSelect = new List<DynamicSelect>();

            if (customers.Any())
            {
                dynamicSelect.AddRange(customers.Select(item => new DynamicSelect
                {
                    Id = item.Id.ToString(),
                    Name = $"{item.CustomerName} ({item.CompanyName}) -- {item.EmailAddress}"
                }));
            }

            return Json(dynamicSelect);
        }
        catch (Exception ex)
        {
            await _errorLogService.LogErrorAsync(ex);
            return Json(new { error = "Internal server error" });
        }
    }
}
