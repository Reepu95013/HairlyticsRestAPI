using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Interfaces
{
    public interface IAuthRepository
    {
        //Task LoginUserAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
        Task CreateUserAsync(User user);
        Task SaveChangesAsync();
    }
}
