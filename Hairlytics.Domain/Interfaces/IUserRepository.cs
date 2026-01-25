using Hairlytics.Domain.Entities;
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
    }
}
