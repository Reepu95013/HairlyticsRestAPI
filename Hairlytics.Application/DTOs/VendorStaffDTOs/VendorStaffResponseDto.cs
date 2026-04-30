using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.VendorStaffDTOs
{
    public class VendorStaffResponseDto
    {
        public int Id { get; set; }
        public int VendorProfileId { get; set; }

        // 👤 Basic Info
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? ProfileImageUrl { get; set; }
        public string? Description { get; set; }
        public int ExperienceYears { get; set; }

        // ⏱ Working Hours
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // 📅 Status
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }

        // ⭐ Performance
        public double Rating { get; set; }
        public int TotalReviews { get; set; }

        // 📅 Audit
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 🔗 Availability (Optional but useful)
        public List<StaffAvailabilityResponseDto>? StaffAvailabilities { get; set; }
    }
}
