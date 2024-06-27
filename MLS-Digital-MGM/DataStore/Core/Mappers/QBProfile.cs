using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.ViewModels;

public class QBProfile : Profile
{
    public QBProfile()
    {
        CreateMap<QBInvoiceViewModel, QBInvoice>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InvoiceNumber))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(src => src.InvoiceDate))
            .ForMember(dest => dest.InvoiceAmount, opt => opt.MapFrom(src => src.InvoiceAmount))
            .ForMember(dest => dest.UnpaidAmount, opt => opt.MapFrom(src => src.UnpaidAmount))
            .ForMember(dest => dest.InvoiceType, opt => opt.MapFrom(src => src.InvoiceType))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate))
            .ForMember(dest => dest.InvoiceDescription, opt => opt.MapFrom(src => src.InvoiceDescription));

        CreateMap<QBReceiptViewModel, QBReceipt>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
            .ForMember(dest => dest.ReceiptNumber, opt => opt.MapFrom(src => src.ReceiptNumber))
            .ForMember(dest => dest.TotalPaymentAmount, opt => opt.MapFrom(src => src.TotalPaymentAmount))
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
            .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId))
            .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(src => src.InvoiceNumber))
            .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(src => src.InvoiceDate))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate));

        CreateMap<QBCustomerViewModel, QBCustomer>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobTitle))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.BillingAddressLine1, opt => opt.MapFrom(src => src.BillingAddressLine1))
            .ForMember(dest => dest.BillingAddressLine2, opt => opt.MapFrom(src => src.BillingAddressLine2))
            .ForMember(dest => dest.BillingAddressLine3, opt => opt.MapFrom(src => src.BillingAddressLine3))
            .ForMember(dest => dest.BillingAddressLine4, opt => opt.MapFrom(src => src.BillingAddressLine4))
            .ForMember(dest => dest.BillingAddressLine5, opt => opt.MapFrom(src => src.BillingAddressLine5))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
            .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
            .ForMember(dest => dest.ActiveStatus, opt => opt.MapFrom(src => src.ActiveStatus))
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.AccountBalance, opt => opt.MapFrom(src => src.AccountBalance))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate));

        CreateMap<QBPaymentViewModel, QBPayment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
            .ForMember(dest => dest.PaymentAmount, opt => opt.MapFrom(src => src.PaymentAmount))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate));


    }
}
