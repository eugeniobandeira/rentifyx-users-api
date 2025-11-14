using ErrorOr;
using Rentifyx.Users.Domain.Entities;

namespace Rentifyx.Users.Domain.Interfaces.User;

public interface IReadOnlyUserRepository
{
    Task<ErrorOr<UserEntity>> GetByDocumentAsync(string document, CancellationToken cancellationToken);
}
