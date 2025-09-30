using ErrorOr;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentifyx.Clients.Domain.Entities;
using Rentifyx.Clients.Domain.Shared.Results;

namespace Rentifyx.Clients.Application.Features.Clients.Handler.Create;

public interface ICreateClientHandler
{
    Task<ErrorOr<ClientEntity>> RegisterClientAsync(
        CreateClientRequestDto request, 
        CancellationToken cancellationToken = default);
}
