using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Hairlytics.Application.DTOs.UserDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
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

        public UserService(IAuthRepository authRepository,IUserRepository userRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<IEnumerable<UserResponseDto>>GetUsersAsync()
        {
           var user = await _userRepository.GetUsersAsync();

            var users =  _mapper.Map<IEnumerable<UserResponseDto>>(user);
            return users;
        }
    }
}
