using System.Linq.Expressions;
using ContactManagement.Data.Infrastructure.EFDbContext;
using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Common;
using Microsoft.EntityFrameworkCore;
using Response = ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs.ContactResponse;

namespace ContactManagement.WebAPI.Modules.ContactModule.Repo.Queries;

public class GetContactsQuery : IGetContactsQueryRepo
{
    private readonly ContactManagementDbContext _dbContext;

    public GetContactsQuery(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Response>> GetContacts(IPagination pagination, CancellationToken cancellationToken)
    {
        var contactQuery = _dbContext.Contacts
            .AsNoTracking()
            .Where(c => !c.IsDeleted);

        contactQuery = pagination.SortAsc.GetValueOrDefault()
            ? contactQuery.OrderBy(GetContactOrderSelector(pagination))
            : contactQuery.OrderByDescending(GetContactOrderSelector(pagination));
        
        return await contactQuery
            .Select(c => new Response(
                c.Id, 
                c.FullName.FirstName, 
                c.FullName.LastName, 
                c.Email, 
                c.Phone))
            .ToPagedResult(pagination, cancellationToken);
    }

    private Expression<Func<Contact, object>> GetContactOrderSelector(IPagination pagination)
    {
        return pagination.SortBy?.ToLower() switch
        {
            "firstname" => contact => contact.FullName.FirstName,
            "lastname" => contact => contact.FullName.LastName,
            "email" => contact => contact.Email ?? string.Empty,
            _ => contact => contact.CreatedAt
        };
    }
}