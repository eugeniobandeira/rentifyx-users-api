using Rentifyx.Clients.Domain.Entities;
using Rentifyx.Clients.Domain.Interfaces.Client;

namespace Rentifyx.Clients.Infrastructure.Repositories;

internal class ClientRepository : IClientWriteOnlyRepository
{
    public Task AddAsync(ClientEntity clientEntity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
