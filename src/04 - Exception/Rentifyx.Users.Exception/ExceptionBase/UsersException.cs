using System.Collections.ObjectModel;

namespace Rentifyx.Users.Exceptions.ExceptionBase;

public abstract class UsersException : Exception
{
    protected UsersException() { }

    protected UsersException(string message)
        : base(message) { }

    protected UsersException(string message, Exception innerException)
        : base(message, innerException) { }

    public abstract int StatusCode { get; }

    public abstract ReadOnlyCollection<string> GetErrors();
}
