using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ApplicationHelper
{
    public class HelperRazorpay
    {
        private readonly IConfiguration _config;
        private readonly string _api_key = "rzp_test_Sm728if14fWfvW";
        private readonly string _secret_key = "2XBA2ZhG2OOdKIRwML8TJ0y0";

        public HelperRazorpay(IConfiguration config, string basePath)
        {
            _config = config;
        }


        public Task<object> CreateRazorpayOrder(decimal amount)
        {
            var client = new RazorpayClient(_api_key, _secret_key);

            var options = new Dictionary<string, object>();

            options.Add("amount", (long)(amount * 100)); // ✅ FIX
            options.Add("currency", "INR");
            options.Add("receipt", Guid.NewGuid().ToString());
            options.Add("payment_capture", 1);

            Order order = client.Order.Create(options);

            return order.Attributes;
        }
    }
}
