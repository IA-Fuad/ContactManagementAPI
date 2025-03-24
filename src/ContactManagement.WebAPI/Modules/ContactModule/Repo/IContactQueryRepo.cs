using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Common;
using ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs;

namespace ContactManagement.WebAPI.Modules.ContactModule.Repo;

public interface IContactCommonQueryRepo
{
    Task<Contact?> GetContactById(Guid id, CancellationToken cancellationToken);
}

public interface IDeleteContactByIdQueryRepo 
{
    Task<bool> IsAssignedToFund(Guid contactId, CancellationToken cancellationToken);
}

public interface IGetContactByIdQueryRepo
{
    Task<ContactResponse?> GetContactById(Guid id, CancellationToken cancellationToken);
}

public interface IGetContactsQueryRepo
{
    Task<PagedResult<ContactResponse>> GetContacts(IPagination pagination, CancellationToken cancellationToken);
}