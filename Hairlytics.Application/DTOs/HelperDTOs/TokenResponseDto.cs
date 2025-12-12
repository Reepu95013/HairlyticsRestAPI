using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.TokenDTOs
{
    public class TokenResponseDto
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
