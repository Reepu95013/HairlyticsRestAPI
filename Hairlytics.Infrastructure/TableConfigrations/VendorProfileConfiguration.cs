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
    public class VendorProfileConfiguration : IEntityTypeConfiguration<VendorProfile>
    {
        public void Configure(EntityTypeBuilder<VendorProfile> builder)
        {
            builder.HasKey(v => v.Id);

            builder.HasMany(v => v.Services)
                   .WithOne(s => s.VendorProfile)
                   .HasForeignKey(s => s.VendorProfileId);

            builder.HasMany(v => v.Documents)
                   .WithOne(vd => vd.VendorProfile)
                   .HasForeignKey(vd => vd.VendorProfileId);
        }
    }
}
