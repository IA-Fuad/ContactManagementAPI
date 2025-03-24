using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ContactManagement.WebAPI.Modules.AuthModule.Configurations;

public static class RegisterAuthModuleServicesExtension
{
    public static IServiceCollection AddAuthModuleServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // Set true and configure for production
                    ValidateAudience = false, // Set true and configure for production
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!))
                };
            });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            options.AddPolicy("FundManagerOnly", policy => policy.RequireRole("fund manager"));
            options.AddPolicy("AdminOrFundManager", policy => policy.RequireRole("admin", "fund manager"));
        });
        
        return services;
    }
}