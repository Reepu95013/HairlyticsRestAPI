using Hairlytics.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IAuthService
    {
        
        Task RegisterUserAsync(UserCreateDto dto);
        Task LoginUserAsync(string username, string password);

    }
}
