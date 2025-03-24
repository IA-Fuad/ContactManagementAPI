using ContactManagement.Data.Models;
using ContactManagement.WebAPI.Modules.ContactModule.Endpoints;
using ContactManagement.WebAPI.Modules.ContactModule.SharedDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace ContactManagement.ContactModule.UnitTest;

public class CreateContactHandler : BaseContactModuleTest<CreateContact>
{
    [Fact]
    public async Task CreateContact_ReturnsOkContactResponse_WhenContactIsCreated()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new CreateContact.Request("firstname", "lastname", "email", "phone");
        
        _contactCommandRepoMock.Setup(r => r.CreateContact(It.IsAny<Contact>(), cancellationToken))
            .ReturnsAsync(true);

        // Act
        var response = await CreateContact.Handle(request, cancellationToken, _nullLogger, _contactCommandRepoMock.Object);
        
        // Assert
        var okResult = Assert.IsType<Ok<ContactResponse>>(response.Result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal("firstname", okResult.Value!.FirstName);
        Assert.Equal("lastname", okResult.Value.LastName);
        Assert.Equal("email", okResult.Value.Email);
        Assert.Equal("phone", okResult.Value.Phone);
    }

    [Fact]
    public async Task CreateContact_ReturnsError_WhenContactIsNotCreated()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new CreateContact.Request("firstname", "lastname", "email", "phone");
        
        _contactCommandRepoMock.Setup(r => r.CreateContact(It.IsAny<Contact>(), cancellationToken))
            .ReturnsAsync(false);

        // Act
        var response = await CreateContact.Handle(request, cancellationToken, _nullLogger, _contactCommandRepoMock.Object);
        
        // Asset
        var errorResult = Assert.IsType<ProblemHttpResult>(response.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, errorResult.StatusCode);
    }
}