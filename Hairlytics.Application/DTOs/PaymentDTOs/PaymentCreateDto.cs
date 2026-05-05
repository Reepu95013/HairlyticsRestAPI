using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.PaymentDTOs
{
    public class PaymentCreateDto
    {
        public int BookingId { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentGateway PaymentGateway { get; set; }
        public PaymentTransactionStatus Status { get; set; }
        public string? TransactionId { get; set; }
        public string? OrderId { get; set; } // From payment gateway
        public string? Signature { get; set; } // For verification

    }
}
