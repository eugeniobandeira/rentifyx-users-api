using Microsoft.AspNetCore.Mvc;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;

namespace Rentityx.Clients.ApiService.Endpoints.Clients;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("clients", async (
            [FromBody] CreateClientRequestDto dto,
            [FromServices] ICreateClientHandler handler,
            CancellationToken cancellationToken) =>
        {
            
            var result = await handler.RegisterClientAsync(dto, cancellationToken);

            return Results.Ok(result);
        })
        .WithTags(Tags.Clients)
        .WithOpenApi();
    }
}
