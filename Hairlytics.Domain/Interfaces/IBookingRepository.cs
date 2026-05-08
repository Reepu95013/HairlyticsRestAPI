using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task CreateBookingAsync(Booking booking);
        Task<List<Booking>>GetBookings(int staffId, DateOnly date);
        Task AddBookingServicesAsync(List<BookedService> BookedService);
        Task UpdateBookAsync(Booking booking);
        Task <Booking>GetBookingDetailByBookingIdAsync(int bookingId);


       
    }
}
