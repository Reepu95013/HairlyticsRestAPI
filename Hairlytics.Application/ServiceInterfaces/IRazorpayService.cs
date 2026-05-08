using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.RazorpayDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IRazorpayService
    {
        Task<RazorpayCreateOrderResponse> CreateOrder(decimal amount);
        Task<ServiceResponse<string>> VerifyPayment(VerifyPaymentDto verifyPaymentDto);
    }


}
