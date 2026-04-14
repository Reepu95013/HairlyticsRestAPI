using Hairlytics.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.VendroGalleryDTOs
{
    public class VendorGalleryCreateDto
    {
        public required int VendorProfileId { get; set; }
        public string? ImageUrl { get; set; }
        public required IFormFile ImageFile { get; set; }
        public bool IsThumbnail { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
