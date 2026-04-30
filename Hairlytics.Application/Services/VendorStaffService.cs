using AutoMapper;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.ServiceDTOs;
using Hairlytics.Application.DTOs.VendorStaffDTOs;
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
    public class VendorStaffService : IVendorStaffService
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IVendorStaffRepository _vendorStaffRepository;

        public VendorStaffService(IFileService fileService, IMapper mapper, IVendorStaffRepository vendorStaffRepository)
        {
            _fileService = fileService;
            _mapper = mapper;
            _vendorStaffRepository = vendorStaffRepository;
        }

        public async Task<ServiceResponse<string>> AddVendorStafAvailabilityAsync(int staffId, List<StaffAvailabilityCreateDto> StaffAvailabilityCreateDtos)
        {
            var response = new ServiceResponse<string>();
            var existingAvailabilities = await _vendorStaffRepository.GetStaffAvailability(staffId);


            foreach (var dto in StaffAvailabilityCreateDtos)
            {
                // 🔹 validation
                if (!dto.IsOffDay && dto.StartTime >= dto.EndTime)
                {
                    response.Success = false;
                    response.Message = $"Invalid time for {dto.DayOfWeek}";
                    return response;
                }

                var existing = existingAvailabilities
                    .FirstOrDefault(a => a.DayOfWeek == dto.DayOfWeek);

                if (existing != null)
                {
                    // 🔥 UPDATE
                    existing.StartTime = dto.IsOffDay ? TimeSpan.Zero : dto.StartTime;
                    existing.EndTime = dto.IsOffDay ? TimeSpan.Zero : dto.EndTime;
                    existing.IsOffDay = dto.IsOffDay;
                }
                else
                {
                    // 🔥 INSERT
                    var availability = _mapper.Map<StaffAvailability>(dto);
                    availability.StaffId = staffId;

                    if (dto.IsOffDay)
                    {
                        availability.StartTime = TimeSpan.Zero;
                        availability.EndTime = TimeSpan.Zero;
                    }

                    await _vendorStaffRepository.AddAvailabilityAsync(availability);
                }
            }
            await _vendorStaffRepository.SaveChangesAsync();

            response.Success = true;
            response.Message = "Availability saved (added + updated)";
            return response;

        }

        public async Task<ServiceResponse<string>> CreateVendorStafAsync(VendorStaffCreateDto vendorStaffCreateDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var image = await _fileService.SaveImage(vendorStaffCreateDto.ProfileImage, FolderNames.Staff);
                
                var vendorStaff = _mapper.Map<VendorStaff>(vendorStaffCreateDto);
                vendorStaff.ProfileImageUrl = image;
                await _vendorStaffRepository.CreateVendorStaff(vendorStaff);
                response.Success = true;
                response.Message = "Service added successfully!";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<VendorStaffResponseDto>> GetVendorStafDetailsAsync(int staffId)
        {
            var response = new ServiceResponse<VendorStaffResponseDto>();

           var staff =  await _vendorStaffRepository.GetVendorStafDetails(staffId);
           var vendorStaff = _mapper.Map<VendorStaffResponseDto>(staff);

            try
            {

                response.Success = true;
                response.Message = "Succes!";
                response.Data = vendorStaff;

                return response;

            }
            catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = null;

                return response;
            }

        }
    }
}
