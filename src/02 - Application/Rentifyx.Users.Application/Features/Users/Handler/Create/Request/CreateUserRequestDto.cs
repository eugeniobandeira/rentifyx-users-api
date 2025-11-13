using Rentifyx.Users.Application.Commom.Dto;

namespace Rentifyx.Users.Application.Features.Users.Handler.Create.Request;

public record CreateUserRequestDto(
    string Document,
    string Name,
    string Email,
    AddressRequestDto AddressRequestDto,
    string? ProfileImageFileName);

