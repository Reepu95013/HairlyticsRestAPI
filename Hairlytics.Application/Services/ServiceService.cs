using AutoMapper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.ServiceDTOs;
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
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;


        public ServiceService(IServiceRepository serviceRepository, IMapper mapper, IFileService fileService)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ServiceResponse<string>> AddServiceAsync(ServiceCreateDto serviceCreateDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
               var image = await _fileService.SaveImage(serviceCreateDto.ImageFile, FolderNames.Services);

                serviceCreateDto.Image = image;
               var servie  = _mapper.Map<Service>(serviceCreateDto);
               await  _serviceRepository.AddService(servie);

                response.Success = true;
                response.Message = "Service added successfully!";

            }
            catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
           
        }

        public async Task<ServiceResponse<List<ServiceResponseDto>>> GetServiceListAsync(int vendorProfileId)
        {
            var response = new ServiceResponse<List<ServiceResponseDto>>();
            var services = await _serviceRepository.GetServiceList(vendorProfileId);

           var data = _mapper.Map<List<ServiceResponseDto>>(services);

            foreach (var item in data)
            {
                item.Image = _fileService.GetImage(item.Image);
            }

            response.Success = true;
            response.Message = "Success";
            response.Data = data;

            return response;

        }


        public async Task<ServiceResponse<List<ServiceResponseDto>>> GetServiceListAsync()
        {
            var response = new ServiceResponse<List<ServiceResponseDto>>();
            var services = await _serviceRepository.GetServiceList();

            var data = _mapper.Map<List<ServiceResponseDto>>(services);

            foreach (var item in data)
            {
                item.Image = _fileService.GetImage(item.Image);
            }

            response.Success = true;
            response.Message = "Success";
            response.Data = data;

            return response;

        }
    }
}
