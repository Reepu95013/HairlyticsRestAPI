using Hairlytics.Application.DTOs.VendorDocumentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.VendorProfileDTOs
{
    public class VendorProfileUpdateDto
    {
        public string? ShopName { get; set; }
        public string? ShopTelephone { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? TaxNumber { get; set; }
        public bool Status { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<VendorDocumentUpdateDto>? VendorDocumentUpdateDto { get; set; }
    }
}
