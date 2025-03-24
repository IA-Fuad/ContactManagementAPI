using ContactManagement.Data.Infrastructure.EFDbContext;
using ContactManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using IServiceScopeFactory = Microsoft.Extensions.DependencyInjection.IServiceScopeFactory;

namespace ContactManagement.WebAPI.Configurations;

public static class ConfigureMiddlewares
{
    public static async Task AddContactManagementWebAPIMiddlewares(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "Contact Management API";
                options.DisplayRequestDuration();
                options.EnableTryItOutByDefault();
                options.DefaultModelsExpandDepth(-1);
            });
        }

        try
        {
            logger.LogInformation("Trying to create SQL Server database...");
            await CreateDatabaseIfNotExists(app);
            logger.LogInformation("Created SQL Server database.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred creating the Database. Please check the connection string and make sure the SQL Server database is available.");
            throw;
        }

        app.UseExceptionHandler();

        app.MapEndpoints();

        app.UseAuthentication();
        app.UseAuthorization();
    }

    private static async Task CreateDatabaseIfNotExists(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        var dbContext = serviceScope?.ServiceProvider.GetRequiredService<ContactManagementDbContext>();

        if (!await dbContext!.Database.CanConnectAsync())
        {
            throw new Exception($"Cannot connect to SQL Server. Please check the connection string.");
        }
        
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync()!;
        
        await SeedDatabase(dbContext);
    }

    private static async Task SeedDatabase(ContactManagementDbContext dbContext)
    {
        if (!await dbContext.Funds.AnyAsync())
        {
            var funds = Enumerable.Range(1, 100).Select(i => Fund.Create($"Fund {i}"));
            dbContext.Funds.AddRange(funds);
            await dbContext.SaveChangesAsync();
        }
    }
}