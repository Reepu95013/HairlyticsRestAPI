using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.BookingDTOs
{
    public class BookingCreateDto
    {
        public int CustomerId { get; set; }
        public int VendorProfileId { get; set; }
        public int VendorStaffId { get; set; }

        // Usually, you only need IDs of services being booked
        public List<int> ServiceIds { get; set; } = new List<int>();

        public DateOnly AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public required string CustomerName { get; set; }
        public required string CustomerPhone { get; set; }

       
    }
}
