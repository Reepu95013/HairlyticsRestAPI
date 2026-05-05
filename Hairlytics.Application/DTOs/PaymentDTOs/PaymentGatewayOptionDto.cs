using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.PaymentDTOs
{
    public class PaymentGatewayOptionDto
    {
        public string? Name { get; set; }      // Display name
        public PaymentGateway Value { get; set; }     // Enum/string value
        public string? Icon { get; set; }      // Icon URL
    }
}
