using Domain.Entities.ContactUsAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ContactUsAggregate;
public class ContactUsConfiguration : IEntityTypeConfiguration<ContactUs>
{
    public void Configure(EntityTypeBuilder<ContactUs> builder)
    {
        builder.ToTable("ContactUs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FullName).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
    }
}