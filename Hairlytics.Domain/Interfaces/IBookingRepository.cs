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
        Task<List<Booking>>GetAllBookingAsync(int pageNumber, int pageSize);
        Task<List<Booking>> GetAllCancelledBookingAsync(int pageNumber, int pageSize);
        Task<List<Booking>> GetAllBookingByVendorAsync(int pageNumber, int pageSize, int vendorId);
        Task<List<Booking>> GetAllBookingByStaffAsync(int pageNumber, int pageSize, int staffId);
        Task<List<Booking>> GetAllBookingByUserAsync(int pageNumber, int pageSize, int userId);
        



    }
}
