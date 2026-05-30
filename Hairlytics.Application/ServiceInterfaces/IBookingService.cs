using Hairlytics.Application.DTOs.BookingDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.PaymentDTOs;
using Hairlytics.Application.DTOs.RazorpayDTOs;
using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IBookingService
    {
        Task<ServiceResponse<OnlinePaymentResponseDto>> CreateBooking(BookingCreateDto bookingCreateDto);
        Task<ServiceResponse<RazorpayCreateOrderResponse>> CreatePaymentOrder(int bookingId, PaymentGateway paymentGateway);
        Task<ServiceResponse<string>> CancelBooking(int bookingId);
        Task<ServiceResponse<List<BookingResponseDto>>> GetBookingList(PaginationDto paginationDto);
        Task<ServiceResponse<List<BookingResponseDto>>> GetCancelledBookingList(PaginationDto paginationDto);
        Task<ServiceResponse<List<BookingResponseDto>>> GetBookingListByVendor(PaginationDto paginationDto, int vendorId);
        Task<ServiceResponse<List<BookingResponseDto>>> GetBookingListByStaff(PaginationDto paginationDto, int staffId);
        Task<ServiceResponse<List<BookingResponseDto>>> GetBookingListByUser(PaginationDto paginationDto, int userId);
        Task<ServiceResponse<int>> GetBookingCount(DateOnly appointmentDate, BookingStatus bookingStatus);
    }
}
