using Hairlytics.Application.DTOs.UserDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;


        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto dto)
        {
            var result = await _authService.RegisterUserAsync(dto);
            if (result.Success == false)
            {
                return BadRequest(result.Message);
            }
            return Created("Register",result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
           var tokenData =  await _authService.LoginUserAsync(loginDto.Username, loginDto.Password);
            if (tokenData.Success==false)
                return Unauthorized(tokenData.Message);

            return Ok(tokenData);
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto dto)
        {
            var tokenData = await _authService.RefreshTokenAsync(dto.UserId, dto.RefreshToken); 
            if (tokenData.Success==false)
            {
                return BadRequest(tokenData.Message);
            }
            return Ok(tokenData);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordDto forgotPasswordDto)
        {
           var data =  await _authService.ForgortPasswordAsync(forgotPasswordDto.Username);

            if(data.Success == false)
            {
                return BadRequest(data.Message);
            } 

            return Ok(data);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var data = await _authService.ResetPasswordAsync(resetPasswordDto);
            if(data.Success == false)
            {
               return BadRequest(data.Message);
            }

            return Ok(data);
        }


        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var data = await _authService.ChangePasswordAsync(changePasswordDto);
            if (data.Success == false)
            {
                return BadRequest(data.Message);
            }

            return Ok(data);
        }        
    }
}
