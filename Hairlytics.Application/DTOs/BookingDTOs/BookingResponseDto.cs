using Hairlytics.Application.DTOs.PaymentDTOs;
using Hairlytics.Application.DTOs.VendorProfileDTOs;
using Hairlytics.Application.DTOs.VendorStaffDTOs;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.BookingDTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VendorProfileId { get; set; }
       
        public int VendorStaffId { get; set; }
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
        public List<PaymentResponseDto>? Payments { get; set; } = new ();
        public List<BookedServiceResponseDto> BookedService { get; set; }= new();
    }
}
