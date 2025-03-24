using ContactManagement.WebAPI.Modules.ContactModule.Repo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ContactManagement.ContactModule.UnitTest;

public abstract class BaseContactModuleTest<T>
{
    protected readonly Mock<IContactCommandRepo> _contactCommandRepoMock;
    protected readonly Mock<IContactCommonQueryRepo> _contactCommonQueryRepoMock;
    protected readonly Mock<IGetContactByIdQueryRepo> _getContactByIdQueryRepoMock;
    protected readonly Mock<IDeleteContactByIdQueryRepo> _deleteContactByIdQueryRepoMock;
    protected readonly ILogger<T> _nullLogger;

    public BaseContactModuleTest()
    {
        _contactCommandRepoMock = new Mock<IContactCommandRepo>();
        _contactCommonQueryRepoMock = new Mock<IContactCommonQueryRepo>();
        _getContactByIdQueryRepoMock = new Mock<IGetContactByIdQueryRepo>();
        _deleteContactByIdQueryRepoMock = new Mock<IDeleteContactByIdQueryRepo>();
        _nullLogger = new NullLogger<T>();
    } 
}