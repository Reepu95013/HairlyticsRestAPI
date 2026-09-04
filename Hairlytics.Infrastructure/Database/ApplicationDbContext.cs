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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<VendorProfile> VendorProfiles { get; set; }
        public DbSet<VendorDocument> VendorDocuments { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ForgotPassword> ForgotPassword { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<VendorGallery> VendorGallery { get; set; }
        public DbSet<VendorStaff> VendorStaff { get; set; }
        public DbSet<StaffAvailability> StaffAvailability { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookedService> BookedServices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RegisterPhoneNumber> RegisterPhoneNumbers { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }







    }
}
