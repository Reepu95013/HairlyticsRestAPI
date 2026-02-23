using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public int VendorProfileId { get; set; }

        public required string ServiceName { get; set; }
        public required string MainImage { get; set; }
        public decimal Price { get; set; }
        public decimal? OffPrice { get; set; }
        public required string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 🔗 Navigation
        public VendorProfile? VendorProfile { get; set; }
        public ICollection<ServiceImage> ServiceImages { get; set; } = new List<ServiceImage>();
    }
}
