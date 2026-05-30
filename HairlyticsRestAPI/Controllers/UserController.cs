using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.UserDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _userService.GetUserAsync(userId);
            return Ok(user);
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpGet("list/{role}/{pageNumber}")]
        public async Task<IActionResult> GetUsersByRole(UserRole role, int pageNumber, [FromQuery] int pageSize = 10)
        {
            var response = await _userService.GetUsersPagedAsync(role, pageNumber, pageSize);
            return Ok(response);
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            var response = await _userService.UpdateUserAsync(userUpdateDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpPost("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("UserId is required");

            var response = await _userService.DeleteUserAsync(userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
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

            return Ok(new { IsAuthenticated = user, Roles = roles });
        }

        [HttpGet("get/vendor/{pageNumber}")]
        public async Task<IActionResult> GetVendors(int pageNumber, [FromQuery] int pageSize = 10)
        {
            var response = await _userService.GetUsersPagedAsync(UserRole.Vendor, pageNumber, pageSize);
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
