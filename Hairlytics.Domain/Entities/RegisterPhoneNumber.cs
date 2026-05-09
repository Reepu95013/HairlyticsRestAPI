using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class RegisterPhoneNumber
    {
        public int Id { get; set; }

        public required string PhoneNumber { get; set; }

        public required string OtpCode { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsVerified { get; set; }
    }
}
