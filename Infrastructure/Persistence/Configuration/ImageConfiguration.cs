using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
{

    builder.HasKey(i => i.Id);

    builder.Property(i => i.ImgPath)
        .IsRequired()
        .HasMaxLength(500);

    builder.Property(i => i.ImgUrl)
        .IsRequired(false)
        .HasMaxLength(1000);

    builder.HasOne(i => i.User)
        .WithMany(u => u.Images)
        .HasForeignKey(i => i.UserId)
        .OnDelete(DeleteBehavior.NoAction); 

    builder.HasOne(i => i.Post)
        .WithMany(p => p.Images)
        .HasForeignKey(i => i.PostId)
        .OnDelete(DeleteBehavior.Cascade); 

    builder.HasIndex(i => i.UserId);
    builder.HasIndex(i => i.PostId);
}
}

