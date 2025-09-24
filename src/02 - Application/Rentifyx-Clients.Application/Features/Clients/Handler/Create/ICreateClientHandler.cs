using Rentifyx.Clients.Application.Features.Clients.Request;
using Rentifyx.Clients.Domain.Entities;

namespace Rentifyx.Clients.Application.Features.Clients.Handler.Create;

internal interface ICreateClientHandler
{
    Task<ClientEntity> RegisterClientAsync(
        CreateClientRequestDto request, 
        CancellationToken cancellationToken = default);
}
