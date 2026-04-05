using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.CategoryDTOs
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }

        public string? CategoryName { get; set; }

        public string Image { get; set; }

        public string? Description { get; set; } 

        public int? ParentCategoryId { get; set; }

        public List<CategoryResponseDto> SubCategories { get; set; } = new();
    }
}
