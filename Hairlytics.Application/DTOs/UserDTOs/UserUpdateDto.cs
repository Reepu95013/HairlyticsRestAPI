using Hairlytics.Application.DTOs.VendorProfileDTOs;
using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.UserDTOs
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateOnly? Birth { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime UpdatedAt { get; set; }
        public VendorProfileUpdateDto? VendorProfileUpdateDto { get; set; }
    }
}
