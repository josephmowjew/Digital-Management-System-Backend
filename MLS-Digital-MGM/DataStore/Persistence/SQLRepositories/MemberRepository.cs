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
            return await _context.Members.Include(m => m.User).FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public new async Task<List<Member>> GetAllAsync()
        {
            return await _context.Members.Include(m => m.User).ToListAsync();
        }
    }
}
