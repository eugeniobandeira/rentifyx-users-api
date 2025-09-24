using System.Collections.ObjectModel;
using System.Net;

namespace Rentifyx.Clients.Exceptions.ExceptionBase;

public class ErrorOnValidationException : ClientsException
{
    private readonly ReadOnlyCollection<string> _errors;

    public ErrorOnValidationException(ReadOnlyCollection<string> errorMessages)
        : base(string.Empty)
    {
        _errors = errorMessages;
    }

    public ErrorOnValidationException()
        : base(string.Empty)
    {
        _errors = new ReadOnlyCollection<string>(Array.Empty<string>());
    }

    public ErrorOnValidationException(string message) 
        : base(message)
    {
        _errors = new ReadOnlyCollection<string>(Array.Empty<string>());
    }

    public ErrorOnValidationException(string message, Exception innerException) 
        : base(message, innerException)
    {
        _errors = new ReadOnlyCollection<string>(Array.Empty<string>());
    }

    public override int StatusCode 
        => (int)HttpStatusCode.BadRequest;

    public override ReadOnlyCollection<string> GetErrors()
    {
        return _errors;
    }
}
