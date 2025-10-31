using Moq;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;

namespace CommomTestUtilities.Repository;

public static class UserAddOnlyRepositoryBuilder
{
    public static IAddOnlyUserRepository<UserEntity> Build()
    {
        var mock = new Mock<IAddOnlyUserRepository<UserEntity>>();

        mock.Setup(repo => repo.AddAsync(
                It.IsAny<UserEntity>(),
                It.IsAny<CancellationToken>()));

        return mock.Object;
    }
}
