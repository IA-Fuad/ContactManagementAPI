using ContactManagement.Data.Infrastructure.EFDbContext;
using ContactManagement.Data.Models;

namespace ContactManagement.WebAPI.Modules.ContactModule.Repo.Commands;

internal class ContactCommandRepo : IContactCommandRepo
{
    private readonly ContactManagementDbContext _dbContext;

    public ContactCommandRepo(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateContact(Contact contact, CancellationToken cancellationToken)
    {
        _dbContext.Contacts.Add(contact);
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateContact(Contact contact, CancellationToken cancellationToken)
    {
        _dbContext.Contacts.Update(contact);
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}