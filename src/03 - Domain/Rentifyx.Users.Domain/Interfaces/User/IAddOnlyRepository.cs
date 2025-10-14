using Rentifyx.Users.Domain.Entities;

namespace Rentifyx.Users.Domain.Interfaces.User;

public interface IAddOnlyRepository<TEntity> 
    where TEntity : class
{
    Task<bool> AddAsync(TEntity entity, string tableName, CancellationToken cancellationToken = default);
}
