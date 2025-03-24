using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.FundModule.Repo;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebAPI.Modules.FundModule.Endpoints;

public class CreateFund : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", Handle)
            .WithSummary("Creates a fund [AuthRole: FundManager].")
            .WithFluentValidation<Request>();
    }

    public record Request(string Name);

    public class FundValidator : AbstractValidator<Request>
    {
        public FundValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        }
    }
    
    public record Response(Guid Id, string Name);

    internal static async Task<Ok<Response>> Handle(
        [FromBody]Request request,
        CancellationToken cancellationToken,
        ILogger<CreateFund> logger,
        IFundCommandRepo commandRepo)
    {
        Fund fund = Fund.Create(request.Name); 
        await commandRepo.AddFund(fund, cancellationToken);
        
        logger.LogInformation("Created fund {FundId}", fund.Id);
        return TypedResults.Ok(new Response(fund.Id, fund.Name));
    }
}