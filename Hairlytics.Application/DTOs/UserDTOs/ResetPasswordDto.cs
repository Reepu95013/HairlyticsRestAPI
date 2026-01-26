using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.UserDTOs
{
    public class ResetPasswordDto
    {
        public required string Email { get; set; }
        public required string OTP { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }

    }
}
