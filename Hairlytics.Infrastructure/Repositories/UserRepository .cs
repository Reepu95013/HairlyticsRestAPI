using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
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
            var user = await _context.Users
            .Include(u => u.VendorProfile).ThenInclude(vp => vp.Documents)
            .FirstOrDefaultAsync(u => u.Id == UserId);
            return user;
        }

        public async Task<IEnumerable<User>>GetUsersAsync() {

            return await _context.Users.Include(u => u.VendorProfile).AsNoTracking().ToListAsync();
        }

        public async Task<bool> CheckAdminExitsAsync(UserRole userRole)
        {
            return await _context.Users.AnyAsync(u => u.Role == userRole);

        }
        

        public async Task<List<User>> GetUserListAsync(UserRole userRole, int pageNumber, int pageSize)
        {
            return await _context.Users
                .Include(u => u.VendorProfile) 
                .Where(u => u.Role == userRole && u.IsActive)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateUser(User user)
        {
            var response = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
           
            if (response != null)
            {
                if (user.Name != null) response.Name = user.Name;
                if (user.LastName != null) response.LastName = user.LastName;
                if (user.Birth != null) response.Birth = user.Birth;
                if (user.Password != null) response.Password = user.Password;

                response.UpdatedAt = DateTime.Now;

                if (user.VendorProfile != null && response.VendorProfile != null)
                {
                    response.VendorProfile.ShopName = user.VendorProfile.ShopName;
                    response.VendorProfile.UpdatedAt = DateTime.Now;
                }

               
               await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteUser(int userId)
        {
            var response = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (response != null)
            {
                response.IsActive = false;
                response.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
    }
}
