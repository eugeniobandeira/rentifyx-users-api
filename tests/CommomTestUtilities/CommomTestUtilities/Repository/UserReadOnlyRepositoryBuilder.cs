using Moq;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;

namespace CommomTestUtilities.Repository;

public static class UserReadOnlyRepositoryBuilder
{
    public static IReadOnlyUserRepository Build()
    {
        var mock = new Mock<IReadOnlyUserRepository>();

        mock.Setup(repo => repo.GetByDocumentAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(UserEntity));

        return mock.Object;
    }
}
