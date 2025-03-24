using ContactManagement.Data.Models;

namespace ContactManagement.WebAPI.Modules.FundModule.Repo;

public interface IFundCommandRepo
{
    Task<bool> AddFund(Fund fund, CancellationToken cancellationToken);
    Task<bool> AddFundContact(FundContact fundContact, CancellationToken cancellationToken);
    Task<bool> RemoveFundContact(Guid fundId, Guid contactId, CancellationToken cancellationToken);
}