using System;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Data;

namespace DataStore.Core.Services
{
    public class EntityResolverService: IEntityResolverService
    {
        private readonly ApplicationDbContext _context;

        public EntityResolverService(ApplicationDbContext context)
        {
            _context = context;
        }

        public object GetReferencedEntity(InvoiceRequest invoiceRequest)
        {
            switch (invoiceRequest.ReferencedEntityType)
            {
                case "CPD":
                    return _context.CPDTrainings.Find(invoiceRequest.ReferencedEntityId);
                case "LicenceApplication":
                    return _context.LicenseApplications.Find(invoiceRequest.ReferencedEntityId);
                // Add more cases as needed
                default:
                    throw new InvalidOperationException("Unknown entity type");
            }
        }
    }
}