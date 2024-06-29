using System;
using System.Threading.Tasks;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Data;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Core.Services
{
    public class EntityResolverService : IEntityResolverService
    {
        private readonly ApplicationDbContext _context;

        public EntityResolverService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Original method
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

        
        public object ResolveEntityFromDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return null;
            }

            string[] parts = description.Split(' ');
            if (parts.Length < 5)
            {
                return null;
            }

            string entityType = parts[^2]; // Second to last part
            if (!int.TryParse(parts[^1], out int entityId))
            {
                return null;
            }

            return ResolveEntity(entityType, entityId);
        }

       
        private object ResolveEntity(string entityType, int entityId)
        {
            switch (entityType)
            {
                case "CPD":
                    return _context.CPDTrainings.Find(entityId);
                case "LicenceApplication":
                    return _context.LicenseApplications.Find(entityId);
                // Add more cases as needed
                default:
                    return null;
            }
        }

        // New method
        public int ParseReferencedEntityId(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return 0;
            }

            string[] parts = description.Split(' ');
            if (parts.Length < 2)
            {
                return 0;
            }

            if (int.TryParse(parts[parts.Length - 1], out int entityId))
            {
                return entityId;
            }

            return 0;
        }
    }
}