using Moq;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;

namespace CommomTestUtilities.Repository;

public static class UserWriteOnlyRepositoryBuilder
{
    public static IAddOnlyRepository<UserEntity> Build()
    {
        var mock = new Mock<IAddOnlyRepository<UserEntity>>();

        return mock.Object;
    }
}
