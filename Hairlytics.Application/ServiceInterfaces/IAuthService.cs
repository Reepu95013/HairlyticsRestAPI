using Hairlytics.Application.DTOs.HelperDTOs;
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
        
        Task<ServiceResponse<TokenResponseDto>> RegisterUserAsync(UserCreateDto dto);
        Task<ServiceResponse<TokenResponseDto>> LoginUserAsync(string username, string password);
        Task<ServiceResponse<TokenResponseDto?>> RefreshTokenAsync(int userId, string refreshToken);

    }
}
