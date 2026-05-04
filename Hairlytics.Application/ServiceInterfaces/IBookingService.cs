using Hairlytics.Application.DTOs.BookingDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IBookingService
    {
        Task <ServiceResponse<string>> CreateBooking(BookingCreateDto bookingCreateDto) ;
    }
}
