using ContactManagement.WebAPI.Modules.FundModule.Repo;
using ContactManagement.WebAPI.Modules.FundModule.Repo.Commands;
using ContactManagement.WebAPI.Modules.FundModule.Repo.Queries;

namespace ContactManagement.WebAPI.Modules.FundModule.Configurations;

public static class RegisterFundModuleServicesExtension
{
   public static IServiceCollection AddFundModuleServices(this IServiceCollection services)
   {
      services.AddScoped<IFundCommandRepo, FundCommandRepo>();
      services.AddScoped<IFundCommonQueryRepo, FundCommonQuery>();
      services.AddScoped<IGetFundContactQueryRepo, GetFundContactQuery>();
      services.AddScoped<IAssignContactQueryRepo, AssignContactQuery>();
      services.AddScoped<IGetFundsQueryRepo, GetFundsQuery>();
      
      return services;
   }
}