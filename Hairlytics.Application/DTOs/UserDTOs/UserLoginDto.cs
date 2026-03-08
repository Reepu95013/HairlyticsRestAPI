using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.UserDTOs
{
    public class UserLoginDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public UserRole Role { get; set; }
    }
}
