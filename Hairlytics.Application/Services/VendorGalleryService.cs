using AutoMapper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.ServiceDTOs;
using Hairlytics.Application.DTOs.VendroGalleryDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using Hairlytics.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.Services
{
    public class VendorGalleryService : IVendorGalleryService
    {
        private readonly IVendorGalleryRepository _vendorGalleryRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;


        public VendorGalleryService(IVendorGalleryRepository vendorGalleryRepository, IMapper mapper, IFileService fileService)
        {
            _vendorGalleryRepository = vendorGalleryRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ServiceResponse<string>> AddVendorGalleryAsync(VendorGalleryCreateDto vendroGalleryCreateDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var image = await _fileService.SaveImage(vendroGalleryCreateDto.ImageFile, FolderNames.VendorGallery);

                vendroGalleryCreateDto.ImageUrl = image;
                var vendorGellery = _mapper.Map<VendorGallery>(vendroGalleryCreateDto);
                await _vendorGalleryRepository.AddVendorGallery(vendorGellery);

                response.Success = true;
                response.Message = "Image added successfully!";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<VendorGalleryResponseDto>>> GetVendorGalleryByVendorIdAsync(int vendorId)
        {
            var response = new ServiceResponse<List<VendorGalleryResponseDto>>();

            try
            {
                var data = await _vendorGalleryRepository.GetByVendorIdAsync(vendorId);

                var mappedData = _mapper.Map<List<VendorGalleryResponseDto>>(data);

                foreach (var item in mappedData)
                {
                    item.ImageUrl = _fileService.GetCategoryImage(item.ImageUrl);
                }


                response.Data = mappedData;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
