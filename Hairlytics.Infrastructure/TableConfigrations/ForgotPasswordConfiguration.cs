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
    public class ForgotPasswordConfiguration : IEntityTypeConfiguration<ForgotPassword>
    {
        public void Configure(EntityTypeBuilder<ForgotPassword> builder)
        {
            builder.HasKey(fp => fp.Id);
        }
    }
}
