using ContactManagement.WebAPI.Modules.FundModule.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace ContactManagement.FundModule.UnitTest;

public class AssignContactHandler : BaseFundModuleTest<AssignContact>
{
    [Fact]
    public async Task AssignContact_ReturnsNotFound_WhenContactDoesNotExist()
    {
        // Arrange
        var request = new AssignContact.Request(Guid.NewGuid(), Guid.NewGuid());
        
        _assignContactQueryRepoMock.Setup(repo => repo.ContactExists(request.ContactId, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var result = await AssignContact
            .Handle(request, CancellationToken.None, _nullLogger, _fundCommonQueryRepoMock.Object,
                _assignContactQueryRepoMock.Object, _fundCommandRepoMock.Object);

        // Assert
        var response = Assert.IsType<ProblemHttpResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AssignContact_ReturnsNotFound_WhenFundDoesNotExist()
    {
        // Arrange
        var request = new AssignContact.Request(Guid.NewGuid(), Guid.NewGuid());
        
        _assignContactQueryRepoMock.Setup(repo => repo.ContactExists(request.ContactId, CancellationToken.None))
            .ReturnsAsync(true);
        _fundCommonQueryRepoMock.Setup(repo => repo.FundExists(request.FundId, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var response = await AssignContact
            .Handle(request, CancellationToken.None, _nullLogger, _fundCommonQueryRepoMock.Object, 
                _assignContactQueryRepoMock.Object, _fundCommandRepoMock.Object);

        // Assert
        var problemResult = Assert.IsType<ProblemHttpResult>(response.Result);
        Assert.Equal(StatusCodes.Status404NotFound, problemResult.StatusCode);
    }

    [Fact]
    public async Task AssignContact_ReturnsUnprocessableEntity_WhenContactIsAlreadyAssigned()
    {
        // Arrange
        var request = new AssignContact.Request(Guid.NewGuid(), Guid.NewGuid());
        
        _assignContactQueryRepoMock.Setup(repo => repo.ContactExists(request.ContactId, CancellationToken.None))
            .ReturnsAsync(true);
        _fundCommonQueryRepoMock.Setup(repo => repo.FundExists(request.FundId, CancellationToken.None))
            .ReturnsAsync(true);
        _assignContactQueryRepoMock.Setup(repo => repo.ContactIsAssignedToFund(request.FundId, request.ContactId, CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var response = await AssignContact
            .Handle(request, CancellationToken.None, _nullLogger, _fundCommonQueryRepoMock.Object, 
                _assignContactQueryRepoMock.Object, _fundCommandRepoMock.Object);

        // Assert
        var problemHttpResult = Assert.IsType<ProblemHttpResult>(response.Result);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, problemHttpResult.StatusCode);
    }

    [Fact]
    public async Task AssignContact_ReturnsOk_WhenContactIsSuccessfullyAssigned()
    {
        // Arrange
        var request = new AssignContact.Request(Guid.NewGuid(), Guid.NewGuid());

        _assignContactQueryRepoMock.Setup(repo => repo.ContactExists(request.ContactId, CancellationToken.None))
            .ReturnsAsync(true);
        _fundCommonQueryRepoMock.Setup(repo => repo.FundExists(request.FundId, CancellationToken.None))
            .ReturnsAsync(true);
        _assignContactQueryRepoMock.Setup(repo => repo.ContactIsAssignedToFund(request.FundId, request.ContactId, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var response = await AssignContact
            .Handle(request, CancellationToken.None, _nullLogger, _fundCommonQueryRepoMock.Object, 
                _assignContactQueryRepoMock.Object, _fundCommandRepoMock.Object);
        
        // Assert
        Assert.IsType<Ok>(response.Result);
    }
}