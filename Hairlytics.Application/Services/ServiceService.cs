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
                var servie = _mapper.Map<Service>(serviceCreateDto);
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

        public async Task<ServiceResponse<ServiceResponseDto>> GetServiceAsync(int serviceId)
        {
            var response = new ServiceResponse<ServiceResponseDto>();
            try
            {
                var service = await _serviceRepository.GetServiceByIdAsync(serviceId);
                if (service == null)
                {
                    response.Success = false;
                    response.Message = "Service not found.";
                    return response;
                }

                var data = _mapper.Map<ServiceResponseDto>(service);
                data.Image = _fileService.GetImage(data.Image);
                response.Success = true;
                response.Message = "Success";
                response.Data = data;
            }
            catch (Exception ex)
            {
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

        public async Task<ServiceResponse<List<ServiceResponseDto>>> GetAllServicesAsync()
        {
            var response = new ServiceResponse<List<ServiceResponseDto>>();
            var services = await _serviceRepository.GetAllServicesAsync();
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

        public async Task<ServiceResponse<string>> UpdateServiceAsync(ServiceUpdateDto serviceUpdateDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = await _serviceRepository.GetServiceByIdAsync(serviceUpdateDto.Id);
                if (existing == null)
                {
                    response.Success = false;
                    response.Message = "Service not found.";
                    return response;
                }

                if (serviceUpdateDto.ImageFile != null)
                {
                    if (!string.IsNullOrWhiteSpace(existing.Image))
                    {
                        _fileService.DeleteFile(existing.Image);
                    }
                    serviceUpdateDto.Image = await _fileService.SaveImage(serviceUpdateDto.ImageFile, FolderNames.Services);
                }

                _mapper.Map(serviceUpdateDto, existing);

                if (!string.IsNullOrWhiteSpace(serviceUpdateDto.Image))
                {
                    existing.Image = serviceUpdateDto.Image;
                }

                await _serviceRepository.UpdateServiceAsync(existing);
                response.Success = true;
                response.Message = "Service updated successfully!";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteServiceAsync(int serviceId)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var existing = await _serviceRepository.GetServiceByIdAsync(serviceId);
                if (existing == null)
                {
                    response.Success = false;
                    response.Message = "Service not found.";
                    return response;
                }

                existing.Status = false;
                existing.UpdatedAt = DateTime.Now;
                await _serviceRepository.UpdateServiceAsync(existing);
                response.Success = true;
                response.Message = "Service deactivated successfully!";
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
