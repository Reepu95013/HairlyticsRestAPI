using AutoMapper;
using Hairlytics.Application.DTOs.BookingDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.ServiceDTOs;
using Hairlytics.Application.DTOs.UserDTOs;
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
using static System.Reflection.Metadata.BlobBuilder;

namespace Hairlytics.Application.Services
{
    public class VendorStaffService : IVendorStaffService
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IVendorStaffRepository _vendorStaffRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IBookingRepository _bookingRepository;

        public VendorStaffService(IFileService fileService, IMapper mapper, IVendorStaffRepository vendorStaffRepository, IServiceRepository serviceRepository, IBookingRepository bookingRepository)
        {
            _fileService = fileService;
            _mapper = mapper;
            _vendorStaffRepository = vendorStaffRepository;
            _serviceRepository = serviceRepository;
            _bookingRepository = bookingRepository;
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

        public async Task<ServiceResponse<List<TimeSlotDto>>> GetAvailableSlots(StaffAvailabilitySlotDto dto)
        {
            var response = new ServiceResponse<List<TimeSlotDto>>();

            // 1. check valid date
            if (dto.Date < DateOnly.FromDateTime(DateTime.Today))
            {
                response.Success = false;
                response.Data = []; // empty list
                response.Message = "Cannot book past date";
                return response;
            }


            // 1. Get staff
            var staff = await _vendorStaffRepository.GetVendorStafDetails(dto.StaffId);

            if (staff == null)
            {
                response.Success = false;
                response.Message = "Staff not found";
                return response;
            }

            // 2. Get services
            var services = await _serviceRepository.GetServicesByIdsAsync(dto.ServiceIds);

            if (services == null || !services.Any())
            {
                response.Success = false;
                response.Message = "Invalid services";
                return response;
            }

            // 3. Calculate total duration
            var totalDuration = services.Sum(s => s.Duration);
            var slotDuration = TimeSpan.FromMinutes(totalDuration);

            // 4. Default working hours
            var startTime = staff.StartTime;
            var endTime = staff.EndTime;

            // 5. Check custom availability
            var dayAvailability = staff.StaffAvailabilities
                .FirstOrDefault(x => x.DayOfWeek == dto.Date.DayOfWeek);

            if (dayAvailability != null)
            {
                if (dayAvailability.IsOffDay)
                {
                    response.Success = true;
                    response.Data = []; // empty slots
                    response.Message = "Staff is off on this day";
                    return response;
                }

                startTime = dayAvailability.StartTime;
                endTime = dayAvailability.EndTime;
            }

            // 6. Get bookings
            var bookings = await _bookingRepository
                .GetBookings(dto.StaffId, dto.Date);

            // 7. Generate slots
            var slots = new List<TimeSlotDto>();
            var current = startTime;


            while (current + slotDuration <= endTime)
            {
                var slotEnd = current + slotDuration;

                // 🔥 skip past time (today only)
                if (dto.Date == DateOnly.FromDateTime(DateTime.Today))
                {
                    var now = DateTime.Now.TimeOfDay;

                    if (current < now)
                    {
                        current = current.Add(slotDuration);
                        continue;
                    }
                }

                var isConflict = bookings.Any(b =>
                    current < b.EndTime &&
                    slotEnd > b.StartTime
                );

                if (!isConflict)
                {
                    slots.Add(new TimeSlotDto
                    {
                        StartTime = current,
                        EndTime = slotEnd
                    });
                }

                current = current.Add(slotDuration);
            }
            // 8. Assign response
            response.Success = true;
            response.Data = slots;
            response.Message = "Available slots fetched";

            return response;
        }

        public async Task<ServiceResponse<List<VendorStaffResponseDto>>> GetVendorStaffs(int vendorId)
        {
            var response = new ServiceResponse<List<VendorStaffResponseDto>>();
            var  staffs = await _vendorStaffRepository.GetVendorStaffsAsync(vendorId);
            var data = _mapper.Map<List<VendorStaffResponseDto>>(staffs);

            if (data.Count==0)
            {
                response.Success = false;
                response.Data = [];
                response.Message = "Data not found";
            }else
            {
                response.Success = true;
                response.Data = data;
                response.Message = "Success!";
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
