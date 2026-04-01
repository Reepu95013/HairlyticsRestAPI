
using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HairlyticsRestAPI.Controllers
{
    
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        // ✅ 1. Create Category / Subcategory
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
        {
            var response = await _categoryServices.AddCategoryAsync(categoryCreateDto);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
            
        }



        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpGet("list")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _categoryServices.GetCategoryListAsync();
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }


        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var response = await _categoryServices.GetCategoryAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }



        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteCategories(int id)
        {
            var response = await _categoryServices.DeleteCategoryAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
    }
}
