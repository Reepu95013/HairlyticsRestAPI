using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.RazorpayDTOs
{
    public class RazorpayCreateOrderResponse
    {
        public string? OrderId { get; set; }
        public  decimal? Amount { get; set; }
        public  string? Currency { get; set; }
        public string? Receipt { get; set; }
        public string? key { get; set; }

    }
}
