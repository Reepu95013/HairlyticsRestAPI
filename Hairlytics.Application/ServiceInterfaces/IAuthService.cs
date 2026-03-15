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
        Task<ServiceResponse<string>> ForgortPasswordAsync(string username);
        Task<ServiceResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        Task<ServiceResponse<string>> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        Task<UserLoginDto?> LoginAdminAsync(LoginDto request);

        Task<ServiceResponse<TokenResponseDto>> RegisterAdminAsync(UserCreateDto dto);


        //Task LogoutAsync();

    }
}
