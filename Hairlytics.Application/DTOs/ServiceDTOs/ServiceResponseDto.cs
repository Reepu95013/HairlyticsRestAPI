using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.ServiceDTOs
{
    public class ServiceResponseDto
    {
        public int Id { get; set; }
        public int VendorProfileId { get; set; }
        public int CategoryId { get; set; }
        public required string ServiceName { get; set; }
        public required string Image { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public decimal? OffPrice { get; set; }
        public required string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CategoryResponseDto? CategoryResponseDto { get; set; }

    }
}
