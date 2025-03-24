using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.ContactModule.Repo;
using ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebAPI.Modules.ContactModule.Endpoints;

public class GetContactById : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", Handle)
            .WithSummary("Gets a contact by id [AuthRole: Admin or FundManager].");
    }

    internal static async Task<Results<Ok<ContactResponse>, NotFound>> Handle(
        [FromRoute]Guid id, 
        CancellationToken cancellationToken,
        ILogger<GetContactById> logger,
        IGetContactByIdQueryRepo repo)
    {
        ContactResponse? contact = await repo.GetContactById(id, cancellationToken);
        if (contact is null)
        {
            logger.LogInformation("Contact with id {ContactId} not found", id);
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(contact);
    }
}
