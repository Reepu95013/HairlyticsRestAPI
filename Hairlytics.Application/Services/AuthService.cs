using AutoMapper;
using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.UserDTOs;
using Hairlytics.Application.DTOs.VendorProfileDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using Hairlytics.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
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
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;


        public AuthService(IAuthRepository authRepository, IMapper mapper, IPasswordHasher passwordHasher, IJwtService jwtService, IUserRepository userRepository, IEmailService emailService, ISmsService smsService)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _emailService = emailService;
            _smsService = smsService;

        }

        public async Task<ServiceResponse<string>> ForgortPasswordAsync(string username)
        {
            // declare retun type 
            var response = new ServiceResponse<string>();
            var user = await _authRepository.GetByUsernameAsync(username);
            if (user != null)
            {
                var otp = new Random().Next(100000, 999999).ToString();

                var forgortpasswrd = new ForgotPassword
                {
                    Email = user.Email,
                    Phone = user.Phone,
                    OTP = otp,
                    ExpiryTime = DateTime.Now.AddMinutes(10),
                };

                var mail = new EmailDto
                (
                    user.Email,
                    "Forgot Password",
                    EmailBody.EmailStringBody($"Your OTP is {otp}. It is valid for 10 minutes.")

                );
                _emailService.SendEmail(mail);
                await _authRepository.ForgotPassword(forgortpasswrd);

                response.Success = true;
                response.Message = $"Your OTP is sent on your register email id";
                response.Data = otp;
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid username!";
            }

            return response;


        }

        public async Task<ServiceResponse<TokenResponseDto>> LoginUserAsync(string username, string password)
        {
            // declare retun type 
            var response = new ServiceResponse<TokenResponseDto>();

            // check user exit or not
            var user = await _authRepository.GetByUsernameAsync(username);
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

            var refreshTokenData = await _authRepository.RefreshToken(userId, refreshToken);
            if (refreshTokenData != null)
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


            if(dto.Role == UserRole.Admin)
            {
                bool adminExit = await _userRepository.CheckAdminExitsAsync(dto.Role);

                if (adminExit)
                {
                    response.Success = false;
                    response.Message = "You can't create multiple admin!";
                    return response;
                }

            }

          

            var existingUser = await _authRepository.GetByUsernameAsync(dto.Username);

            bool emailexit = await _authRepository.IsExitsEmailAsync(dto.Email);

            if (emailexit)
            {
                response.Success = false;
                response.Message = "Email already exists , try with another eamil!";
                return response;
            }


            bool exitsPhone = await _authRepository.IsExitsPhoneAsync(dto.Phone);

            if (exitsPhone)
            {
                response.Success = false;
                response.Message = "Phone Number already exists , try with another phone number!";
                return response;
            }


            if (existingUser != null)
            {
                response.Success = false;
                response.Message = "Username already exists.";
            }
            else
            {
                // validate role
                if (dto.Role == UserRole.SubAdmin)
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
                            vendorProfile.Status = false;

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

        public async Task<ServiceResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            // declare retun type 
            var response = new ServiceResponse<string>();
            if (resetPasswordDto != null)
            {
                if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword || resetPasswordDto.Password.Length < 8)
                {
                    response.Success = false;
                    response.Message = "Password and confirm password must be same and password length should be 8";
                }
                else
                {
                    var data = await _authRepository.GetResetPasswordData(resetPasswordDto.Email);

                    if (data != null)
                    {

                        if (data.ExpiryTime < DateTime.Now)
                        {
                            response.Success = false;
                            response.Message = "Your time is expaired, Please try again!";

                        }
                        else if (data.OTP != resetPasswordDto.OTP)
                        {
                            response.Success = false;
                            response.Message = "Invalid OTP!";

                        }
                        else
                        {
                            // creating hash password
                            string hashedPassword = _passwordHasher.HashPassword(resetPasswordDto.Password);

                            var user = await _userRepository.GetUserByEmailAsync(resetPasswordDto.Email);
                            if (user != null)
                            {
                                user.Password = hashedPassword;
                                user.UpdatedAt = DateTime.Now;
                                data.Revoke = true;
                                await _authRepository.SaveChangesAsync();
                                response.Success = true;
                                response.Message = "Your Passsword has been successfuly reset!";
                            }
                            else
                            {
                                response.Success = false;
                                response.Message = "Not found record!";

                            }
                        }

                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Not found record!";
                    }



                }

            }
            else
            {
                response.Success = false;
                response.Message = "All fields are required!";
            }



            return response;
        }


        public async Task<ServiceResponse<string>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            // declare retun type 
            var response = new ServiceResponse<string>();

            var user = await _authRepository.GetByUsernameAsync(changePasswordDto.Username);

            if (user != null)
            {
                if (changePasswordDto.Password != changePasswordDto.ConfirmPassword || changePasswordDto.Password.Length < 8)
                {
                    response.Success = false;
                    response.Message = "Password and confirm password must be same and password length should be 8";
                }
                else
                {

                    // creating hash password
                    string hashedPassword = _passwordHasher.HashPassword(changePasswordDto.Password);
                    user.Password = hashedPassword;
                    user.UpdatedAt = DateTime.Now;
                    await _authRepository.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "Password successfuly changed!";

                }
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }

            return response;
        }




        //  Admin work //


        public async Task<UserLoginDto?> LoginAdminAsync(LoginDto dto)
        {
            var user = await _authRepository.GetByUsernameAsync(dto.Username);


            if (user == null)
            {
                return null;
            }
            else
            {

                // verify password
                bool isPasswordCorrect = _passwordHasher.VerifyPassword(dto.Password, user.Password);
                if (!isPasswordCorrect)
                {
                    return null;
                }
                else
                {
                    return new UserLoginDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Role = user.Role
                    };

                }

            }

        }


        public async Task<ServiceResponse<TokenResponseDto>> RegisterAdminAsync(UserCreateDto dto)
        {
            // declare retun type 
            var response = new ServiceResponse<TokenResponseDto>();

            if (!new EmailAddressAttribute().IsValid(dto.Email))
            {
                response.Success = false;
                response.Message = "Invalid email address";
                return response;
            }

            var existingUser = await _authRepository.GetByUsernameAsync(dto.Username);

            bool emailexit = existingUser?.Email == dto.Email;

            if (emailexit)
            {
                response.Success = false;
                response.Message = "Email already exists , try with another eamil!";
                return response;
            }

            if (existingUser != null)
            {
                response.Success = false;
                response.Message = "Username already exists.";
            }
            else
            {
                // validate role
                if (dto.Role == UserRole.Admin)
                {
                    response.Success = false;
                    response.Message = "You can't create multitple admin!";
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

        public async Task<ServiceResponse<string>> SendPhoneOtp(string phoneNumber)
        {
            var response = new ServiceResponse<string>();
            try
            {

                // Generate OTP
                var otp = new Random().Next(100000, 999999).ToString();


                RegisterPhoneNumber data = new RegisterPhoneNumber()
                {

                    OtpCode = otp,
                    PhoneNumber = phoneNumber,
                    ExpiryTime = DateTime.Now.AddMinutes(5),

                };

                await _authRepository.RegisterPhoneNumber(data);

                await _smsService.SendOtpSms(phoneNumber, otp);


                response.Success = true;
                response.Message = "Success!";
                response.Data = "Otp has been sent your phone number!";



            }
            catch (Exception ex) { 
                response.Success = false;
                response.Message = "Failed!";
                response.Data = ex.Message;
            
            }

            return response;
        }

        //public async Task LogoutAsync()
        //{
        //    //await _httpContextAccessor.HttpContext.SignOutAsync(
        //    //CookieAuthenticationDefaults.AuthenticationScheme);
        //    await
        //}







    }
}
