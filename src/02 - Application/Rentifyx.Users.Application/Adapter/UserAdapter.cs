using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentifyx.Users.Domain.Entities;

namespace Rentifyx.Users.Application.Adapter;

public static class UserAdapter
{
    public static UserEntity FromRequestToEntity(CreateUserRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new UserEntity()
        {
            Name = request.Name,
            Document = request.Document,
            Email = request.Email
        };
    }
}
