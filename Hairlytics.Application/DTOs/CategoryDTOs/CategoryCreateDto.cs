using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.CategoryDTOs
{
    public class CategoryCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string? CategoryName { get; set; }

        public string? Image { get; set; }
       
        public required IFormFile file { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Description { get; set; }

        public int? ParentCategoryId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
