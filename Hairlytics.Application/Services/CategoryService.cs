using AutoMapper;
using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.UserDTOs;
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
    public class CategoryService : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> AddCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var category = _mapper.Map<Category>(categoryCreateDto);
                await _categoryRepository.AddCategory(category);
                response.Success = true;
                response.Message = "Category has been created successfuly!";

            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
               
            return response;                       
        }

        public async Task<ServiceResponse<string>> DeleteCategoryAsync(int categoryId)
        {
            var response = new ServiceResponse<string>();
            try
            {
                await _categoryRepository.DeleteCategory(categoryId);

                response.Success = true;
                response.Message = "Category has been deleted successfully!";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message; // or custom message
            }
            return response;
        }

        public async Task<ServiceResponse<CategoryResponseDto>> GetCategoryAsync(int categoryId)
        {
            var response = new ServiceResponse<CategoryResponseDto>();

            var data = await _categoryRepository.GetCategory(categoryId);

           var category = _mapper.Map<CategoryResponseDto>(data);

            if (category == null)
            {
                response.Success = false;
                response.Message = "Data not found";
                response.Data = null;
            }
            else
            {
                response.Success = true;
                response.Message = "Data not found";
                response.Data = category;
            }


            return response;

        }

        public async Task<ServiceResponse<List<CategoryResponseDto>>> GetCategoryListAsync()
        {
            var response = new ServiceResponse<List<CategoryResponseDto>>();

            var data = await _categoryRepository.GetCategoryList();

            var categories = _mapper.Map<List<CategoryResponseDto>>(data);

            response.Success = true;
            response.Message = "Success";
            response.Data= categories;

            return response;

        }
    }
}
