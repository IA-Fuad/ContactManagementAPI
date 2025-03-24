using ContactManagement.WebAPI.Modules.ContactModule.Repo;
using ContactManagement.WebAPI.Modules.ContactModule.Repo.Commands;
using ContactManagement.WebAPI.Modules.ContactModule.Repo.Queries;

namespace ContactManagement.WebAPI.Modules.ContactModule.Configurations;

public static class RegisterContactModuleServicesExtension
{
    public static IServiceCollection AddContactModuleServices(this IServiceCollection services)
    {
        services.AddScoped<IContactCommandRepo, ContactCommandRepo>();
        services.AddScoped<IContactCommonQueryRepo, ContactCommonQuery>();
        services.AddScoped<IGetContactByIdQueryRepo, GetContactByIdQuery>();
        services.AddScoped<IDeleteContactByIdQueryRepo, DeleteContactByIdQuery>();
        services.AddScoped<IGetContactsQueryRepo, GetContactsQuery>();
        
        return services;
    }
}