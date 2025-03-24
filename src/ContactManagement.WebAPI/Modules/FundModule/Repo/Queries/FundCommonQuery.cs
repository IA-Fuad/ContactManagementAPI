using ContactManagement.Data.Infrastructure.EFDbContext;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.WebAPI.Modules.FundModule.Repo.Queries;

internal class FundCommonQuery : IFundCommonQueryRepo
{
    private readonly ContactManagementDbContext _dbContext;

    public FundCommonQuery(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> FundExists(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Funds.AnyAsync(f => f.Id == id && !f.IsDeleted, cancellationToken);
    }
}