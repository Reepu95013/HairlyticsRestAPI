using AutoMapper;
using Hairlytics.Application.DTOs.CategoryDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Hairlytics.Domain.Interfaces;

namespace Hairlytics.Application.Services
{
    public class CategoryService : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IFileService fileService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ServiceResponse<string>> AddCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var imagePath = await _fileService.SaveImage(categoryCreateDto.file, FolderNames.Category);
                categoryCreateDto.Image = imagePath;
                var category = _mapper.Map<Domain.Entities.Category>(categoryCreateDto);
                await _categoryRepository.AddCategory(category);
                response.Success = true;
                response.Message = "Category has been created successfully!";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<string>> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = await _categoryRepository.GetCategory(categoryUpdateDto.Id);
                if (existing == null)
                {
                    response.Success = false;
                    response.Message = "Category not found.";
                    return response;
                }

                if (categoryUpdateDto.file != null)
                {
                    if (!string.IsNullOrWhiteSpace(existing.Image))
                    {
                        _fileService.DeleteFile(existing.Image);
                    }
                    categoryUpdateDto.Image = await _fileService.SaveImage(categoryUpdateDto.file, FolderNames.Category);
                }

                _mapper.Map(categoryUpdateDto, existing);

                if (!string.IsNullOrWhiteSpace(categoryUpdateDto.Image))
                {
                    existing.Image = categoryUpdateDto.Image;
                }

                await _categoryRepository.UpdateCategory(existing);
                response.Success = true;
                response.Message = "Category updated successfully!";
            }
            catch (Exception ex)
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
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<CategoryResponseDto>> GetCategoryAsync(int categoryId)
        {
            var response = new ServiceResponse<CategoryResponseDto>();
            var data = await _categoryRepository.GetCategory(categoryId);

            if (data == null)
            {
                response.Success = false;
                response.Message = "Category not found";
                return response;
            }

            var category = _mapper.Map<CategoryResponseDto>(data);
            category.Image = _fileService.GetCategoryImage(category.Image);

            foreach (var item in category.SubCategories)
            {
                item.Image = _fileService.GetCategoryImage(item.Image);
            }

            response.Success = true;
            response.Message = "Success";
            response.Data = category;
            return response;
        }

        public async Task<ServiceResponse<List<CategoryResponseDto>>> GetCategoryListAsync()
        {
            var response = new ServiceResponse<List<CategoryResponseDto>>();
            var data = await _categoryRepository.GetCategoryList();
            var categories = _mapper.Map<List<CategoryResponseDto>>(data);

            foreach (var item in categories)
            {
                item.Image = _fileService.GetCategoryImage(item.Image);
            }

            response.Success = true;
            response.Message = "Success";
            response.Data = categories;
            return response;
        }

        public async Task<ServiceResponse<PagedResultDto<CategoryResponseDto>>> GetCategoryListPagedAsync(int pageNumber, int pageSize)
        {
            var response = new ServiceResponse<PagedResultDto<CategoryResponseDto>>();
            var total = await _categoryRepository.GetCategoryCountAsync();
            var data = await _categoryRepository.GetCategoryList(pageNumber, pageSize);
            var categories = _mapper.Map<List<CategoryResponseDto>>(data);

            foreach (var item in categories)
            {
                item.Image = _fileService.GetCategoryImage(item.Image);
            }

            response.Success = true;
            response.Message = "Success";
            response.Data = new PagedResultDto<CategoryResponseDto>
            {
                Items = categories,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total
            };
            return response;
        }
    }
}
