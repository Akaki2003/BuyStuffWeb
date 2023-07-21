using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuyStuff.GE.Domain.Images;

namespace BuyStuff.GE.Persistence.Configurations
{
    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(img => img.Id);
            builder.HasMany(img => img.Items).WithMany(prod => prod.Images);
            builder.Property(img => img.ImgName).IsRequired();
            builder.Property(img => img.ImagePath).IsRequired();
        }
    }
}
