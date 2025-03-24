using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.FundModule.Repo;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ContactManagement.WebAPI.Modules.FundModule.Endpoints;

public class GetFunds : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/", Handle)
            .WithFluentValidation<Request>()
            .WithSummary("Get funds (paginated) [AuthRole: Anonymous].")
            .AllowAnonymous();
    }

    public record Request(int Page, int PageSize, string? SortBy, bool? SortAsc) : IPagination;

    public record Response(Guid Id, string Name);

    public class GetFundsValidator : PaginationRequestValidator<Request>
    {
        public GetFundsValidator()
        {
            RuleFor(f => f.SortBy)
                .Must(sortBy => sortBy?.ToLower() switch
                {
                    null or "" or "name" => true,
                    _ => false
                }).WithMessage(f => $"SortBy parameter value {f.SortBy} is invalid.");
        }
    }

    internal static async Task<Ok<PagedResult<Response>>> Handle(
        [AsParameters]Request request,
        CancellationToken cancellationToken,
        IGetFundsQueryRepo repo)
    {
        var funds = await repo.GetFunds(request, cancellationToken);
        return TypedResults.Ok(funds);
    }
}