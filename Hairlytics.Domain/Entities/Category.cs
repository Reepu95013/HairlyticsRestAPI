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
        public required string CategoryName { get; set; }
        public required string Image { get; set; }
        public int? ParentCategoryId { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }

        // 🔗 Navigation
        public User? User { get; set; }

        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    }
}
