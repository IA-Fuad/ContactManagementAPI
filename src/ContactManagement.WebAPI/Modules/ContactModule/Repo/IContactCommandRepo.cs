using ContactManagement.Data.Models;

namespace ContactManagement.WebAPI.Modules.ContactModule.Repo;

public interface IContactCommandRepo
{
    Task<bool> CreateContact(Contact contact, CancellationToken cancellationToken);

    Task<bool> UpdateContact(Contact contact, CancellationToken cancellationToken);
}