using Hairlytics.Application.DTOs.VendorDocumentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.VendorProfileDTOs
{
    public class VendorProfileResponseDto
    {
        public int Id { get; set; }
        public required string ShopName { get; set; }
        public required string ShopTelephone { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string Region { get; set; }
        public required string Country { get; set; }
        public required string PostalCode { get; set; }
        public required string TaxNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // navigational 
        public List<VendorDocumentResponseDto>? VendorDocumentResponseDto { get; set; }
    }
}
