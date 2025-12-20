using Hairlytics.Application.DTOs.VendorProfileDTOs;
using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.DTOs.UserDTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? LastName { get; set; }
        public DateOnly? Birth { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        //navigational
        public VendorProfileResponseDto? VendorProfileResponseDto { get; set; }
    }
}
