using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.RazorpayDTOs
{
    public class VerifyPaymentDto
    {  
        public required string PaymentId { get; set; }
        public required string OrderId { get; set; }
        public required string Signature { get; set; }
    }
}
