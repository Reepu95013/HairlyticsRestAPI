using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.TokenDTOs;
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
        
        Task<ServiceResponse<UserResponseDto?>> RegisterUserAsync(UserCreateDto dto);
        Task<TokenResponseDto?> LoginUserAsync(string username, string password);

    }
}
