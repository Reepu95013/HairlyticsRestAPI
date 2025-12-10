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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ User ↔ VendorProfile (One-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.VendorProfile)
                .WithOne(v => v.User)
                .HasForeignKey<VendorProfile>(v => v.UserId);

            // ✅ VendorProfile ↔ VendorDocument (One-to-Many)
            modelBuilder.Entity<VendorProfile>()
                .HasMany(v => v.Documents)
                .WithOne(d => d.VendorProfile)
                .HasForeignKey(d => d.VendorProfileId);
        }
    }
}
