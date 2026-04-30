using Hairlytics.Application.DTOs.HelperDTOs;
using Hairlytics.Application.DTOs.VendorStaffDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ServiceInterfaces
{
    public interface IVendorStaffService
    {
        Task<ServiceResponse<string>> CreateVendorStafAsync(VendorStaffCreateDto vendorStaffCreateDto);
        Task<ServiceResponse<VendorStaffResponseDto>> GetVendorStafDetailsAsync(int staffId);
        Task<ServiceResponse<string>> AddVendorStafAvailabilityAsync(int staffId, List<StaffAvailabilityCreateDto> StaffAvailabilityCreateDtos);
    }
}
