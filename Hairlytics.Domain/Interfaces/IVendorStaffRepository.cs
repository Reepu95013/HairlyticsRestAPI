using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Interfaces
{
    public interface IVendorStaffRepository
    {
        Task CreateVendorStaff(VendorStaff vendorStaff);
        Task<VendorStaff> GetVendorStafDetails(int staffId);
        Task<List<StaffAvailability>> GetStaffAvailability(int staffId);
        Task AddAvailabilityAsync(StaffAvailability availability);
        Task SaveChangesAsync();
        Task<List<VendorStaff>> GetVendorStaffsAsync(int vendorId);
    }
}
