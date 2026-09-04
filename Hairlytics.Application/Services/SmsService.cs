using Hairlytics.Application.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;
using Twilio.Types;

namespace Hairlytics.Application.Services
{
    public class SmsService:ISmsService
    {

        private readonly string accountSid = "ACe04f68b03e55b659ca035d1ff27d538d";
        private readonly string authToken = "11c493135dc2258688d74ffbe254265a";
        private readonly string twilioPhoneNumber = "+17542915154";

        public async Task SendOtpSms(string phoneNumber, string otp)
        {
            try
            {
                // Remove spaces
                phoneNumber = phoneNumber.Replace(" ", "").Trim();

                // Add +91 if missing
                if (!phoneNumber.StartsWith("+91"))
                {
                    phoneNumber = $"+91{phoneNumber}";
                }

                // Initialize Twilio
                TwilioClient.Init(accountSid, authToken);

                // Send SMS
                var message = await MessageResource.CreateAsync(
                    body: $"Your OTP is {otp}",
                    from: new PhoneNumber(twilioPhoneNumber),
                    to: new PhoneNumber(phoneNumber)
                );

                Console.WriteLine($"SMS SID: {message.Sid}");
                Console.WriteLine($"SMS Status: {message.Status}");

               }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

                
            }
        }
    }
}
