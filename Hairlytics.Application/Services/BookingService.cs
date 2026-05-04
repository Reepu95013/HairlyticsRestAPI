using AutoMapper;
using Hairlytics.Application.DTOs.BookingDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hairlytics.Domain.Enums;

namespace Hairlytics.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IVendorStaffRepository _vendorStaffRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public BookingService(IBookingRepository bookingRepository, IVendorStaffRepository vendorStaffRepository, IServiceRepository serviceRepository, IMapper mapper, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _vendorStaffRepository = vendorStaffRepository;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> CreateBooking(BookingCreateDto bookingCreateDto)
        {
            var response = new ServiceResponse<string>();

            //0. check vendor exit or not

           var isVendor =  await _userRepository.IsVendorActive(bookingCreateDto.VendorProfileId);

            if (isVendor == false)
            {
                response.Success = false;
                response.Message = "Vendor is not active right now!";
                response.Data = "Failed!";
                return response;

            }


            // 1. Validate Services
            if (bookingCreateDto.ServiceIds == null || !bookingCreateDto.ServiceIds.Any())
            {
                response.Success = false;
                response.Message = "At least one service is required.";
                response.Data = "Failed!";
                return response;
            }

            // 2. Get staff
            var staff = await _vendorStaffRepository.GetVendorStafDetails(bookingCreateDto.VendorStaffId);
            if (staff == null)
            {
                response.Success = false;
                response.Message = "Invalid staff selected.";
                response.Data = "Failed!";
                return response;
            }

            // 3. Check appointment date
            if (bookingCreateDto.AppointmentDate < DateOnly.FromDateTime(DateTime.Today))
            {
                response.Success = false;
                response.Message = "Past date booking is not allowed.";
                response.Data = "Failed!";
                return response;
            }

            // 4. Get selected services
            var services = await _serviceRepository.GetServicesByIdsAsync(bookingCreateDto.ServiceIds);

            if (services.Count != bookingCreateDto.ServiceIds.Count)
            {
                response.Success = false;
                response.Message = "One or more services are invalid.";
                response.Data = "Failed!";
                return response;
            }

            // 5. Calculate amount and duration
            var totalAmount = services.Sum(x => x.Price);
            var totalDuration = services.Sum(x => x.Duration);

    
            var bookingData = _mapper.Map<Booking>(bookingCreateDto);
            bookingData.TotalAmount = totalAmount;

            if(bookingData.PaymentMethod == PaymentMethod.Cash)
            {
                bookingData.Status = BookingStatus.Confirmed;
            }

            await _bookingRepository.CreateBookingAsync(bookingData);

            // 8. Save Booking Services
            var bookingServices = bookingCreateDto.ServiceIds.Select(x => new BookedService
            {
                BookingId = bookingData.Id,
                ServiceId = x
            }).ToList();


            await _bookingRepository.AddBookingServicesAsync(bookingServices);

            response.Success = true;
            response.Message = "Booking created successfully.";
            response.Data = "Success!";
            return response;
        }
    }
}
