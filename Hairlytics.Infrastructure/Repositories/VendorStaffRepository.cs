using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.Repositories
{
    public class VendorStaffRepository : IVendorStaffRepository
    {
        private readonly ApplicationDbContext _context;
        public VendorStaffRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateVendorStaff(VendorStaff vendorStaff)
        {
            var vendorExists = await _context.VendorProfiles
            .AnyAsync(v => v.Id == vendorStaff.VendorProfileId && v.Status);

            if (!vendorExists)
                throw new Exception("Invalid VendorProfileId");

            await _context.VendorStaff.AddAsync(vendorStaff);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StaffAvailability>> GetStaffAvailability(int staffId)
        {
            return await _context.StaffAvailability
           .Where(a => a.StaffId == staffId)
           .ToListAsync();
        }

        public async Task AddAvailabilityAsync(StaffAvailability availability)
        {
            await _context.StaffAvailability.AddAsync(availability);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<VendorStaff> GetVendorStafDetails(int staffId)
        {
            var staff = await _context.VendorStaff
              .Include(s => s.StaffAvailabilities)
              .FirstOrDefaultAsync(s => s.Id == staffId && s.IsActive);

            if (staff == null)
                throw new Exception("Not Found!");

            return staff;
        }

        public async Task<List<VendorStaff>> GetVendorStaffsAsync(int vendorId)
        {
            return await _context.VendorStaff
            .Include(s => s.StaffAvailabilities)
            .Where(s => s.IsActive)
            .ToListAsync();
        }
    }
}
