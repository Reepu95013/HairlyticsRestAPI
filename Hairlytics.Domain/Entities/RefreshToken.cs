using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }

    }
}
