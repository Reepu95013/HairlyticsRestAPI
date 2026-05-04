using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
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

        //[Authorize(Roles = nameof(UserRole.Vendor) + "," + nameof(UserRole.Admin))]
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


        //[Authorize]
        [HttpPost("active/{userId}")]
        public async Task<IActionResult> ActiveUserStatus(int userId)
        {
            if (userId <= 0)
                return BadRequest("UserId is required");

            var response = await _userService.ActiveUser(userId);

            return response.Success ? Ok(response) : BadRequest(response);
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


        //[Authorize(Roles = "Vendor")]
        [HttpGet("get/vendor/{pageNumber}")]
        public async Task<IActionResult> GetVendors(int pageNumber)
        {
          var response  =  await _userService.GetUsersAsync(UserRole.Vendor, pageNumber, 10);          
          return Ok(response);
        }


        [HttpPost("send")]
        public IActionResult SendEmail(string email)
        {
            var emailData = new EmailDto(email, "Hello Gmail Service", EmailBody.EmailStringBody(email));
          
            _emailService.SendEmail(emailData);               

            return Ok("Email sent successfully");
        }
    }
}
