using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VendorProfileId { get; set; }
        public VendorProfile? VendorProfile { get; set; }   

        public int VendorStaffId { get; set; }
        public VendorStaff? VendorStaff { get; set; }
        public ICollection<BookingService> BookingService { get; set; } = new List<BookingService>();
        public DateOnly AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<Payment>? Payments { get; set; }
    }
}
