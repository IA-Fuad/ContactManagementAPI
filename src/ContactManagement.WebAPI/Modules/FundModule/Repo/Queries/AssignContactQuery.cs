using ContactManagement.Data.Infrastructure.EFDbContext;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.WebAPI.Modules.FundModule.Repo.Queries;

internal class AssignContactQuery : IAssignContactQueryRepo
{
    private readonly ContactManagementDbContext _dbContext;

    public AssignContactQuery(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ContactIsAssignedToFund(Guid fundId, Guid contactId, CancellationToken cancellationToken)
    {
        return await _dbContext
            .FundContacts.AnyAsync(fc => fc.FundId == fundId && fc.ContactId == contactId, cancellationToken);
    }
    
    public async Task<bool> ContactExists(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Contacts.AnyAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
    }
}