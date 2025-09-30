namespace Rentifyx.Clients.Domain.Shared.Results;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure  => !IsSuccess;
    public T? Value { get; }
    public string? Error { get; }
    public IReadOnlyList<string>? Errors { get; }

    protected Result(bool isSuccess, T? value, string? error, IReadOnlyList<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        Errors = errors;
    }
    
    public static Result<T> Success(T value)
    {
        return new Result<T>(
            isSuccess: true, 
            value: value, 
            error: null);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(
            isSuccess: false, 
            value: default, 
            error: error);
    }

    public static Result<T> ValidationFailure(IReadOnlyList<string> errors)
    {
        var errorMessage = string.Join(", ", errors);

        return new Result<T>(
            isSuccess: false, 
            value: default, 
            error: errorMessage, 
            errors: errors);
    }
}

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }
    public IReadOnlyList<string>? Errors { get; }

    protected Result(bool isSuccess, string? error, IReadOnlyList<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = errors;
    }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(string error)
    {
        return new Result(false, error);
    }

    public static Result ValidationFailure(IReadOnlyList<string> errors)
    {
        var errorMessage = string.Join(", ", errors);
        return new Result(false, errorMessage, errors);
    }
}
