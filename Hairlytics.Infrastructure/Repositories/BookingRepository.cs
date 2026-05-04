using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetBookings(int staffId, DateOnly date)
        {
            return await _context.Bookings
               .Where(b => b.VendorStaffId == staffId
                        && b.AppointmentDate == date
                        && b.Status != BookingStatus.Cancelled) // 🔥 ignore cancelled
               .ToListAsync();
        }


        public async Task AddBookingServicesAsync(List<BookedService> bookedServices)
        {
            await _context.BookedServices.AddRangeAsync(bookedServices);
            await _context.SaveChangesAsync();
        }

    }
}
