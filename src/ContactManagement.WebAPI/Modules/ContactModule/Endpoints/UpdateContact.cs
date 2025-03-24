using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.ContactModule.Repo;
using ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebAPI.Modules.ContactModule.Endpoints;

public class UpdateContact : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/", Handle)
            .WithSummary("Updates a contact [AuthRole: Admin].")
            .WithFluentValidation<Request>()
            .RequireAuthorization("AdminOnly");
    }

    public record Request(Guid Id, string FirstName, string LastName, string? Email, string? Phone);

    public class ContactValidator : AbstractValidator<Request>
    {
        public ContactValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Phone).MinimumLength(8).MaximumLength(20);
        }
    }

    internal static async Task<Results<Ok<ContactResponse>, NotFound>> Handle(
        [FromBody]Request request, 
        CancellationToken cancellationToken,
        ILogger<UpdateContact> logger,
        IContactCommonQueryRepo queryRepo,
        IContactCommandRepo commandRepo)
    {
        Contact? contact = await queryRepo.GetContactById(request.Id, cancellationToken);
        if (contact is null)
        {
            logger.LogError("Contact {ContactId} not found", request.Id);
            return TypedResults.NotFound();
        }

        contact.Update(request.FirstName, request.LastName, request.Email, request.Phone);
        await commandRepo.UpdateContact(contact, cancellationToken);
        
        logger.LogInformation("Contact {ContactId} updated with request {Request}", request.Id, request);
        return TypedResults.Ok(contact.ToResponse());
    }
}