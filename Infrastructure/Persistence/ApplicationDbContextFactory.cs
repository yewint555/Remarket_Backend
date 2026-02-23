using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
public ApplicationDbContext CreateDbContext(string[] args)
{
    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    
    optionsBuilder.UseSqlServer("Server=localhost,1433;Database=RemarketDb;User Id=sa;Password=20010575Ye#;TrustServerCertificate=True;");

    return new ApplicationDbContext(optionsBuilder.Options);
}
}