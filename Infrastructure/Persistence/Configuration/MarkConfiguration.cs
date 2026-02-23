using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class MarkConfiguration : IEntityTypeConfiguration<Mark>
    {
        public void Configure(EntityTypeBuilder<Mark> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasIndex(m => new { m.UserId, m.PostId })
            .IsUnique();

        builder.HasOne(m => m.User)
            .WithMany(u => u.SavedPosts)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(m => m.Post)
            .WithMany()
            .HasForeignKey(m => m.PostId)
            .OnDelete(DeleteBehavior.NoAction); 
    }
    }
}