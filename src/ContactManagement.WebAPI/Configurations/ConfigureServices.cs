using ContactManagement.Data.Infrastructure.EFDbContext;
using ContactManagement.WebAPI.Modules.AuthModule.Configurations;
using ContactManagement.WebAPI.Modules.ContactModule.Configurations;
using ContactManagement.WebAPI.Modules.FundModule.Configurations;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.WebAPI.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddContactManagementWebAPIServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));
            options.ConfigureSwaggerGenAuthOption();
        });

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = context.HttpContext.Request.Path;
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
                
                // used to trace requests through multiple distributed services
                context.ProblemDetails.Extensions.TryAdd("traceId",
                    context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity.Id);
            };
        });
        
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        services.AddDbContext<ContactManagementDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ContactManagementDb"));
        });

        services.AddAuthModuleServices(configuration);
        services.AddContactModuleServices();
        services.AddFundModuleServices();

        return services;
    }
}