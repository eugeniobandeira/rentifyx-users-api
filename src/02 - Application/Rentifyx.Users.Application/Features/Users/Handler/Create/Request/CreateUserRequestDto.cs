using System.Globalization;

namespace Rentifyx.Users.Application.Features.Users.Handler.Create.Request;

public record CreateUserRequestDto(
    string Document,
    string Name, 
    string Email);
