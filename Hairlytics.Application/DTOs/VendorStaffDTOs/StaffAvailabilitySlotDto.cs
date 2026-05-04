using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.VendorStaffDTOs
{
    public class StaffAvailabilitySlotDto
    {
        public int StaffId { get; set; }
        public DateOnly Date { get; set; }
        public List<int> ServiceIds { get; set; } = new();
    }
}
