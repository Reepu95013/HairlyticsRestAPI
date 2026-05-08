using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.RazorpayDTOs;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Enums;
using Hairlytics.Domain.Interfaces;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.Services
{
    public class RazorpayService : IRazorpayService
    {

        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly string _api_key = "rzp_test_Sm728if14fWfvW";
        private readonly string _secret_key = "2XBA2ZhG2OOdKIRwML8TJ0y0";

        public RazorpayService(IPaymentRepository paymentRepository, IBookingRepository bookingRepository)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
        }


        public Task<RazorpayCreateOrderResponse> CreateOrder(decimal amount)
        {
            var client = new RazorpayClient(_api_key, _secret_key);

            var options = new Dictionary<string, object>
            {
                { "amount", amount * 100 }, // convert ₹ to paise
                { "currency", "INR" },
                { "receipt", Guid.NewGuid().ToString() },
                { "payment_capture", 1 } // auto capture
            };

            Order order = client.Order.Create(options);

            var result = new RazorpayCreateOrderResponse
            {
                OrderId = order["id"].ToString(),
                Amount = Convert.ToDecimal(order["amount"]) / 100, // back to ₹
                Currency = order["currency"].ToString(),
                Receipt = order["receipt"].ToString(),
                key = _api_key

            };

            return Task.FromResult(result);
        }

        public async Task<ServiceResponse<string>> VerifyPayment(VerifyPaymentDto dto)
        {
            var response = new ServiceResponse<string>();

            try
            {
                string generatedSignature = GenerateSignature(
                    dto.OrderId,
                    dto.PaymentId
                );

                if (generatedSignature != dto.Signature)
                {
                    response.Success = false;
                    response.Message = "Invalid payment signature.";

                    return response;
                }

                var payment = await _paymentRepository
                    .GetPaymentByOrderIdAsync(dto.OrderId);

                if (payment == null)
                {
                    response.Success = false;
                    response.Message = "Payment not found.";

                    return response;
                }

                payment.TransactionId = dto.PaymentId;
                payment.Signature = dto.Signature;
                payment.Status = PaymentTransactionStatus.Success;
                payment.UpdatedAt = DateTime.Now;

               

                var booking = await _bookingRepository.GetBookingDetailByBookingIdAsync(payment.BookingId);
                booking.PaymentStatus = PaymentStatus.Paid;
                booking.Status = BookingStatus.Completed;
                booking.UpdatedAt = DateTime.Now;

                await _paymentRepository.UpdatePaymentAsync(payment);
                await _bookingRepository.UpdateBookAsync(booking);    

                response.Success = true;
                response.Message = "Payment verified successfully.";
                response.Data = payment.BookingId.ToString();
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

                return response;
            }
        }


        private string GenerateSignature(string orderId, string paymentId)
        {
            string payload = $"{orderId}|{paymentId}";

            var secret = Encoding.UTF8.GetBytes(_secret_key);

            using var hmac = new HMACSHA256(secret);

            var hash = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(payload)
            );

            return BitConverter
                .ToString(hash)
                .Replace("-", "")
                .ToLower();
        }
    }
}
