using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.ContactModule.Repo;
using ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ContactManagement.WebAPI.Modules.ContactModule.Endpoints;

public class GetContacts : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/", Handle)
            .WithSummary("Get contacts (paginated) [AuthRole: Admin or FundManager].")
            .WithFluentValidation<Request>();
    }

    public record Request(int Page, int PageSize, string? SortBy, bool? SortAsc) : IPagination;
    
    public class GetContactsValidator : PaginationRequestValidator<Request>
    {
        public GetContactsValidator()
        {
            RuleFor(f => f.SortBy)
                .Must(sortBy => sortBy?.ToLower() switch
                {
                    null or "" or "firstname" or "lastname" or "email" => true,
                    _ => false
                }).WithMessage(f => $"SortBy parameter value {f.SortBy} is invalid.");
        }
    } 

    internal static async Task<Ok<PagedResult<ContactResponse>>> Handle(
        [AsParameters]Request request,
        CancellationToken cancellationToken,
        IGetContactsQueryRepo repo)
    { 
        var contacts = await repo.GetContacts(request, cancellationToken);
        return TypedResults.Ok(contacts);
    }
}