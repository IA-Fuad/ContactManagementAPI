using ContactManagement.Data.Infrastructure.EFDbContext;
using ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs;
using Microsoft.EntityFrameworkCore;

namespace ContactManagement.WebAPI.Modules.ContactModule.Repo.Queries;

internal class GetContactByIdQuery : IGetContactByIdQueryRepo
{
    private readonly ContactManagementDbContext _dbContext;

    public GetContactByIdQuery(ContactManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ContactResponse?> GetContactById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Contacts
            .Where(c => c.Id == id && !c.IsDeleted)
            .Select(c => new ContactResponse(
                c.Id,
                c.FullName.FirstName,
                c.FullName.LastName,
                c.Email,
                c.Phone))
            .SingleOrDefaultAsync(cancellationToken);
    }
}