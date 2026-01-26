using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) { 
        
            _context = context;
        }

        public async Task<bool> CheckEmailExitsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }


        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<User> GetUserAsync(int UserId)
        {
            return await _context.Users.FirstAsync(u=>u.Id == UserId);
        }

        public async Task<IEnumerable<User>>GetUsersAsync() {

            return await _context.Users.Include(u => u.VendorProfile).AsNoTracking().ToListAsync();
        }

      


    }
}
