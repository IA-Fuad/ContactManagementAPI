using ContactManagement.WebAPI.Modules.ContactModule.Endpoints;
using ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace ContactManagement.ContactModule.UnitTest;

public class GetContactByIdHandler : BaseContactModuleTest<GetContactById>
{
    [Fact]
    public async Task GetContactById_ReturnsOk_WhenContactExists()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var contactResponse = new ContactResponse(contactId, "John", "Doe", "john.doe@example.com", "1234567890");
        _getContactByIdQueryRepoMock.Setup(repo => repo.GetContactById(contactId, CancellationToken.None))
            .ReturnsAsync(contactResponse);

        // Act
        var result = await GetContactById.Handle(contactId, CancellationToken.None, _nullLogger, _getContactByIdQueryRepoMock.Object);

        // Assert
        var okResult = Assert.IsType<Ok<ContactResponse>>(result.Result);
        Assert.Equal(contactResponse, okResult.Value);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_WhenContactDoesNotExist()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        _getContactByIdQueryRepoMock.Setup(repo => repo.GetContactById(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ContactResponse?)null);

        // Act
        var result = await GetContactById.Handle(contactId, CancellationToken.None, _nullLogger, _getContactByIdQueryRepoMock.Object);

        // Assert
        Assert.IsType<NotFound>(result.Result);
    }
}