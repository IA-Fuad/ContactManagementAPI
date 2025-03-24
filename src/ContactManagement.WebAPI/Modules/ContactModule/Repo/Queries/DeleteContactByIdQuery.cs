using ContactManagement.Data.Infrastructure.EFDbContext;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.WebAPI.Modules.ContactModule.Repo.Queries;

public class DeleteContactByIdQuery : IDeleteContactByIdQueryRepo
{
    private readonly ContactManagementDbContext _dbContext;

    public DeleteContactByIdQuery(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsAssignedToFund(Guid contactId, CancellationToken cancellationToken)
    {
        return await _dbContext.FundContacts
            .AsNoTracking()
            .AnyAsync(c => c.ContactId == contactId && c.ContactId == contactId, cancellationToken);
    }
}