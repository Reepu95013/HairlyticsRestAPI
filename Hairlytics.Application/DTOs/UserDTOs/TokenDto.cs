using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.UserDTOs
{
    public class TokenDto
    {   public required int UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
