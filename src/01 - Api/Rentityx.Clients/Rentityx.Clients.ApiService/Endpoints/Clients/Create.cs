using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentityx.Clients.ApiService.Abstract;
using Rentityx.Clients.ApiService.Extensions;
using System.Reflection.Metadata;

namespace Rentityx.Clients.ApiService.Endpoints.Clients;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/clients", Handle)
           .WithTags(Tags.Clients)
           .WithOpenApi();
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateClientRequestDto request,
        [FromServices] ICreateClientHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.RegisterClientAsync(request, cancellationToken);

        return result.Match(
            client => Results.Created($"/api/v1/clients/{client.Document}", new
            {
                Id = client.Document,
                Message = "Created client successfuly."
            }),
            errors => errors.ToProblem()
        );
    }

}
