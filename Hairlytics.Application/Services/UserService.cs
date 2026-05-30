using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.UserDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using Hairlytics.Domain.Interfaces;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Hairlytics.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthRepository  _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;

        public UserService(IAuthRepository authRepository,IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, IEmailService emailService)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<ServiceResponse<string>> ActiveUser(int userId)
        {
            var response = new ServiceResponse<string>();

            try
            {
               var user = await _userRepository.ActiveUserAsync(userId);
                var mail = new EmailDto
                (
                    user.Email,
                    "Account Active!",
                    EmailBody.EmailStringBody($"Your account has been active now!")

                );
                _emailService.SendEmail(mail);

                response.Success = true;
                response.Message = "User Active Successfuly!";
                response.Data = "Success!";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = "something went wrong";

                return response;
            }
        }

        public async Task<bool> CheckEmailExitsAsync(string email)
        {
            return await _userRepository.CheckEmailExitsAsync(email);
        }

        public async Task<ServiceResponse<string>> DeleteUserAsync(int userId)
        {
            var response = new ServiceResponse<string>();

            try
            {
                await _userRepository.DeleteUser(userId);
                response.Success = true;
                response.Message = "User Delete Successfuly!";
                response.Data = "User Delete Successfuly!";
                return response;
            }
            catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = "something went wrong";

                return response;
            }

        }

        public async Task<UserResponseDto> GetUserAsync(int UserId)
        {
            var user = await _userRepository.GetUserAsync(UserId);  
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<IEnumerable<UserResponseDto>>GetUsersAsync()
        {
           var user = await _userRepository.GetUsersAsync();

            var users =  _mapper.Map<IEnumerable<UserResponseDto>>(user);
            return users;
        }


        public async Task<List<UserResponseDto>> GetUsersAsync(UserRole userRole, int pageNumber, int pageSize)
        {
            var user = await _userRepository.GetUserListAsync(userRole, pageNumber, pageSize);

            var users = _mapper.Map<List<UserResponseDto>>(user);
            return users;
        }

        public async Task<PagedResultDto<UserResponseDto>> GetUsersPagedAsync(UserRole userRole, int pageNumber, int pageSize)
        {
            var total = await _userRepository.GetUserCountAsync(userRole);
            var users = await _userRepository.GetUserListAsync(userRole, pageNumber, pageSize);

            return new PagedResultDto<UserResponseDto>
            {
                Items = _mapper.Map<List<UserResponseDto>>(users),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public async Task<ServiceResponse<string>> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (userUpdateDto.Password != null)
                {
                    string hashedPassword = _passwordHasher.HashPassword(userUpdateDto.Password);

                    userUpdateDto.Password = hashedPassword;
                }               

                var user = _mapper.Map<User>(userUpdateDto);
                await _userRepository.UpdateUser(user);

                response.Success = true;
                response.Message = "Profile updated successfully!";
                response.Data = "Profile updated successfully!";
                return response;
            }
            catch(Exception ex)
            {
                response.Success = true;
                response.Message = ex.Message;
                response.Data = "Something went wrong!";
                return response;

            }
          
        }
    }
}
