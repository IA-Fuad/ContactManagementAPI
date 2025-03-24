using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.ContactModule.Repo;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebAPI.Modules.ContactModule.Endpoints;

public class DeleteContactById : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:guid}", Handle)
            .WithSummary("Deletes contact by id [AuthRole: Admin].")
            .RequireAuthorization("AdminOnly");
    }

    internal static async Task<Results<Ok, NotFound, ProblemHttpResult>> Handle(
        [FromRoute]Guid id, 
        CancellationToken cancellationToken,
        ILogger<DeleteContactById> logger,
        IContactCommonQueryRepo commonQueryRepo,
        IDeleteContactByIdQueryRepo deleteQueryRepo,
        IContactCommandRepo commandRepo)
    {
        Contact? contact = await commonQueryRepo.GetContactById(id, cancellationToken);
        if (contact is null)
        {
            logger.LogError("Contact {ContactId} not found", id);
            return TypedResults.NotFound();
        }
        if (await deleteQueryRepo.IsAssignedToFund(id, cancellationToken))
        {
            logger.LogError("Contact {ContactId} could not be deleted as it is assigned to one or more funds.", id);
            return TypedResults.Problem("The contact cannot be deleted as it is assigned to one or more funds.", statusCode: StatusCodes.Status422UnprocessableEntity);
        }

        contact.Delete();
        await commandRepo.UpdateContact(contact, cancellationToken);
        
        logger.LogInformation("Contact {ContactId} has been deleted.", id);
        return TypedResults.Ok();
    }
}