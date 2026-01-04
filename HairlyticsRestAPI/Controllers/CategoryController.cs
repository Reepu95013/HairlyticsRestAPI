using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HairlyticsRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;

        public CategoryController(IUserService userService, ICategoryService categoryService)
        {
            _userService = userService;
            _categoryService = categoryService;
        }

        [Authorize(Roles = $"{nameof(UserRole.Vendor)},{nameof(UserRole.Admin)}")]
        [HttpPost]
        public async Task<IActionResult> AddCategories(CreateCategoryDto dto)
        {
             await _categoryService.AddCategory(dto);

            return Ok("Category Added Successfully!");

        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
           var response = await _categoryService.GetCategories();

            return Ok(response);

        }

        [Authorize(Roles = $"{nameof(UserRole.Vendor)},{nameof(UserRole.Admin)}")]
        [HttpGet("by-vendor/{vendorProfileId}")]
        public async Task<IActionResult> GetCategoriesByVendor(int vendorProfileId)
        {
            var response = await _categoryService.GetCategoriesByVendor(vendorProfileId);
            return Ok(response);
        }

        [Authorize(Roles = $"{nameof(UserRole.Vendor)},{nameof(UserRole.Admin)}")]
        [HttpGet("by-vendor-default/{vendorProfileId}")]
        public async Task<IActionResult> GetCategoriesByVendorAndDefault(int vendorProfileId)
        {
            var response = await _categoryService.GetCategoriesByVendorAndDefault(vendorProfileId);
            return Ok(response);
        }

        [Authorize(Roles = $"{nameof(UserRole.Vendor)},{nameof(UserRole.Admin)}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var response = await _categoryService.DeleteCategory(categoryId);

            return Ok(response);

        }





    }
}
