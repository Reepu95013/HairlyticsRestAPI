using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddService(Service service)
        {
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Service>> GetServiceList(int vendorProfileId)
        {
            return await _context.Services
                .Include(c => c.Category)
                .Where(x => x.VendorProfileId == vendorProfileId && x.Status).ToListAsync();
        }

        public async Task<List<Service>> GetServiceList()
        {
            return await _context.Services
                .Include(c=>c.Category)
                .Where(x => x.Status).ToListAsync();
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await _context.Services
                .Include(c => c.Category)
                .OrderByDescending(x => x.UpdatedAt)
                .ToListAsync();
        }

        public async Task<Service?> GetServiceByIdAsync(int serviceId)
        {
            return await _context.Services
                .Include(c => c.Category)
                .FirstOrDefaultAsync(x => x.Id == serviceId);
        }

        public async Task UpdateServiceAsync(Service service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Service>> GetServicesByIdsAsync(List<int> serviceIds)
        {
            return await _context.Services
                .Where(x => serviceIds.Contains(x.Id))
                .ToListAsync();
        }


    }
}
