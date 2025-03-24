using System.Linq.Expressions;
using ContactManagement.Data.Infrastructure.EFDbContext;
using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Common;
using Microsoft.EntityFrameworkCore;
using Response = ContactManagement.WebAPI.Modules.FundModule.Endpoints.GetFundContacts.Response;

namespace ContactManagement.WebAPI.Modules.FundModule.Repo.Queries;

internal class GetFundContactQuery : IGetFundContactQueryRepo
{
    private readonly ContactManagementDbContext _dbContext;

    public GetFundContactQuery(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Response>> GetFundContacts(Guid id, IPagination pagination, CancellationToken cancellationToken)
    {
        var contactQuery = _dbContext.FundContacts
            .Include(cf => cf.Contact)
            .AsNoTracking()
            .Where(cf => cf.FundId == id && !cf.Contact.IsDeleted);

        contactQuery = pagination.SortAsc.GetValueOrDefault()
            ? contactQuery.OrderBy(GetContactOrderSelector(pagination))
            : contactQuery.OrderByDescending(GetContactOrderSelector(pagination));
        
        return await contactQuery
            .Select(cf => new Response(
                cf.Contact.Id, 
                cf.Contact.FullName.FirstName, 
                cf.Contact.FullName.LastName, 
                cf.Contact.Email, 
                cf.Contact.Phone))
            .ToPagedResult(pagination, cancellationToken);
    }
    
    private Expression<Func<FundContact, object>> GetContactOrderSelector(IPagination pagination)
    {
        return pagination.SortBy?.ToLower() switch
        {
            "firstname" => contactFund => contactFund.Contact.FullName.FirstName,
            "lastname" => contactFund => contactFund.Contact.FullName.LastName,
            "email" => contactFund => contactFund.Contact.Email ?? string.Empty,
            _ => contactFund => contactFund.Contact.CreatedAt
        };
    }
}