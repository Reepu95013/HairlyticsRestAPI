using Hairlytics.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.ServiceDTOs
{
    public class ServiceCreateDto
    {
        public required int VendorProfileId { get; set; }
        public required int CategoryId { get; set; }
        public required string ServiceName { get; set; }
        public required IFormFile ImageFile { get; set; }
        public string? Image { get; set; }
        public required int Duration { get; set; }
        public required decimal Price { get; set; }
        public decimal? OffPrice { get; set; } 
        public required string Description { get; set; }
        public bool Status { get; set; }

       }
}
