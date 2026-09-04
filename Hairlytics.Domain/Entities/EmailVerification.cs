using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class EmailVerification
    {
        public int Id { get; set; }

        public required string Email { get; set; }

        public required string Otp { get; set; } 

        public DateTime ExpiryTime { get; set; }

        public bool IsVerified { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
