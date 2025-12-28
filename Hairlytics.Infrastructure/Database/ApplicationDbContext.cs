using Hairlytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.Database
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 

        }

        public DbSet<User> Users { get; set; }
        public DbSet<VendorProfile> VendorProfiles { get; set; }
        public DbSet<VendorDocument> VendorDocuments { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }


        
    }
}
