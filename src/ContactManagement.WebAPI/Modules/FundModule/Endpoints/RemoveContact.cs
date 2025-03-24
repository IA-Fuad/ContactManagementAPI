using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.FundModule.Repo;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebAPI.Modules.FundModule.Endpoints;

public class RemoveContact : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/contact", Handle)
            .WithSummary("Unassign a contact from a fund [AuthRole: FundManager].");
    }
    
    public record Request(Guid FundId, Guid ContactId);
    
    internal static async Task<Results<Ok, NotFound>> Handle(
        [FromBody]Request request,
        CancellationToken cancellationToken,
        ILogger<RemoveContact> logger,
        IFundCommandRepo commandRepo)
    {
        bool deleted = await commandRepo.RemoveFundContact(request.FundId, request.ContactId, cancellationToken);

        if (!deleted)
        {
            logger.LogInformation("No records found for request {Request}", request);
            return TypedResults.NotFound();
        }
        
        logger.LogInformation("Removed contact {ContactId} from fund {FundId}", request.ContactId, request.FundId);
        return TypedResults.Ok();
    }
}