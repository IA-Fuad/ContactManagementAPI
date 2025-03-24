using ContactManagement.Data.Infrastructure.EFDbContext;
using ContactManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.WebAPI.Modules.FundModule.Repo.Commands;

internal class FundCommandRepo : IFundCommandRepo
{
    private readonly ContactManagementDbContext _dbContext;

    public FundCommandRepo(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddFund(Fund fund, CancellationToken cancellationToken)
    {
        _dbContext.Funds.Add(fund);
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddFundContact(FundContact fundContact, CancellationToken cancellationToken)
    {
        _dbContext.FundContacts.Add(fundContact);
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> RemoveFundContact(Guid fundId, Guid contactId, CancellationToken cancellationToken)
    {
        return await _dbContext.FundContacts
            .Where(fc => fc.FundId == fundId && fc.ContactId == contactId)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }
}