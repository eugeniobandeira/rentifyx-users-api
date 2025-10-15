using Microsoft.Extensions.Configuration;
using Moq;

namespace CommomTestUtilities.Configuration;

public static class ConfigurationBuilder
{
    public static IConfiguration Build()
    {
        var tableName = "rentifyx-users-dev";

        var mock = new Mock<IConfiguration>();

        mock.Setup(x => x["AWS:Tables:Users"])
            .Returns(tableName);

        return mock.Object;
    }
}
