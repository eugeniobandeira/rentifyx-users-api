using ErrorOr;
using Rentifyx.Clients.Domain.Shared.Results;

namespace Rentityx.Clients.ApiService.Extensions;

public static class ErrorOrExtensions
{
    public static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Count is 0)
            return Results.Problem();

        var statusCode = errors[0].Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.ValidationProblem(
            errors.ToDictionary(key => key.Code, value => new[] { value.Description }),
            statusCode: statusCode
        );
    }
}
