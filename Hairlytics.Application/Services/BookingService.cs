using AutoMapper;
using Hairlytics.Application.DTOs.BookingDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.PaymentDTOs;
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
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IVendorStaffRepository _vendorStaffRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;

        public BookingService(IBookingRepository bookingRepository, IVendorStaffRepository vendorStaffRepository, IServiceRepository serviceRepository, IMapper mapper, IUserRepository userRepository, IPaymentRepository paymentRepository)
        {
            _bookingRepository = bookingRepository;
            _vendorStaffRepository = vendorStaffRepository;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<OnlinePaymentResponseDto>> CreateBooking(BookingCreateDto bookingCreateDto)
        {
            var response = new ServiceResponse<OnlinePaymentResponseDto>();

            //0. check vendor exit or not

           var isVendor =  await _userRepository.IsVendorActive(bookingCreateDto.VendorProfileId);

            if (isVendor == false)
            {
                response.Success = false;
                response.Message = "Vendor is not active right now!";
                //response.Data = "Failed!";
                return response;

            }


            // 1. Validate Services
            if (bookingCreateDto.ServiceIds == null || !bookingCreateDto.ServiceIds.Any())
            {
                response.Success = false;
                response.Message = "At least one service is required.";
                //response.Data = "Failed!";
                return response;
            }

            // 2. Get staff
            var staff = await _vendorStaffRepository.GetVendorStafDetails(bookingCreateDto.VendorStaffId);
            if (staff == null)
            {
                response.Success = false;
                response.Message = "Invalid staff selected.";
                //response.Data = "Failed!";
                return response;
            }

            // 3. Check appointment date
            if (bookingCreateDto.AppointmentDate < DateOnly.FromDateTime(DateTime.Today))
            {
                response.Success = false;
                response.Message = "Past date booking is not allowed.";
                //response.Data = "Failed!";
                return response;
            }

            // 4. Get selected services
            var services = await _serviceRepository.GetServicesByIdsAsync(bookingCreateDto.ServiceIds);

            if (services.Count != bookingCreateDto.ServiceIds.Count)
            {
                response.Success = false;
                response.Message = "One or more services are invalid.";
                //response.Data = "Failed!";
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


            var payment = new Payment();

            payment.TotalAmount = totalAmount;
            payment.BookingId = bookingData.Id;
            payment.UpdatedAt = DateTime.Now;
            payment.CreatedAt = DateTime.Now;
            payment.PaymentGateway = PaymentGateway.None;

            if (bookingCreateDto.PaymentMethod == PaymentMethod.Cash) {               
                payment.PaymentMethod = PaymentMethod.Cash;              
                payment.Status = PaymentTransactionStatus.None;
                //response.Data = "Success!";

            }
            else
            {
                payment.PaymentMethod = PaymentMethod.Online;
                payment.Status = PaymentTransactionStatus.Pending;

                var onlinePayment = new OnlinePaymentResponseDto();
                onlinePayment.BookingId = bookingData.Id;
                onlinePayment.TotalAmount = totalAmount;

                onlinePayment.Gateways = GetPaymentGateways();

                response.Data = onlinePayment;
            }

            await _paymentRepository.AddPaymentAsync(payment);

            response.Success = true;
            response.Message = "Booking created successfully.";
          
            return response;
        }


        public List<PaymentGatewayOptionDto> GetPaymentGateways()
        {
            return Enum.GetValues(typeof(PaymentGateway))
                .Cast<PaymentGateway>()
                .Where(g => g != PaymentGateway.None) // skip None
                .Select(g => new PaymentGatewayOptionDto
                {
                    Name = GetDisplayName(g),          // Friendly name
                    Value = g,              // Razorpay, Stripe
                    Icon = GetGatewayIcon(g)           // Icon mapping
                })
                .ToList();
        }


        private string GetDisplayName(PaymentGateway gateway)
        {
            return gateway switch
            {
                PaymentGateway.Razorpay => "Razorpay",
                PaymentGateway.Stripe => "Stripe",
                PaymentGateway.Paytm => "Paytm",
                _ => gateway.ToString()
            };
        }


        private static readonly Dictionary<PaymentGateway, string> GatewayIcons = new()
        {
                { PaymentGateway.Razorpay, "https://yourcdn.com/icons/razorpay.png" },
                { PaymentGateway.Stripe, "https://yourcdn.com/icons/stripe.png" },
                { PaymentGateway.Paytm, "https://yourcdn.com/icons/paytm.png" }
        };

        private string GetGatewayIcon(PaymentGateway gateway)
        {
            return GatewayIcons.TryGetValue(gateway, out var icon)
                ? icon
                : "https://yourcdn.com/icons/default.png";
        }

        public Task<ServiceResponse<string>> CreatePaymentOrder(int bookingId, PaymentGateway paymentGateway)
        {
            throw new NotImplementedException();
        }




        
    }
}
