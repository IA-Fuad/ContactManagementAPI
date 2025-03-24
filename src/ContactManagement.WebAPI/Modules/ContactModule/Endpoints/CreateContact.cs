using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.ContactModule.Repo;
using ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebAPI.Modules.ContactModule.Endpoints;

public class CreateContact : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", Handle)
            .WithSummary("Creates a contact [AuthRole: Admin].")
            .WithFluentValidation<Request>()
            .RequireAuthorization("AdminOnly");
    }
    
    public record Request(string FirstName, string LastName, string? Email, string? Phone);

    public class ContactValidator : AbstractValidator<Request>
    {
        public ContactValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(c => c.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.Phone).MinimumLength(8).MaximumLength(20);
        }
    }

    internal static async Task<Results<Ok<ContactResponse>, ProblemHttpResult>> Handle(
        [FromBody]Request request, 
        CancellationToken cancellationToken,
        ILogger<CreateContact> logger,
        IContactCommandRepo repo)
    {
        Contact contact = Contact.Create(request.FirstName, request.LastName, request.Email, request.Phone);
        bool success = await repo.CreateContact(contact, cancellationToken);

        if (success)
        {
            logger.LogInformation("Created contact with id: {ContactId}", contact.Id);
            return TypedResults.Ok(contact.ToResponse());
        }
        return TypedResults.Problem("Failed to create contact.", statusCode: StatusCodes.Status500InternalServerError);
    }
}