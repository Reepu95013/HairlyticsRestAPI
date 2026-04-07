using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.Repositories
{
    public class VendorGalleryRepository : IVendorGalleryRepository
    {
        private readonly ApplicationDbContext _context;
        public VendorGalleryRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task AddVendorGallery(VendorGallery vendorGallery)
        {
            await _context.VendorGallery.AddAsync(vendorGallery);                
            await _context.SaveChangesAsync();
        }

        public async Task<List<VendorGallery>> GetByVendorIdAsync(int vendorId)
        {
            return await _context.VendorGallery.Where(x => x.VendorProfileId == vendorId).ToListAsync();
        }
    }
}
