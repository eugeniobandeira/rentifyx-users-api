using Moq;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;

namespace CommomTestUtilities.Repository;

public static class UserAddOnlyRepositoryBuilder
{
    public static IAddOnlyRepository<UserEntity> Build()
    {
        var mock = new Mock<IAddOnlyRepository<UserEntity>>();

        mock.Setup(repo => repo.AddAsync(
                It.IsAny<UserEntity>(),
                It.IsAny<CancellationToken>()));

        return mock.Object;
    }
}
