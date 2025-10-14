using ErrorOr;
using System.Net;

namespace Rentityx.Users.ApiService.Extensions;

public static class ErrorOrExtensions
{
    public static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Count is 0)
            return Results.Problem();

        var firstError = errors[0];

        return firstError.Type switch
        {
            ErrorType.Validation => CreateValidationProblem(errors),
            ErrorType.NotFound => Results.NotFound(firstError.Description),
            ErrorType.Conflict => Results.Conflict(firstError.Description),
            ErrorType.Unauthorized => Results.Unauthorized(),
            _ => Results.Problem(firstError.Description)
        };
    }

    public static IResult ToResult<T>(this ErrorOr<T> errorOr) =>
        errorOr.Match(Results.Ok, errors => errors.ToProblem());

    private static IResult CreateValidationProblem(List<Error> errors)
    {
        var errorsDict = errors
            .GroupBy(error => error.Code)
            .ToDictionary(group => 
                group.Key, g => g.Select(e => e.Description).ToArray());

        return Results.ValidationProblem(
            errorsDict,
            statusCode: (int)HttpStatusCode.UnprocessableEntity
        );
    }
}
