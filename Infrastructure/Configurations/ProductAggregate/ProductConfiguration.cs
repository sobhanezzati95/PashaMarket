﻿using Domain.Entities.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ProductAggregate;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(15).IsRequired();
        builder.Property(x => x.Picture).HasMaxLength(1000);
        builder.Property(x => x.PictureAlt).HasMaxLength(255);
        builder.Property(x => x.PictureTitle).HasMaxLength(500);
        builder.Property(x => x.Keywords).HasMaxLength(100).IsRequired();
        builder.Property(x => x.MetaDescription).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(500).IsRequired();
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId);
        builder.HasMany(x => x.ProductPictures)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
        builder.HasMany(x => x.Discounts)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
        builder.HasMany(x => x.OrderItems)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
    }
}