using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.DTOs.ServiceDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Application.Services;
using Hairlytics.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService) {
            _serviceService=serviceService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create(ServiceCreateDto serviceCreateDto)
        {
            var response = await _serviceService.AddServiceAsync(serviceCreateDto);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
    }
}
