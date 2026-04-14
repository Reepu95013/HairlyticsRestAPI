using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.VendroGalleryDTOs
{
    public class VendorGalleryResponseDto
    {
        public int Id { get; set; }

        public int VendorProfileId { get; set; }
        public VendorProfile? VendorProfile { get; set; }

        public required string ImageUrl { get; set; }

        public bool IsThumbnail { get; set; }
        public bool IsActive { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
