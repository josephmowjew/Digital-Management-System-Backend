using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
  public class MemberRepository : Repository<Member>, IMemberRepository
  {
    protected readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public MemberRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork) 
    {
      this._context = context;
      this._unitOfWork = unitOfWork;
    }

        public async Task<Member?> GetMemberByUserId(string userId)
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Customer).FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Customer).Where(q => q.Status != Lambda.Deleted).ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Customer).FirstOrDefaultAsync(q => q.Id == id && q.Status != Lambda.Deleted);
        }

        public async Task<int> GetMembersCountAsync(){
            return await _context.Members.CountAsync(q => q.Status == Lambda.Active);
        }

        
        public async Task<int> GetLicensedMembersCountAsync()
        {
            return await _context.Members
                .CountAsync(m => _context.Licenses.Any(l => l.MemberId == m.Id) && m.Status != Lambda.Deleted);
        }

        //get unlicensed members count
        public async Task<int> GetUnlicensedMembersCountAsync()
        {
            return await _context.Members
                .CountAsync(m => !_context.Licenses.Any(l => l.MemberId == m.Id) && m.Status != Lambda.Deleted);
        }

        
        public async Task<IEnumerable<Member>> GetLicensedMembersAsync()
        {
            return await _context.Members
                .Include(m => m.User)
                .Include(m => m.Customer)
                .Include(m => m.Firm)
                .Where(m => _context.Licenses.Any(l => l.MemberId == m.Id) && m.Status != Lambda.Deleted)
                .ToListAsync();
        }
        
    }
}
