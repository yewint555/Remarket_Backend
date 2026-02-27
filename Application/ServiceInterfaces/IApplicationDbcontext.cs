using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.ServiceInterfaces;

public interface IApplicationDbcontext
{
DbSet<Users> Users { get; set; }
DbSet<Addresses> Addresses { get; set; }
DbSet<SocialMediaLinks> SocialMediaLinks { get; set; }
DbSet<Posts> Posts { get; set; }
DbSet<Orders> Orders { get; set; }
DbSet<Image> Images { get; set; }
DbSet<Mark> Marks { get; set; }

int SaveChanges();
Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}