using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
   public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public UserController(IUserService userService, IEmailService emailService) { 
            _userService = userService;
            _emailService = emailService;
        }

        [Authorize(Roles = nameof(UserRole.Vendor))]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers() {
           var users = await _userService.GetUsersAsync();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var users = await _userService.GetUserAsync(userId);
            return Ok(users);
        }


        [Authorize(Roles = "Vendor")]
        [HttpGet("vendor-test")]
        public IActionResult VendorTest()
        {
            var user = User.Identity?.IsAuthenticated;
            var roles = User.Claims
                .Where(c => c.Type.Contains("role"))
                .Select(c => c.Value);

            return Ok(new
            {
                IsAuthenticated = user,
                Roles = roles
            });
        }


        [Authorize(Roles = "Vendor")]
        [HttpGet("vendor")]
        public IActionResult VendorRoute()
        {
            return Ok("Vendor route hit");
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(string email)
        {

            var emailData = new EmailDto(email, "Hello Gmail Service", EmailBody.EmailStringBody(email));
          
            _emailService.SendEmail(emailData);               

            return Ok("Email sent successfully");
        }
    }
}
