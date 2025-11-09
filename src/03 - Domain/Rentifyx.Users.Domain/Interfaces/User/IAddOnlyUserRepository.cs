using ErrorOr;
using Rentifyx.Users.Domain.Entities;

namespace Rentifyx.Users.Domain.Interfaces.User;

public interface IAddOnlyUserRepository<in TEntity> 
    where TEntity : class
{
    Task<ErrorOr<UserEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
}
