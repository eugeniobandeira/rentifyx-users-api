using ErrorOr;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentifyx.Users.Domain.Entities;

namespace Rentifyx.Users.Application.Features.Users.Handler.Create;

public interface ICreateUserHandler
{
    Task<ErrorOr<UserEntity>> CreateUsersAsync(
        CreateUserRequestDto request, 
        CancellationToken cancellationToken = default);
}
