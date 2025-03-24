using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.FundModule.Repo;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebAPI.Modules.FundModule.Endpoints;

public class AssignContact : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/assignContact", Handle)
            .WithSummary("Assign a contact to a fund [AuthRole: FundManger]");
    }
    
    public record Request(Guid FundId, Guid ContactId);

    internal static async Task<Results<Ok, ProblemHttpResult>> Handle(
        [FromBody]Request request,
        CancellationToken cancellationToken,
        ILogger<AssignContact> logger,
        IFundCommonQueryRepo commonQueryRepo,
        IAssignContactQueryRepo assignContactQueryRepo,
        IFundCommandRepo commandRepo)
    {
        if (!await assignContactQueryRepo.ContactExists(request.ContactId, cancellationToken))
        {
            logger.LogInformation("Contact {ContactId} not found", request.ContactId);
            return TypedResults.Problem($"The contact with id {request.ContactId} could not be found.", statusCode: StatusCodes.Status404NotFound);
        }
        if (!await commonQueryRepo.FundExists(request.FundId, cancellationToken))
        {
            logger.LogInformation("Fund {FundId} not found", request.FundId);
            return TypedResults.Problem($"The fund with id {request.FundId} could not be found.", statusCode: StatusCodes.Status404NotFound);
        }
        if (await assignContactQueryRepo.ContactIsAssignedToFund(request.FundId, request.ContactId, cancellationToken))
        {
            logger.LogInformation("Contact {ContactId} is already assigned to fund {FundId}", request.ContactId, request.FundId);
            return TypedResults.Problem($"The contact is already assigned to the fund", statusCode: StatusCodes.Status422UnprocessableEntity);
        }
        
        await commandRepo.AddFundContact(FundContact.Create(request.ContactId, request.FundId), cancellationToken);
        
        logger.LogInformation("Assigned contact {ContactId} to the fund {FundId}", request.ContactId, request.FundId);
        return TypedResults.Ok();
    }
}