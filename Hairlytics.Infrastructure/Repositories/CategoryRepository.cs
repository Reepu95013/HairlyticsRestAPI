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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {

            _context = context;
        }

        public  async Task AddCategory(Category category)
        {
            category.Status = true;
            category.CreatedAt = DateTime.Now;
            category.UpdatedAt = DateTime.Now;
            await _context.Category.AddAsync(category);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = await _context.Category.FirstOrDefaultAsync(c => c.Id == categoryId && c.Status);
            if (category == null)
                throw new Exception("Category not found");
            // Soft delete
            category.Status = false;
            category.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> GetCategory(int categoryId)
        {
           return await _context.Category.Include(c => c.SubCategories).FirstOrDefaultAsync(c => c.Id == categoryId && c.Status);
        }

        public async Task<List<Category>> GetCategoryList()
        {
            return await _context.Category.Where(c => c.Status).ToListAsync();

        }
    }
}
