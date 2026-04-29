using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class VendorStaff
    {
        public int Id { get; set; }
        public int VendorProfileId { get; set; }
        public VendorProfile? VendorProfile { get; set; }

        // 👤 Basic Info
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Description { get; set; }
        public int ExperienceYears { get; set; }

        // ⏱ Working Hours (Default)
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // 📅 Status
        public bool IsActive { get; set; } = true;
        public bool IsAvailable { get; set; } = true;

        // ⭐ Performance
        public double Rating { get; set; }
        public int TotalReviews { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 🔗 Navigation
        public ICollection<StaffAvailability> StaffAvailabilities { get; set; } = new List<StaffAvailability>();
    }
}
