using BuyStuff.GE.Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuyStuff.GE.Persistence.Configurations
{
    internal class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(item => item.Id);
            builder.HasMany(item => item.Images)
                .WithMany(image => image.Items);
            builder.Property(item=>item.Title).IsRequired();
            builder.Property(item=>item.PhoneNumber).IsRequired();
            builder.Property(item=>item.Title).IsRequired();
            builder.Property(item=>item.Description).IsRequired();
            builder.Property(item=>item.IsDeleted).IsRequired();
        }
    }
}
