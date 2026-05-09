using Hairlytics.Application.DTOs.BookingDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.RazorpayDTOs;
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
        private readonly IRazorpayService _razorpayService;
        public BookingController(IBookingService bookingService, IRazorpayService razorpayService) {
             _bookingService=bookingService;
            _razorpayService=razorpayService;
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


        [HttpPost("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var response = await _bookingService.CancelBooking(bookingId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("verify/razorpay/payment")]
        public async Task<IActionResult> VerifyPayment(VerifyPaymentDto verifyPaymentDto)
        {
            var response = await _razorpayService.VerifyPayment(verifyPaymentDto);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }


        // access only admin
        [HttpGet("list")]
        public async Task<IActionResult> GetAllBooking([FromQuery] PaginationDto paginationDto)
        {
            var response = await _bookingService.GetBookingList(paginationDto);

            return Ok(response);
        }

        // access only admin
        [HttpGet("cancel/list")]
        public async Task<IActionResult> GetAllCancelledBooking([FromQuery] PaginationDto paginationDto)
        {
            var response = await _bookingService.GetCancelledBookingList(paginationDto);

            return Ok(response);
        }


        // access admin, vendor, 
        [HttpGet("list/vendor/{vendorId}")]
        public async Task<IActionResult> GetAllBookingByVendor([FromQuery] PaginationDto paginationDto , int vendorId)
        {
            var response = await _bookingService.GetBookingListByVendor(paginationDto, vendorId);

            return Ok(response);
        }

        // access admin, vendor, 
        [HttpGet("list/staff/{staffId}")]
        public async Task<IActionResult> GetAllBookingByStaff([FromQuery] PaginationDto paginationDto, int staffId)
        {
            var response = await _bookingService.GetBookingListByStaff(paginationDto, staffId);

            return Ok(response);
        }


        // access admin, user
        [HttpGet("list/user/{userId}")]
        public async Task<IActionResult> GetAllBookingByUser([FromQuery] PaginationDto paginationDto, int userId)
        {
            var response = await _bookingService.GetBookingListByUser(paginationDto, userId);

            return Ok(response);
        }

    }
}
