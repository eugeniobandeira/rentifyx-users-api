using System.Globalization;

namespace Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;

public record CreateClientRequestDto(
    string Document,
    string Name, 
    string Email);
