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
    public class PaymentRepository : IPaymentRepository
    {

        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }
       

        public async Task<Payment> GetPaymentByBookingIdAsync(int bookingId)
        {
            var payment = await _context.Payments
                            .Where(p => p.BookingId == bookingId && p.Status !=PaymentTransactionStatus.Failed)
                            .OrderByDescending(p => p.CreatedAt)
                            .FirstOrDefaultAsync();

            if (payment == null)
                throw new Exception("payment not found!");

            return payment;

        }

        public Task<Payment> GetPaymentByOrderIdAsync(string orderId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
             _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
