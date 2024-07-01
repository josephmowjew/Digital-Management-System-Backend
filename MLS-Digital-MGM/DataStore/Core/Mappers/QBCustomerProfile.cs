using AutoMapper;
using DataStore.Core.DTOs.QBCustomerDTOs;
using DataStore.Core.DTOs.QBInvoicesDTOs;
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Mappers
{
    public class QBCustomerProfile : Profile
    {
        public QBCustomerProfile()
        {
            CreateMap<QBCustomer, ReadQBCustomerDTO>();
        }
    }
}
