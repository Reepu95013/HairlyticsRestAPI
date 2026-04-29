using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class Payment
    {
       public int Id { get; set; }
       public int BookingId { get; set; }
       public Booking? Booking { get; set; }
       public decimal TotalAmount { get; set; }
       public PaymentMethod PaymentMethod { get; set; }
       public PaymentGateway PaymentGateway { get; set; }
       public PaymentTransactionStatus Status { get; set; }
       public string? TransactionId { get; set; }
       public string? OrderId { get; set; } // From payment gateway
       public string? Signature { get; set; } // For verification
       public DateTime CreatedAt { get; set; }
       public DateTime UpdatedAt { get; set; }
    }
}
