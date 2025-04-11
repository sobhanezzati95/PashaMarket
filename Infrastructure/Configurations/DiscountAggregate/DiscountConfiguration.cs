using Domain.Entities.DiscountAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.DiscountAggregate
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discounts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Reason).HasMaxLength(500);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Discounts)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
