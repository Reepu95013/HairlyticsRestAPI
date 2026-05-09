using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface ISmsService
    {
        Task SendOtpSms(string phoneNumber, string otp);
    }
}
