namespace Rentifyx.Users.Application.Commom.Dto;

public record AddressRequestDto(
    string Street,
    string Number,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string? Complement);
