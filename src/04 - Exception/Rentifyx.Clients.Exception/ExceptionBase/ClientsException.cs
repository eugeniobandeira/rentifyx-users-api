using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System;

namespace Rentifyx.Clients.Exceptions.ExceptionBase;

public abstract class ClientsException : Exception
{
    protected ClientsException() { }

    protected ClientsException(string message)
        : base(message) { }

    protected ClientsException(string message, Exception innerException)
        : base(message, innerException) { }

    public abstract int StatusCode { get; }

    public abstract ReadOnlyCollection<string> GetErrors();
}
