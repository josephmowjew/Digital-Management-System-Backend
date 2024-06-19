using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
  public class MemberQualificationRepository : Repository<MemberQualification>, IMemberQualificationRepository
  {
    protected readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public MemberQualificationRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
      this._context = context;
      this._unitOfWork = unitOfWork;
    }

    public async Task<MemberQualification?> GetMemberQualificationByMemberId(int memberId)
    {
      return await _context.MemberQualifications
                .Include(m => m.Member)
                .Include(q => q.QualificationType)
                .Include(t => t.Attachments)
                .ThenInclude(t => t.AttachmentType)
                .FirstOrDefaultAsync(x => x.MemberId == memberId);
    }

    public new async Task<MemberQualification> GetByIdAsync(int id)
    {
      return await _context.MemberQualifications
                .Include(m => m.Member)
                
                .Include(q => q.QualificationType)
                .FirstOrDefaultAsync(x => x.Id == id);
    }
  }
}
