using Microsoft.AspNetCore.Mvc;
using Rentifyx.Users.ApiService.Extensions;
using Rentifyx.Users.Application.Features.Users.Handler.GetByDocument;
using Rentityx.Users.ApiService.Abstract;
using Rentityx.Users.ApiService.Endpoints;

namespace Rentifyx.Users.ApiService.Endpoints.Users;

internal sealed class GetByDocument : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/users/{document}", HandleAsync)
           .WithTags(Tags.Users)
           .WithOpenApi();
    }

    private static async Task<IResult> HandleAsync(
        [FromRoute] string document,
        [FromServices] IGetUserByDocumentHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.GetUserByDocumentAsync(document, cancellationToken);

        return result.Match(
            client => Results.Ok(client),
            errors => errors.ToProblem()
        );
    }
}
