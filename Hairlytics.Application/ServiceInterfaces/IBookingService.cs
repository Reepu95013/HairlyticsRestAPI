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
        Task <ServiceResponse<OnlinePaymentResponseDto>> CreateBooking(BookingCreateDto bookingCreateDto) ;
        Task<ServiceResponse<RazorpayCreateOrderResponse>> CreatePaymentOrder(int bookingId, PaymentGateway paymentGateway);

    }
}
