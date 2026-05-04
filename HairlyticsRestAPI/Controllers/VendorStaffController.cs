using Hairlytics.Application.DTOs.VendorStaffDTOs;
using Hairlytics.Application.DTOs.VendroGalleryDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/vendor/staff")]
    [ApiController]
    public class VendorStaffController : ControllerBase
    {
        private readonly IVendorStaffService _vendorStaffService;
        public VendorStaffController(IVendorStaffService vendorStaffService) {
            _vendorStaffService = vendorStaffService;

        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] VendorStaffCreateDto vendorStaffCreateDto)
        {
            var response = await _vendorStaffService.CreateVendorStafAsync(vendorStaffCreateDto);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpPost("add/availability")]
        public async Task<IActionResult> AddStaffAvailability(int staffId,[FromBody] List<StaffAvailabilityCreateDto> staffAvailabilityCreateDtos)
        {
            if (staffId <= 0)
                return BadRequest("Invalid staffId");

            var response = await _vendorStaffService.AddVendorStafAvailabilityAsync(staffId, staffAvailabilityCreateDtos);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffDetails(int id)
        {
            var response  = await  _vendorStaffService.GetVendorStafDetailsAsync(id);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }


        [HttpPost("get/available/slots")]
        public async Task<IActionResult> GetAvailableSlots([FromBody] StaffAvailabilitySlotDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request");

            var response = await _vendorStaffService.GetAvailableSlots(dto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


    }
}
