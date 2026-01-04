using AutoMapper;
using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.Services
{
    public class CategoryService : ICategoryService
    {  
        private readonly ICategoryRepository _CategoryRepository;
        private readonly IMapper _mapper;
        public CategoryService( ICategoryRepository categoryRepository, IMapper mapper) {
            _CategoryRepository = categoryRepository;
            _mapper = mapper;
        
        }
        public async Task AddCategory(CreateCategoryDto dto)
        {
            var data = _mapper.Map<Category>(dto);

            data.IsDeleted = false;
            data.CreateAt = DateTime.Now;
            data.UpdateAt = DateTime.Now;
            if (data.VendorProfileId != 0)
            {
                data.IsGlobal = false;
            }
            else
            {
                data.IsGlobal = true;
            }

            await _CategoryRepository.AddCategory(data);
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
           return await _CategoryRepository.DeleteCategory(categoryId);
        }

        public async Task<IEnumerable<ResponseCategoryDto>> GetCategories()
        {
            return _mapper.Map<IEnumerable<ResponseCategoryDto>>(
                    await _CategoryRepository.GetCategories()
            );
        }

        public async Task<IEnumerable<ResponseCategoryDto>> GetCategoriesByVendor(int vendorId)
        {
            return _mapper.Map<IEnumerable<ResponseCategoryDto>>(
                    await _CategoryRepository.GetCategoriesByVendor(vendorId)
            );
        }

        public  async Task<IEnumerable<ResponseCategoryDto>> GetCategoriesByVendorAndDefault(int vendorId)
        {
            return _mapper.Map<IEnumerable<ResponseCategoryDto>>(
                   await _CategoryRepository.GetCategoriesByVendorAndDefault(vendorId)
           );
        }
    }
}

