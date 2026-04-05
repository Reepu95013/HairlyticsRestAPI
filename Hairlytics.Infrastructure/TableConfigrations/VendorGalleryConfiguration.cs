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
    public class VendorGalleryConfiguration : IEntityTypeConfiguration<VendorGallery>
    {
        public void Configure(EntityTypeBuilder<VendorGallery> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageUrl)
                .IsRequired();
                
            builder.Property(x => x.IsThumbnail)
                .HasDefaultValue(false);
            
            // 🔗 Relationship
            builder.HasOne(x => x.VendorProfile)
                .WithMany(v => v.Gallery)
                .HasForeignKey(x => x.VendorProfileId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.VendorProfileId);
        }
    }
}
