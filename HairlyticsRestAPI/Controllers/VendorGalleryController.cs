using Hairlytics.Application.DTOs.VendroGalleryDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/vendor-gallery")]
    [ApiController]
    public class VendorGalleryController : ControllerBase
    {
        private IVendorGalleryService _vendorGalleryService;
        public VendorGalleryController(IVendorGalleryService vendorGalleryService) {
            _vendorGalleryService = vendorGalleryService;        
        }

       // [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin) + "," + nameof(UserRole.Vendor))]
        [HttpPost("create")]
        public async Task<IActionResult> Create(VendorGalleryCreateDto vendorGalleryCreateDto)
        {
            var response = await _vendorGalleryService.AddVendorGalleryAsync(vendorGalleryCreateDto);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        //[Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin) + "," + nameof(UserRole.Vendor))]
        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetVendorGallery(int vendorId)
        {
            var response = await _vendorGalleryService.GetVendorGalleryByVendorIdAsync(vendorId);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }



    }
}
