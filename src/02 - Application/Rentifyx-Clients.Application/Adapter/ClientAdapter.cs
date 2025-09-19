using Rentifyx.Clients.Application.Features.Clients.Request;
using Rentifyx.Clients.Domain.Entities;

namespace Rentifyx.Clients.Application.Adapter;

public static class ClientAdapter
{
    public static ClientEntity FromRequestToEntity(CreateClientRequest request)
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
