using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ContactManagement.WebAPI.Modules.AuthModule.Configurations;

public static class SwaggergenAuthOptionExtension
{
    public static SwaggerGenOptions ConfigureSwaggerGenAuthOption(this SwaggerGenOptions options)
    {
        OpenApiSecurityScheme securityScheme = new() 
        {
            Name = "Authorization",
            Description = "Enter 'Bearer {token}'",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        };

        OpenApiSecurityRequirement securityRequirement = new() 
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                []
            }
        };

        options.AddSecurityDefinition("Bearer", securityScheme);
        options.AddSecurityRequirement(securityRequirement); 
        
        return options;
    }
}