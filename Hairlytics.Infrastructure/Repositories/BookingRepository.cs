using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task UpdateBookAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<Booking> GetBookingDetailByBookingIdAsync(int bookingId)
        {
            var booking  =  await _context.Bookings
                     .FirstOrDefaultAsync(b =>
                         b.Id == bookingId &&
                         b.Status != BookingStatus.Cancelled);

            if (booking == null)
                throw new Exception("not fount booking");


            return booking;
        }
    }
}
