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

namespace Hairlytics.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthRepository  _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IAuthRepository authRepository,IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;

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
