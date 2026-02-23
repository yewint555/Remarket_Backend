using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class SocialMediaLinkConfiguration : IEntityTypeConfiguration<SocialMediaLinks>
{
    public void Configure(EntityTypeBuilder<SocialMediaLinks> builder)
{

    builder.HasKey(s => s.Id);

    builder.Property(s => s.Links)
        .IsRequired()
        .HasMaxLength(500);

    builder.HasOne(s => s.User)
        .WithMany(u => u.SocialMediaLinks)
        .HasForeignKey(s => s.UserId)
        .OnDelete(DeleteBehavior.Cascade); 
        

    builder.HasIndex(s => s.UserId);
}
}