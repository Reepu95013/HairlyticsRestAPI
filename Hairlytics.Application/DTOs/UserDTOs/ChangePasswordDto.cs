using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.UserDTOs
{
    public class ChangePasswordDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
