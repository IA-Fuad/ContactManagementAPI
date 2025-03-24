using ContactManagement.WebAPI.Modules.FundModule.Repo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ContactManagement.FundModule.UnitTest;

public class BaseFundModuleTest<T>
{
    protected Mock<IFundCommonQueryRepo> _fundCommonQueryRepoMock;
    protected Mock<IGetFundContactQueryRepo> _getFundContactQueryRepoMock;
    protected Mock<IAssignContactQueryRepo> _assignContactQueryRepoMock;
    protected Mock<IFundCommandRepo> _fundCommandRepoMock;
    protected ILogger<T> _nullLogger;

    protected BaseFundModuleTest()
    {
        _fundCommonQueryRepoMock = new Mock<IFundCommonQueryRepo>();
        _getFundContactQueryRepoMock = new Mock<IGetFundContactQueryRepo>();
        _assignContactQueryRepoMock = new Mock<IAssignContactQueryRepo>();
        _fundCommandRepoMock = new Mock<IFundCommandRepo>();
        _nullLogger = new NullLogger<T>();
    }
}