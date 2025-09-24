using Microsoft.Extensions.Logging;
using Moq;

namespace CommomTestUtilities.Logger;

public static class LoggerBuilder<T> where T : class
{
    public static ILogger<T> Build()
    {
        var logger = new Mock<ILogger<T>>();

        return logger.Object;
    }
}
