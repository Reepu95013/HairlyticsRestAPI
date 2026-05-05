using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.PaymentDTOs
{
    public class OnlinePaymentResponseDto
    {
        public int BookingId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PaymentGatewayOptionDto>? Gateways { get; set; }
    }
}
