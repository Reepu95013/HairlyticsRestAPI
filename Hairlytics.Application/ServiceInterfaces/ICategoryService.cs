using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface ICategoryService
    {
        Task AddCategory(CreateCategoryDto dto);
        Task<IEnumerable<ResponseCategoryDto>> GetCategories();
        Task<IEnumerable<ResponseCategoryDto>> GetCategoriesByVendor(int vendorId);
        Task<IEnumerable<ResponseCategoryDto>> GetCategoriesByVendorAndDefault(int vendorId);
        Task<bool> DeleteCategory(int categoryId);
    }
}
