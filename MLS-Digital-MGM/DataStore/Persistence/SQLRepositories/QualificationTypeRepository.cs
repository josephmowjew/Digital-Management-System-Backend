using DataStore.Core.Models;
using DataStore.Data;  
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
  public class QualificationTypeRepository : Repository<QualificationType>, IQualificationTypeRepository
  {
    protected readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public QualificationTypeRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork) 
    {
      this._context = context;
      this._unitOfWork = unitOfWork;
    }
  }
}
