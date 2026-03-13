using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Posts>
{
    public void Configure(EntityTypeBuilder<Posts> builder)
{
    builder.HasKey(p => p.Id);

    builder.Property(p => p.ItemName)
        .IsRequired()
        .HasMaxLength(200);

    builder.Property(p => p.Price)
        .HasColumnType("decimal(11,2)")
        .IsRequired();

    builder.Property(p => p.Description)
        .IsRequired()
        .HasMaxLength(2000);

    builder.Property(p => p.BuyerCondition)
        .HasConversion<string>()
        .HasMaxLength(50);

    builder.Property(p => p.ItemCondition)
        .HasConversion<string>()
        .HasMaxLength(50);

    builder.Property(p => p.ItemStatus)
        .HasConversion<string>()
        .HasMaxLength(50)
        .HasDefaultValue(ItemStatus.Active);

    builder.HasOne(p => p.User)
        .WithMany(u => u.Posts)
        .HasForeignKey(p => p.UserId)
        .OnDelete(DeleteBehavior.Restrict); 

    builder.HasOne(p => p.Market)
        .WithMany()
        .HasForeignKey(p => p.MarketId)
        .OnDelete(DeleteBehavior.NoAction);

    builder.HasMany(p => p.Images)
        .WithOne(i => i.Post)
        .HasForeignKey(i => i.PostId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(p => p.UserId);
    builder.HasIndex(p => p.MarketId);
    builder.HasIndex(p => p.ItemStatus);

}
}