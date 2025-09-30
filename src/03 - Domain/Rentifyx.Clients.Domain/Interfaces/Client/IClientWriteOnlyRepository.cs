using Rentifyx.Clients.Domain.Entities;

namespace Rentifyx.Clients.Domain.Interfaces.Client;

public interface IClientWriteOnlyRepository
{
    Task<bool> AddAsync(ClientEntity clientEntity, CancellationToken cancellationToken = default);
}
