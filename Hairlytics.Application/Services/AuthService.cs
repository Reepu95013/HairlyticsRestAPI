using AutoMapper;
using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.UserDTOs;
using Hairlytics.Application.DTOs.VendorProfileDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using Hairlytics.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public AuthService(IAuthRepository authRepository, IMapper mapper, IPasswordHasher passwordHasher, IJwtService jwtService, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<TokenResponseDto>> LoginUserAsync(string username, string password)
        {
           // declare retun type 
            var response = new ServiceResponse<TokenResponseDto>();

            // check user exit or not
            var user  =  await _authRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                response.Message = "Username or Password is incorrect!";
                response.Success = false;
            }
            else
            {

                // verify password
                bool isPasswordCorrect = _passwordHasher.VerifyPassword(password, user.Password);
                if (!isPasswordCorrect)
                {
                    response.Message = "Username or Password is incorrect!";
                    response.Success = false;
                }
                else
                {
                    // create  refresh token
                    string refreshToken = Helper.GenerateRefreshToken();

                    var refeshTokenData = new RefreshToken
                    {
                        UserId = user.Id,
                        Token = refreshToken,
                        ExpiryDate = DateTime.Now.AddDays(7),
                        IsRevoked = false
                    };

                    await _authRepository.CreateRefreshToken(refeshTokenData);


                    // generate jwt Token
                    string token = _jwtService.GenerateToken(user);


                    var data = new TokenResponseDto
                    {
                        Token = token,
                        RefreshToken = refreshToken

                    };

                    response.Data = data;
                    response.Success = true;
                    response.Message = "You are login Successfully!";

                }

            }

            return response;

        }

        public async Task<ServiceResponse<TokenResponseDto?>> RefreshTokenAsync(int userId, string refreshToken)
        {
            // declare retun type 
            var response = new ServiceResponse<TokenResponseDto?>();

            var  refreshTokenData = await _authRepository.RefreshToken(userId, refreshToken);
                    if (refreshTokenData!=null)
                    {
                        var user = await _userRepository.GetUserAsync(userId);
                        // generate jwt token
                        string token = _jwtService.GenerateToken(user);


                        var data = new TokenResponseDto
                        {
                            Token = token,
                            RefreshToken = refreshTokenData.Token
                        };

                        response.Data = data;
                        response.Success = true;
                        response.Message = "New Token Created Successfully!";

            }
            else
            {
                response.Success = false;
                response.Message = "Record not found!";

               
            }

            return response;
        }

        public async Task<ServiceResponse<TokenResponseDto>> RegisterUserAsync(UserCreateDto dto)
        {
            // declare retun type 
            var response = new ServiceResponse<TokenResponseDto>();

            if (!new EmailAddressAttribute().IsValid(dto.Email))
            {
                response.Success = false;
                response.Message = "Invalid email address";
                return response;
            }


            bool emailexit = await _userRepository.CheckEmailExitsAsync(dto.Email);

            if (emailexit)
            {
                response.Success = false;
                response.Message = "Email already exists , try with another eamil!";
                return response;
            }


            

            // check user exit or not
            var existingUser = await _authRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                response.Success = false;
                response.Message = "Username already exists.";               
            }
            else
            {
                // validate role
                if (dto.Role == UserRole.Admin || dto.Role == UserRole.SubAdmin)
                {
                    response.Success = false;
                    response.Message = "You have not permission!";
                }
                else
                {
                    // creating hash password
                    string hashedPassword = _passwordHasher.HashPassword(dto.Password);

                    dto.Password = hashedPassword;
                    dto.CreatedAt = DateTime.Now;
                    dto.UpdatedAt = DateTime.Now;

                    var newUser = _mapper.Map<User>(dto);

                    // create a user as vendor or customer
                    if (dto.Role == UserRole.Vendor)
                    {
                        if (dto.VendorProfileCreateDto == null)
                        {
                            response.Success = false;
                            response.Message = "Vendor profile is required!";
                        }
                        else
                        {
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
                    }

                    await _authRepository.CreateUserAsync(newUser);


                    // refresh token creating
                    string refreshToken = Helper.GenerateRefreshToken();

                    var refeshTokenData = new RefreshToken
                    {
                        UserId = newUser.Id,
                        Token = refreshToken,
                        ExpiryDate = DateTime.Now.AddDays(7),
                        IsRevoked = false
                    };

                    await _authRepository.CreateRefreshToken(refeshTokenData);
                    await _authRepository.SaveChangesAsync();

                    // generate jwt token
                    string token = _jwtService.GenerateToken(newUser);

                    var data = new TokenResponseDto
                    {
                        Token = token,
                        RefreshToken = refreshToken
                    };

                    response.Data = data;
                    response.Success = true;
                    response.Message = "Your Account has been Created Successfully!";

                }

            }            

           return response;
        }
    }
}
