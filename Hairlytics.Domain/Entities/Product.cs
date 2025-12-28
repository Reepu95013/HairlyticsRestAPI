using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [MinLength(20, ErrorMessage ="Description must be atleast 20 characters log!")]
        public required string Description { get; set; }
        public int VendorProfileId { get; set; }
        public string? MainImageUrl { get; set; }
        public string? Types { get; set; } = ServiceType.General.ToString();
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }

        // navigational 
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();


    }
}
