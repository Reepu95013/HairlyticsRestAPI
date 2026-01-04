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
    public class CategoryRepository:ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) {
            _context = context;
        
        }

        public async Task AddCategory(Category category)
        {
         
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();

        }

        public  async Task<bool> DeleteCategory(int categoryId)
        {
                    var category = await _context.Category.FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted);
                    
                    if(category == null)
                        {
                            return false;       
                        }
                    category.IsDeleted = true;
                    category.IsActive = false;
                    category.UpdateAt = DateTime.Now;

                    await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
             return await _context.Category.Where(c => !c.IsDeleted && c.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByVendor(int vendorId)
        {
            return await _context.Category.Where(c => !c.IsDeleted && c.IsActive && c.VendorProfileId==vendorId).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByVendorAndDefault(int vendorId)
        {
            return await _context.Category.Where(c => !c.IsDeleted && c.IsActive && (c.IsGlobal || c.VendorProfileId == vendorId)).ToListAsync();
        }
    }
}
