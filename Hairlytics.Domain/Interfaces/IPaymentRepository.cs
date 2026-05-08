using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);
        Task<Payment> GetPaymentByBookingIdAsync(int bookingId);
        Task<Payment> GetPaymentByOrderIdAsync(string orderId);
        Task UpdatePaymentAsync(Payment payment);
    }
}
