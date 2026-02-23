using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class AddressConfiguration : IEntityTypeConfiguration<Addresses>
{ 
    public void Configure(EntityTypeBuilder<Addresses> builder)
{

    builder.HasKey(a => a.Id);

    builder.Property(a => a.HomeNumber)
        .IsRequired()
        .HasMaxLength(50);

    builder.Property(a => a.AptNumber)
        .IsRequired(false)
        .HasMaxLength(50);

    builder.Property(a => a.Street)
        .IsRequired()
        .HasMaxLength(200);

    builder.Property(a => a.TownShip)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(a => a.City)
        .IsRequired()
        .HasMaxLength(100);

    builder.HasOne(a => a.User)
        .WithMany(u => u.Addresses)
        .HasForeignKey(a => a.UserId)
        .OnDelete(DeleteBehavior.Restrict); 

    builder.HasIndex(a => a.City);
    builder.HasIndex(a => a.TownShip);
}
}