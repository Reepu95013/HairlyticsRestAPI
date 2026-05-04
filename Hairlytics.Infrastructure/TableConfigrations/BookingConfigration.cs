using Hairlytics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.TableConfigrations
{
    public class BookingConfigration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(b => b.Payments)
              .WithOne(b => b.Booking)
              .HasForeignKey(b => b.BookingId)
              .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(b => b.BookedService)
                .WithOne(b => b.Booking)
                .HasForeignKey(b => b.BookingId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(b => b.VendorProfileId);
            builder.HasIndex(b => b.VendorStaffId);
            builder.HasIndex(b => b.AppointmentDate);



        }
    }
}
