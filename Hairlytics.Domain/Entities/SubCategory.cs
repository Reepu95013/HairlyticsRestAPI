using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class SubCategory
    {
        public int Id { get; set; }
        public  required string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }       
        public int? VendorProfileId { get; set; }
        public bool IsGlobal { get; set; } 
        public bool IsActive { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();



    }
}
