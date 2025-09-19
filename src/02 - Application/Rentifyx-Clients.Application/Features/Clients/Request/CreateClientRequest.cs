using System.Globalization;

namespace Rentifyx.Clients.Application.Features.Clients.Request;

public record CreateClientRequest(
    string Document,
    string Name, 
    string Email);
