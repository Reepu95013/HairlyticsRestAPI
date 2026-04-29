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
    public class VendorStaffConfiguration : IEntityTypeConfiguration<VendorStaff>
    {
        public void Configure(EntityTypeBuilder<VendorStaff> builder)
        {
            builder.HasKey(v => v.Id);

            builder.HasMany(v=>v.StaffAvailabilities)
            .WithOne(vd => vd.Staff)
            .HasForeignKey(vd => vd.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
