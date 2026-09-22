using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

#nullable disable

namespace AssureCloud.Infrastructure.Persistence.Context;

public class AssureCloudDbContextFactory : IDesignTimeDbContextFactory<AssureCloudDbContext>
{
    public AssureCloudDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AssureCloudDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AssureCloud;Trusted_Connection=true;MultipleActiveResultSets=true");

        return new AssureCloudDbContext(optionsBuilder.Options);
    }
}
