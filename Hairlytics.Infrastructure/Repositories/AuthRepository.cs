using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.Repositories
{
   
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CreateRefreshToken(RefreshToken refreshToken)
        {
            if (refreshToken.UserId!=0)
            {
                var oldTokens = await _context.RefreshTokens
                .Where(x => x.UserId == refreshToken.UserId && !x.IsRevoked)
                .ToListAsync();

                foreach (var token in oldTokens)
                {
                    token.IsRevoked = true;
                    token.ExpiryDate = DateTime.Now;
                }
            }
           await _context.RefreshTokens.AddAsync(refreshToken);
           await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> RefreshToken(int userId, string refreshToken)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId==userId && x.Token==refreshToken && !x.IsRevoked && x.ExpiryDate > DateTime.Now);

            if (token == null)
            {
                return null;
            }

            token.ExpiryDate = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();   
            
            return token;
        }

        public async Task ForgotPassword(ForgotPassword forgotPassword)
        {
           
                var oldEntry = await _context.ForgotPassword
                .Where(x => x.Email ==  forgotPassword.Email && !x.Revoke)
                .ToListAsync();

                foreach (var entry in oldEntry)
                {
                    entry.Revoke = true;                  
                }
                await _context.ForgotPassword.AddAsync(forgotPassword);
                await _context.SaveChangesAsync();
        }

        public async Task<ForgotPassword?> GetResetPasswordData(string email)
        {
            var data = await _context.ForgotPassword
                .FirstOrDefaultAsync(u => u.Email == email && !u.Revoke);
            return data;
        }
    }
}
