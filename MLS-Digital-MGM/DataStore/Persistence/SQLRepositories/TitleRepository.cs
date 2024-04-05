using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace DataStore.Persistence.SQLRepositories
{
    public class TitleRepository : Repository<Title>, ITitleRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public TitleRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        // Implement additional methods here for title-specific operations
    }
}