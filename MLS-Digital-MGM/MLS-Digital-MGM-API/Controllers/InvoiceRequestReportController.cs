using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStore.Core.Models;
using DataStore.Core.Services;
using DataStore.Core.DTOs;
using DataStore.Persistence.Interfaces;
using AutoMapper;
using DataStore.Core.DTOs.InvoiceRequest;

[ApiController]
[Route("api/[controller]")]
public class InvoiceRequestReportController : ControllerBase
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger<InvoiceRequestReportController> _logger;

    public InvoiceRequestReportController(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILogger<InvoiceRequestReportController> logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoiceRequestReport(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        [FromQuery] string referencedEntityType = null)
    {
        try
        {
            var invoiceRequests = await _repositoryManager.InvoiceRequestRepository
                .GetInvoiceRequestsForReportAsync(startDate, endDate, referencedEntityType);
            var invoiceRequestDTOs = _mapper.Map<List<ReadInvoiceRequestDTO>>(invoiceRequests);

            // Group by date and ReferencedEntityType, and count requests
            var reportData = invoiceRequestDTOs
                .GroupBy(ir => new { ir.CreatedDate.Date, ir.ReferencedEntityType })
                .Select(g => new
                {
                    Date = g.Key.Date,
                    EntityType = g.Key.ReferencedEntityType,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.EntityType)
                .ToList();

            // Get unique entity types
            var entityTypes = reportData.Select(d => d.EntityType).Distinct().ToList();

            // Format data for ChartJS
            var chartJsData = new
            {
                labels = reportData.Select(d => d.Date.ToString("dd-MM-yyyy")).Distinct().ToArray(),
                datasets = entityTypes.Select(et => new
                {
                    label = et,
                    data = reportData.Where(d => d.EntityType == et)
                                     .Select(d => d.Count)
                                     .ToArray(),
                    backgroundColor = GetRandomColor(),
                    borderColor = GetRandomColor(0.8),
                    borderWidth = 1
                }).ToArray()
            };

            return Ok(chartJsData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetInvoiceRequestReport");
            return StatusCode(500, "An error occurred while generating the report.");
        }
    }

    private string GetRandomColor(double alpha = 0.2)
    {
        var random = new Random();
        return $"rgba({random.Next(256)}, {random.Next(256)}, {random.Next(256)}, {alpha})";
    }
}
