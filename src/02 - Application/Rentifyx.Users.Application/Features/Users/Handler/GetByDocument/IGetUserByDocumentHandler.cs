using ErrorOr;
using Rentifyx.Users.Domain.Entities;

namespace Rentifyx.Users.Application.Features.Users.Handler.GetByDocument;

public interface IGetUserByDocumentHandler
{
    Task<ErrorOr<UserEntity>> GetUserByDocumentAsync(string document, CancellationToken cancellationToken);
}
