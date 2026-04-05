
using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Application.Services;
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
        private readonly IFileService _fileService;
        public CategoryController(ICategoryServices categoryServices, IFileService fileService)
        {
            _categoryServices = categoryServices;
            _fileService = fileService;
        }

        // ✅ 1. Create Category / Subcategory
        //[Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.SubAdmin))]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
        {
            try
            {
                var imagePath = await _fileService.SaveImage(categoryCreateDto.file, FolderNames.Category);
                categoryCreateDto.Image = imagePath;
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
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
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



       [HttpPost("image")]
       public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var imagePath = await _fileService.SaveImage(file, "Category");

                return Ok(new
                {
                    success = true,
                    path = imagePath,
                    message = "Image uploaded successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
       }
    }

}
