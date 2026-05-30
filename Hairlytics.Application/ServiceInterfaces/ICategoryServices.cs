using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface ICategoryServices
    {
        Task<ServiceResponse<string>> AddCategoryAsync(CategoryCreateDto categoryCreateDto);
        Task<ServiceResponse<string>> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
        Task<ServiceResponse<List<CategoryResponseDto>>>GetCategoryListAsync();
        Task<ServiceResponse<PagedResultDto<CategoryResponseDto>>> GetCategoryListPagedAsync(int pageNumber, int pageSize);
        Task<ServiceResponse<CategoryResponseDto>> GetCategoryAsync(int categoryId);
        Task<ServiceResponse<string>> DeleteCategoryAsync(int categoryId);
    }
}
