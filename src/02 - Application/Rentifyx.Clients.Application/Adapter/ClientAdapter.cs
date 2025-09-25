using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentifyx.Clients.Domain.Entities;

namespace Rentifyx.Clients.Application.Adapter;

public static class ClientAdapter
{
    public static ClientEntity FromRequestToEntity(CreateClientRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new ClientEntity()
        {
            Name = request.Name,
            Document = request.Document,
            Email = request.Email
        };
    }
}
