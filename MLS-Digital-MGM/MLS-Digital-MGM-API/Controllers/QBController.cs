using Microsoft.AspNetCore.Mvc;
using DataStore.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataStore.Persistence.Interfaces;
using DataStore.Core.Services.Interfaces;
using DataStore.Core.Models;
using System.Linq;
using DataStore.Helpers;

namespace MLS_Digital_MGM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QBController : ControllerBase
    {
        // Repositories
        private readonly IRepositoryManager _repositoryManager;
        private readonly IErrorLogService _errorLogService;
        private readonly IUnitOfWork _unitOfWork;

        // Services
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public QBController(IRepositoryManager repositoryManager, IErrorLogService errorLogService, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _errorLogService = errorLogService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        // Create Invoices
        [HttpPost("invoices")]
        public async Task<ActionResult> CreateInvoices([FromBody] List<QBInvoiceViewModel> invoiceViewModels)
        {
            // Validate model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Map the data
            var invoices = _mapper.Map<List<QBInvoice>>(invoiceViewModels);

            // Filter out existing invoices
            var existingInvoices = await _repositoryManager.InvoiceRepository.GetAllAsync();
            invoices = invoices.Where(i => !existingInvoices.Any(ei => ei.Id == i.Id)).ToList();

            //filter the invoices based on the Id removing duplicate
            invoices = invoices.GroupBy(i => i.Id).Select(g => g.First()).ToList();


            // Add invoices to the database
            _repositoryManager.InvoiceRepository.AddRange(invoices);
            await _unitOfWork.CommitAsync();

            foreach (var invoice in invoiceViewModels)
            {
               //extract the invoice request id from the description

                int invoiceRequestId =  Lambda.ExtractInvoiceRequestId(invoice.InvoiceDescription);

                if(invoiceRequestId!= 0)
                {
                    //find the invoice request with the id that was submited 
                    var invoiceRequest = await _repositoryManager.InvoiceRequestRepository.GetByIdAsync(invoiceRequestId);

                    if(invoiceRequest != null)
                    {
                        //update the invoice request with the invoice id
                        invoiceRequest.QBInvoiceId = invoice.Id;
                        _repositoryManager.InvoiceRequestRepository.UpdateAsync(invoiceRequest);
                        //save the changes
                        await _unitOfWork.CommitAsync();
                    }
                }
            }

            return Ok();
        }

        // Create Payments
        [HttpPost("payments")]
        public async Task<ActionResult> CreatePayments([FromBody] List<QBPaymentViewModel> paymentViewModels)
        {
            // Validate model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Map the data
            var payments = _mapper.Map<List<QBPayment>>(paymentViewModels);

            // Filter out existing payments
            var existingPayments = await _repositoryManager.PaymentRepository.GetAllAsync();
            payments = payments.Where(p => !existingPayments.Any(ep => ep.Id == p.Id)).ToList();

            //filter the payments based on the Id removing duplicate
            payments = payments.GroupBy(p => p.Id).Select(g => g.First()).ToList();
            // Add payments to the database
            _repositoryManager.PaymentRepository.AddRange(payments);
            await _unitOfWork.CommitAsync();
            return Ok();
        }

        // Create Receipts
        [HttpPost("receipts")]
        public async Task<ActionResult> CreateReceipts([FromBody] List<QBReceiptViewModel> receiptViewModels)
        {
            // Validate model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Map the data
            var receipts = _mapper.Map<List<QBReceipt>>(receiptViewModels);

            // Filter out existing receipts
            var existingReceipts = await _repositoryManager.ReceiptRepository.GetAllAsync();
            receipts = receipts.Where(r => !existingReceipts.Any(er => er.Id == r.Id)).ToList();
            //filter the receipts based on the Id removing duplicate
            receipts = receipts.GroupBy(r => r.Id).Select(g => g.First()).ToList();
            // Add receipts to the database
            _repositoryManager.ReceiptRepository.AddRange(receipts);
            await _unitOfWork.CommitAsync();
            return Ok();
        }

        // Create Customers
        [HttpPost("customers")]
        public async Task<ActionResult> CreateCustomers([FromBody] List<QBCustomerViewModel> customerViewModels)
        {
            // Validate model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Map the data
            var customers = _mapper.Map<List<QBCustomer>>(customerViewModels);

            // Filter out existing customers
            var existingCustomers = await _repositoryManager.CustomerRepository.GetAllAsync();
            customers = customers.Where(c => !existingCustomers.Any(ec => ec.Id == c.Id)).ToList();

            // Add customers to the database
            _repositoryManager.CustomerRepository.AddRange(customers);
            await _unitOfWork.CommitAsync();
            return Ok();
        }
    }
}