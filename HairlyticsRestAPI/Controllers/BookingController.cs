using Hairlytics.Application.DTOs.BookingDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService) {
             _bookingService=bookingService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDto bookingCreateDto)
        {
            var response = await _bookingService.CreateBooking(bookingCreateDto);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }



        [HttpPost("create/order/{bookingId}")]
        public async Task<IActionResult> CreateOrder(int bookingId, PaymentGateway paymentGateway)
        {
            var response = await _bookingService.CreatePaymentOrder(bookingId, paymentGateway);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
