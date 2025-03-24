using ContactManagement.Data.Infrastructure.EFDbContext;
using ContactManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.WebAPI.Modules.ContactModule.Repo.Queries;

public class ContactCommonQuery : IContactCommonQueryRepo 
{
    private readonly ContactManagementDbContext _dbContext;

    public ContactCommonQuery(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Contact?> GetContactById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Contacts
            .AsNoTracking()
            .Where(c => c.Id == id && !c.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);
    }

}