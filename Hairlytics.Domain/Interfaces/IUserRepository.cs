using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>>GetUsersAsync();
        Task<User> GetUserAsync(int UserId);
        Task<bool> CheckEmailExitsAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> CheckAdminExitsAsync(UserRole userRole);

        Task<List<User>> GetUserListAsync(UserRole userRole);

    }
}
