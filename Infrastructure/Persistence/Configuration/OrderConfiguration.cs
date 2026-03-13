using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class OrderConfiguration :IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> builder)
    {

    builder.HasKey(o => o.Id);

    builder.Property(o => o.OrderDate)
        .IsRequired()
        .HasDefaultValueSql("GETUTCDATE()");

    builder.Property(o => o.TotalAmount)
        .HasColumnType("decimal(18,2)")
        .IsRequired();

    builder.Property(o => o.PhoneNumber)
        .IsRequired()
        .HasMaxLength(20); 

    builder.Property(o => o.OrderComfirmationStatus)
        .HasConversion<string>()
        .HasMaxLength(50)
        .IsRequired();

    builder.HasOne(o => o.User)
        .WithMany(u => u.Orders)
        .HasForeignKey(o => o.UserId)
        .OnDelete(DeleteBehavior.Restrict); 

    builder.HasOne(o => o.Address)
            .WithMany() 
            .HasForeignKey(o => o.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(o => o.Post)
        .WithMany()
        .HasForeignKey(o => o.PostId)
        .OnDelete(DeleteBehavior.NoAction); 

    builder.HasIndex(o => o.OrderDate);
    builder.HasIndex(o => o.UserId);
    builder.HasIndex(o => o.PostId);
    builder.HasIndex(o => o.AddressId);
}
}