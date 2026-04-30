using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.VendorStaffDTOs
{
    public class VendorStaffCreateDto
    {
        public int VendorProfileId { get; set; }

        // 👤 Basic Info
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }

        // 📸 Image Upload
        public required IFormFile ProfileImage { get; set; }

        public string? Description { get; set; }
        public int ExperienceYears { get; set; }

        // ⏱ Working Hours
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
