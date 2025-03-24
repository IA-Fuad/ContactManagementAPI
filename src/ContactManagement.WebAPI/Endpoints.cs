using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.AuthModule.Endpoints;
using ContactManagement.WebAPI.Modules.ContactModule.Endpoints;
using ContactManagement.WebAPI.Modules.FundModule.Endpoints;

namespace ContactManagement.WebAPI;

internal static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.MapGroup("api");
        apiGroup.WithOpenApi();

        apiGroup.MapAuthModuleEndpoints();
        apiGroup.MapContactModuleEndpoints();
        apiGroup.MapFundModuleEndpoints();
    }

    private static IEndpointRouteBuilder MapAuthModuleEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder authGroup = app.MapGroup("/auth");
        authGroup.AllowAnonymous();

        authGroup.Map<Login>();
        
        return app;
    }

    private static IEndpointRouteBuilder MapContactModuleEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder contactApiGroup = app.MapGroup("/contacts");
        contactApiGroup.RequireAuthorization("AdminOrFundManager");
        contactApiGroup.WithTags("Contact Module");

        contactApiGroup.Map<DeleteContactById>();
        contactApiGroup.Map<CreateContact>();
        contactApiGroup.Map<UpdateContact>();
        
        contactApiGroup.Map<GetContactById>();
        contactApiGroup.Map<GetContacts>();
        
        return app;
    }

    private static IEndpointRouteBuilder MapFundModuleEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder fundApiGroup = app.MapGroup("/funds");
        fundApiGroup.RequireAuthorization("FundManagerOnly");
        fundApiGroup.WithTags("Fund Module");

        fundApiGroup.Map<CreateFund>();
        fundApiGroup.Map<AssignContact>();
        fundApiGroup.Map<RemoveContact>();
        
        fundApiGroup.Map<GetFunds>();
        fundApiGroup.Map<GetFundContacts>();

        return app;
    }

    private static IEndpointRouteBuilder Map<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}