using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.FundModule.Repo;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebAPI.Modules.FundModule.Endpoints;

public class GetFundContacts : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}/contacts", Handle)
            .WithSummary("Gets all contacts assigned to a fund (paginated) [AuthRole: FundManager].")
            .WithFluentValidation<Request>();
    }

    public record Request(int Page, int PageSize, string? SortBy, bool? SortAsc) : IPagination;

    public class FundContactValidator : PaginationRequestValidator<Request>
    {
        public FundContactValidator()
        {
            RuleFor(f => f.SortBy)
                .Must(sortBy => sortBy?.ToLower() switch
                {
                    null or "" or "firstname" or "lastname" or "email" => true,
                    _ => false
                }).WithMessage(f => $"SortBy parameter value {f.SortBy} is invalid.");
        }
    }
    
    public record Response(Guid ContactId, string FirstName, string LastName, string? Email, string? Phone);
    
    internal static async Task<Results<Ok<PagedResult<Response>>, NotFound>> Handle(
        [FromRoute]Guid id,
        [AsParameters]Request request,
        CancellationToken cancellationToken,
        ILogger<GetFundContacts> logger,
        IFundCommonQueryRepo commonQueryRepo,
        IGetFundContactQueryRepo fundContactQueryRepo)
    {
        if (!await commonQueryRepo.FundExists(id, cancellationToken))
        {
            logger.LogInformation("Could not found fund with id {FundId}", id);
            return TypedResults.NotFound();
        }

        var contactResponses = await fundContactQueryRepo.GetFundContacts(id, request, cancellationToken);
        logger.LogInformation("Total {ContactCount} contacts found for fund id {FundId}", contactResponses.TotalCount, id);

        return TypedResults.Ok(contactResponses);
    }
}
