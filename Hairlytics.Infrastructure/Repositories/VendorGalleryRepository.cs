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
            // Step 1: Check if incoming image is marked as thumbnail
            if (vendorGallery.IsThumbnail)
            {
                // Step 2: Find existing thumbnails for same vendor
                var existingThumbnails = await _context.VendorGallery
                    .Where(x => x.VendorProfileId == vendorGallery.VendorProfileId && x.IsThumbnail)
                    .ToListAsync();

                // Step 3: Set old thumbnails to false
                foreach (var item in existingThumbnails)
                {
                    item.IsThumbnail = false;
                    item.UpdatedAt = DateTime.Now;
                }
            }
            else
            {
                // Step 4: If no thumbnail exists at all → make this one thumbnail
                bool hasThumbnail = await _context.VendorGallery
                    .AnyAsync(x => x.VendorProfileId == vendorGallery.VendorProfileId && x.IsThumbnail);

                if (!hasThumbnail)
                {
                    vendorGallery.IsThumbnail = true;
                }
            }

            // Step 5: Add new image
           
            await _context.VendorGallery.AddAsync(vendorGallery);                
            await _context.SaveChangesAsync();
        }

        public async Task<List<VendorGallery>> GetByVendorIdAsync(int vendorId)
        {
            return await _context.VendorGallery.Where(x => x.VendorProfileId == vendorId && x.IsActive).ToListAsync();
        }
    }
}
