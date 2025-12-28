using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? VendorProfileId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsGlobal { get; set; } = false;
        public DateTime CreateAt { get; set; } 
        public DateTime UpdateAt { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();


    }
}
