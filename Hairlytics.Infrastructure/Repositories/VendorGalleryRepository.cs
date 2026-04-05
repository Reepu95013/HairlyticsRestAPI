using Hairlytics.Domain.Entities;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
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
    }
}
