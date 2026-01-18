using Hairlytics.Application.DTOs.UserDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/[controller]")]
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


        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
