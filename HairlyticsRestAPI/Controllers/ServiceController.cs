using Hairlytics.Application.DTOs.ServiceDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin) + "," + nameof(UserRole.Vendor))]
        [HttpPost("create")]
        public async Task<IActionResult> Create(ServiceCreateDto serviceCreateDto)
        {
            var response = await _serviceService.AddServiceAsync(serviceCreateDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("list/{vendorProfileId}")]
        public async Task<IActionResult> GetServicesByVendor(int vendorProfileId)
        {
            var response = await _serviceService.GetServiceListAsync(vendorProfileId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetActiveServices()
        {
            var response = await _serviceService.GetServiceListAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpGet("list/all")]
        public async Task<IActionResult> GetAllServices()
        {
            var response = await _serviceService.GetAllServicesAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var response = await _serviceService.GetServiceAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin) + "," + nameof(UserRole.Vendor))]
        [HttpPost("update")]
        public async Task<IActionResult> Update(ServiceUpdateDto serviceUpdateDto)
        {
            var response = await _serviceService.UpdateServiceAsync(serviceUpdateDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _serviceService.DeleteServiceAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
