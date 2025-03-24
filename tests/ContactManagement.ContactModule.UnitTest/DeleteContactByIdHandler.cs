using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Modules.ContactModule.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace ContactManagement.ContactModule.UnitTest;

public class DeleteContactByIdHandler : BaseContactModuleTest<DeleteContactById>
{
    [Fact]
    public async Task DeleteContactByIdHandler_ReturnsNotFound_WhenContactDoesntExists()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        
        _contactCommonQueryRepoMock.Setup(r => r.GetContactById(It.Is<Guid>(id => id == contactId), CancellationToken.None))
            .ReturnsAsync(() => null);
        
        // Act
        var response = await DeleteContactById
            .Handle(contactId, CancellationToken.None, _nullLogger, _contactCommonQueryRepoMock.Object,
                _deleteContactByIdQueryRepoMock.Object, _contactCommandRepoMock.Object);
        
        // Assert
        var notFoundResponse = Assert.IsType<NotFound>(response.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteContactByIdHandler_ReturnsProblem_WhenContactIsAssignedToFund()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var contact = Contact.Create("firstname", "lastname", "email", "phone");
        
        _contactCommonQueryRepoMock.Setup(r => r.GetContactById(contactId, CancellationToken.None))
            .ReturnsAsync(() => contact);
        _deleteContactByIdQueryRepoMock.Setup(r => r.IsAssignedToFund(It.Is<Guid>(id => id == contactId), CancellationToken.None))
            .ReturnsAsync(true);
        
        // Act
        var response = await DeleteContactById
            .Handle(contactId, CancellationToken.None, _nullLogger, _contactCommonQueryRepoMock.Object, 
                _deleteContactByIdQueryRepoMock.Object, _contactCommandRepoMock.Object);
        
        // Assert
        var problemResponse = Assert.IsType<ProblemHttpResult>(response.Result);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, problemResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteContactByIdHandler_ReturnsOk_WhenContactIsDeleted()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var contact = Contact.Create("firstname", "lastname", "email", "phone");
        
        _contactCommonQueryRepoMock.Setup(r => r.GetContactById(contactId, CancellationToken.None))
            .ReturnsAsync(() => contact);
        _deleteContactByIdQueryRepoMock.Setup(r => r.IsAssignedToFund(It.Is<Guid>(id => id == contactId), CancellationToken.None))
            .ReturnsAsync(false);
        
        // Act
        var response = await DeleteContactById
            .Handle(contactId, CancellationToken.None, _nullLogger, _contactCommonQueryRepoMock.Object, 
                _deleteContactByIdQueryRepoMock.Object, _contactCommandRepoMock.Object);
        
        // Assert
        var problemResponse = Assert.IsType<Ok>(response.Result);
        Assert.Equal(StatusCodes.Status200OK, problemResponse.StatusCode);
    }
}