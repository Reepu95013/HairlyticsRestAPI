using AutoMapper;
using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.TokenDTOs;
using Hairlytics.Application.DTOs.UserDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hairlytics.Domain.Enums;
using Hairlytics.Application.DTOs.VendorProfileDTOs;

namespace Hairlytics.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public AuthService(IAuthRepository authRepository, IMapper mapper, IPasswordHasher passwordHasher, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<TokenResponseDto?> LoginUserAsync(string username, string password)
        {

           var user  =  await _authRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            bool isPasswordCorrect = _passwordHasher.VerifyPassword(password, user.Password);
            if (!isPasswordCorrect) {
                return null;
            }

         string token = _jwtService.GenerateToken(user);

            return new TokenResponseDto
            {
                Expiration = DateTime.UtcNow,
                Token = token
            };

        }

        public async Task<ServiceResponse<UserResponseDto?>> RegisterUserAsync(UserCreateDto dto)
        {
            var response = new ServiceResponse<UserResponseDto?>();

            var existingUser = await _authRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                response.Success = false;
                response.Message = "Username already exists.";
                return response;
            }

            if (existingUser?.Role == UserRole.Admin) {
                response.Success = false;
                response.Message = "You have not permission!";
                return response;
            }

            string hashedPassword = _passwordHasher.HashPassword(dto.Password);

            dto.Password = hashedPassword;
            dto.CreatedAt = DateTime.Now;
            dto.UpdatedAt = DateTime.Now;

           var newUser = _mapper.Map<User>(dto);

            if (dto.Role == UserRole.Vendor)
            {
                if (dto.VendorProfileCreateDto == null)
                {
                    response.Success = false;
                    response.Message = "Vendor profile is required!";
                    return response;
                }
                    
               
                var vendorProfile = _mapper.Map<VendorProfile>(dto.VendorProfileCreateDto);
                vendorProfile.CreatedAt = DateTime.Now;
                vendorProfile.UpdatedAt = DateTime.Now;

                if (dto.VendorProfileCreateDto.VendorDocumentCreateDto != null)
                {
                    foreach (var docDto in dto.VendorProfileCreateDto.VendorDocumentCreateDto)
                    {
                        var document = _mapper.Map<VendorDocument>(docDto);
                        document.CreatedAt = DateTime.Now;
                        document.UpdatedAt = DateTime.Now;
                        vendorProfile.Documents.Add(document);
                    }
                }

                newUser.VendorProfile = vendorProfile;

            }

            

           await _authRepository.CreateUserAsync(newUser);
           await _authRepository.SaveChangesAsync();

            var user = _mapper.Map<UserResponseDto>(newUser);
           


            response.Data = user;
            response.Success = true;
            response.Message = "Your Account has been Created Successfully!";

           return response;
        }
    }
}
