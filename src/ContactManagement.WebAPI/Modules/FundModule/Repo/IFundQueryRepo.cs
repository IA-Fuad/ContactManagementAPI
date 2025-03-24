using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.FundModule.Endpoints;

namespace ContactManagement.WebAPI.Modules.FundModule.Repo;

public interface IFundCommonQueryRepo
{
    Task<bool> FundExists(Guid id, CancellationToken cancellationToken);
}

public interface IGetFundContactQueryRepo
{
    Task<PagedResult<GetFundContacts.Response>> GetFundContacts(Guid id, IPagination pagination, CancellationToken cancellationToken);
}

public interface IGetFundsQueryRepo
{
    Task<PagedResult<GetFunds.Response>> GetFunds(IPagination pagination, CancellationToken cancellationToken);
}

public interface IAssignContactQueryRepo
{
    Task<bool> ContactIsAssignedToFund(Guid fundId, Guid contactId, CancellationToken cancellationToken);
    Task<bool> ContactExists(Guid id, CancellationToken cancellationToken);
}