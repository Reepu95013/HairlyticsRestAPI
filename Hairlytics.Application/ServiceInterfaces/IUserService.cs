using Hairlytics.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hairlytics.Domain.Enums;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IUserService
    {
       
        Task<IEnumerable<UserResponseDto>>GetUsersAsync();
        Task<UserResponseDto> GetUserAsync(int UserId);
        Task<bool> CheckEmailExitsAsync(string email);
        Task<List<UserResponseDto>> GetUsersAsync(UserRole userRole, int pageNumber, int pageSize);
    }
}
