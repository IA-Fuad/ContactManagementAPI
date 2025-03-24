using System.Linq.Expressions;
using ContactManagement.Data.Infrastructure.EFDbContext;
using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Common;
using Microsoft.EntityFrameworkCore;
using Response = ContactManagement.WebAPI.Modules.FundModule.Endpoints.GetFunds.Response;

namespace ContactManagement.WebAPI.Modules.FundModule.Repo.Queries;

public class GetFundsQuery : IGetFundsQueryRepo
{
    private readonly ContactManagementDbContext _dbContext;

    public GetFundsQuery(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Response>> GetFunds(IPagination pagination, CancellationToken cancellationToken)
    {
        var fundQuery = _dbContext.Funds
            .AsNoTracking()
            .Where(f => !f.IsDeleted);

        fundQuery = pagination.SortAsc.GetValueOrDefault()
            ? fundQuery.OrderBy(GetFundOrderSelector(pagination))
            : fundQuery.OrderByDescending(GetFundOrderSelector(pagination));
        
        return await fundQuery
            .Select(f => new Response(
                f.Id, 
                f.Name))
            .ToPagedResult(pagination, cancellationToken);
    }

    private Expression<Func<Fund, object>> GetFundOrderSelector(IPagination pagination)
    {
        return pagination.SortBy?.ToLower() switch
        {
            "name" => fund => fund.Name,
            _ => fund => fund.CreatedAt
        };
    }
}