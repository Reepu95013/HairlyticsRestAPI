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
    public class VendorDocumentConfiguration: IEntityTypeConfiguration<VendorDocument>
    {
        public void Configure(EntityTypeBuilder<VendorDocument> builder)
        {
            builder.HasKey(vd => vd.Id);
        }
    }
}
