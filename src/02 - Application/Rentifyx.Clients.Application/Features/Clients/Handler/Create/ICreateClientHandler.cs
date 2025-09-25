using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentifyx.Clients.Domain.Entities;

namespace Rentifyx.Clients.Application.Features.Clients.Handler.Create;

public interface ICreateClientHandler
{
    Task<ClientEntity> RegisterClientAsync(
        CreateClientRequestDto request, 
        CancellationToken cancellationToken = default);
}
