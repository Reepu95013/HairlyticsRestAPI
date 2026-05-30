using Microsoft.AspNetCore.Http;

namespace Hairlytics.Application.DTOs.CategoryDTOs
{
    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public IFormFile? file { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
