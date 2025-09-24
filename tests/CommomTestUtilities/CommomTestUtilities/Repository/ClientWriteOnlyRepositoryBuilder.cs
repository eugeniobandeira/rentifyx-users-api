using Moq;
using Rentifyx.Clients.Domain.Interfaces.Client;

namespace CommomTestUtilities.Repository;

public static class ClientWriteOnlyRepositoryBuilder
{
    public static IClientWriteOnlyRepository Build()
    {
        var mock = new Mock<IClientWriteOnlyRepository>();

        return mock.Object;
    }
}
