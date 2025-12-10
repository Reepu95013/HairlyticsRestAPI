using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.UserDTOs
{
    public class UserUpdateDto
    {
        public required string Name { get; set; }
        public string? LastName { get; set; }
        public DateOnly? Birth { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }
    }
}
