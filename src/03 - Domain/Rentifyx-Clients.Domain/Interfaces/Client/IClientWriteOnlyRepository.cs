using Rentifyx.Clients.Domain.Entities;

namespace Rentifyx.Clients.Domain.Interfaces.Client;

public interface IClientWriteOnlyRepository
{
    Task AddAsync(ClientEntity clientEntity, CancellationToken cancellationToken = default);
}
