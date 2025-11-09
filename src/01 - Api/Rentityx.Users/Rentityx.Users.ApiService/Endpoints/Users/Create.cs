using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Rentifyx.Users.Application.Features.Users.Handler.Create;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentityx.Users.ApiService.Abstract;
using Rentityx.Users.ApiService.Extensions;
using System.Reflection.Metadata;

namespace Rentityx.Users.ApiService.Endpoints.Users;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/users", Handle)
           .WithTags(Tags.Users)
           .WithOpenApi();
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateUserRequestDto request,
        [FromServices] ICreateUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.CreateUsersAsync(request, cancellationToken);

        return result.Match(
            client => Results.Created($"/api/v1/users/{client.Document}", new
            {
                Id = client.Document,
                Message = "Created user successfuly."
            }),
            errors => errors.ToProblem()
        );
    }
}
