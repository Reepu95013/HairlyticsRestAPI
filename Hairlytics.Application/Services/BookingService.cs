using AutoMapper;
using Hairlytics.Application.DTOs.BookingDTOs;
using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.PaymentDTOs;
using Hairlytics.Application.DTOs.RazorpayDTOs;
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
        private readonly IRazorpayService _razorpayService;
        private readonly IGlobalRepository _globalRepository;

        public BookingService(IBookingRepository bookingRepository, IVendorStaffRepository vendorStaffRepository, IServiceRepository serviceRepository, IMapper mapper, IUserRepository userRepository, IPaymentRepository paymentRepository, IRazorpayService razorpayService, IGlobalRepository globalRepository)
        {
            _bookingRepository = bookingRepository;
            _vendorStaffRepository = vendorStaffRepository;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _razorpayService = razorpayService;
            _globalRepository = globalRepository;
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
            
            var payment = new Payment();
            payment.TotalAmount = totalAmount;
            payment.BookingId = bookingData.Id;
            payment.UpdatedAt = DateTime.Now;
            payment.CreatedAt = DateTime.Now;
            payment.PaymentGateway = PaymentGateway.None;


            var onlinePayment = new OnlinePaymentResponseDto();
            onlinePayment.BookingId = bookingData.Id;
            onlinePayment.TotalAmount = totalAmount;

            if (bookingCreateDto.PaymentMethod == PaymentMethod.Cash) {               
                payment.PaymentMethod = PaymentMethod.Cash;              
                payment.Status = PaymentTransactionStatus.None;
                response.Data = onlinePayment;

            }
            else
            {
                payment.PaymentMethod = PaymentMethod.Online;
                payment.Status = PaymentTransactionStatus.Pending;
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

        public async Task<ServiceResponse<RazorpayCreateOrderResponse>> CreatePaymentOrder(int bookingId, PaymentGateway paymentGateway)
        {
            var response = new ServiceResponse<RazorpayCreateOrderResponse>();

            try
            {
                // 1. Get payment by bookingId
                var payment = await _paymentRepository.GetPaymentByBookingIdAsync(bookingId);                

                if (payment == null)
                {
                    response.Success = false;
                    response.Message = "Payment not found for this booking.";
                    return response;
                }

                if(payment.PaymentMethod == PaymentMethod.Cash)
                {
                    response.Success = false;
                    response.Message = "Booking Id is wrong! please check!";
                    return response;
                }

                // 2. Update selected gateway
                payment.PaymentGateway = paymentGateway;
                payment.UpdatedAt = DateTime.Now;

                var orderResponse = new RazorpayCreateOrderResponse();               

                // 3. Create order based on gateway
                if (paymentGateway == PaymentGateway.Razorpay)
                {
                    var order = await _razorpayService.CreateOrder(payment.TotalAmount);

                    orderResponse.OrderId = order.OrderId;
                    orderResponse.Currency = order.Currency;
                    orderResponse.Amount = order.Amount;
                    orderResponse.Receipt = order.Receipt;
                    orderResponse.key = order.key; 

                   
                }
                else if (paymentGateway == PaymentGateway.Stripe)
                {
                    // Future implementation
                    throw new Exception("Stripe not implemented yet.");
                }
                else if (paymentGateway == PaymentGateway.Paytm)
                {
                    // Future implementation
                    throw new Exception("Paytm not implemented yet.");
                }

                // 4. Save payment update
                await _paymentRepository.UpdatePaymentAsync(payment);

                // 5. Return response
                response.Success = true;
                response.Message = "Payment order created successfully.";
                response.Data = orderResponse;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = null;
                return response;
            }
        }

        public async Task<ServiceResponse<List<BookingResponseDto>>> GetBookingList(PaginationDto paginationDto)
        {
            var response =new ServiceResponse<List<BookingResponseDto>>();

            var bookings  =   await _bookingRepository.GetAllBookingAsync(paginationDto.PageNumber, paginationDto.PageSize);

            var bookingList  =    _mapper.Map<List<BookingResponseDto>>(bookings);

            response.Success = true;
            response.Message = "Booking List!";
            response.Data = bookingList;

            return response;           
            
        }


        public async Task<ServiceResponse<List<BookingResponseDto>>> GetCancelledBookingList(PaginationDto paginationDto)
        {
            var response = new ServiceResponse<List<BookingResponseDto>>();

            var bookings = await _bookingRepository.GetAllCancelledBookingAsync(paginationDto.PageNumber, paginationDto.PageSize);

            var bookingList = _mapper.Map<List<BookingResponseDto>>(bookings);

            response.Success = true;
            response.Message = "Cancelled Booking List!";
            response.Data = bookingList;

            return response;

        }


        public async Task<ServiceResponse<List<BookingResponseDto>>> GetBookingListByVendor(PaginationDto paginationDto, int vendorId)
        {
            var response = new ServiceResponse<List<BookingResponseDto>>();

            var bookings = await _bookingRepository.GetAllBookingByVendorAsync(paginationDto.PageNumber, paginationDto.PageSize, vendorId);

            var bookingList = _mapper.Map<List<BookingResponseDto>>(bookings);

            response.Success = true;
            response.Message = " Booking List!";
            response.Data = bookingList;

            return response;

        }
        public async Task<ServiceResponse<List<BookingResponseDto>>> GetBookingListByStaff(PaginationDto paginationDto, int staffId)
        {
            var response = new ServiceResponse<List<BookingResponseDto>>();

            var bookings = await _bookingRepository.GetAllBookingByStaffAsync(paginationDto.PageNumber, paginationDto.PageSize, staffId);

            var bookingList = _mapper.Map<List<BookingResponseDto>>(bookings);

            response.Success = true;
            response.Message = " Booking List!";
            response.Data = bookingList;

            return response;

        }
        public async Task<ServiceResponse<List<BookingResponseDto>>> GetBookingListByUser(PaginationDto paginationDto, int userId)
        {
            var response = new ServiceResponse<List<BookingResponseDto>>();

            var bookings = await _bookingRepository.GetAllBookingByUserAsync(paginationDto.PageNumber, paginationDto.PageSize, userId);

            var bookingList = _mapper.Map<List<BookingResponseDto>>(bookings);

            response.Success = true;
            response.Message = " Booking List!";
            response.Data = bookingList;

            return response;

        }

        public async Task<ServiceResponse<string>> CancelBooking(int bookingId)
        {
            var response = new ServiceResponse<string>();
            try
            {
               var booking =  await _bookingRepository.GetBookingDetailByBookingIdAsync(bookingId);
                booking.Status = BookingStatus.Cancelled;
                booking.UpdatedAt = DateTime.Now;

                if (booking.PaymentMethod == PaymentMethod.Online)
                {
                    PaymentRefund();
                }

              await _globalRepository.SaveDbContextAsync();

                response.Success = true;
                response.Message = "Booking cancelled successfully";
                response.Data = booking.Id.ToString();
                
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = ex.Message;

            }

            return response;

        }


        private void PaymentRefund()
        {

        }


    }
}
