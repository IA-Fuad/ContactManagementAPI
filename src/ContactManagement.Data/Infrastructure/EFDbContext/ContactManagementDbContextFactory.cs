using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ContactManagement.Data.Infrastructure.EFDbContext;

public class ContactManagementDbContextFactory : IDesignTimeDbContextFactory<ContactManagementDbContext>
{
    public ContactManagementDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        DbContextOptionsBuilder<ContactManagementDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("ContactManagementDb"));
        
        return new ContactManagementDbContext(optionsBuilder.Options);
    }
}