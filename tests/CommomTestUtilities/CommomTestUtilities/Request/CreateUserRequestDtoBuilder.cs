using Bogus;
using Bogus.Extensions.Brazil;
using Rentifyx.Users.Application.Commom.Dto;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using System.Security.Cryptography;

namespace CommomTestUtilities.Request;

public class CreateUserRequestDtoBuilder
{
    private string? _document;
    private string? _name;
    private string? _email;
    private AddressRequestDto? _address;
    private string? _profileImageFileName;

    public static CreateUserRequestDtoBuilder Builder() => new();

    public CreateUserRequestDtoBuilder WithDocument(string document)
    {
        _document = document;
        return this;
    }

    public CreateUserRequestDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CreateUserRequestDtoBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public CreateUserRequestDtoBuilder WithAddress(AddressRequestDto address)
    {
        _address = address;
        return this;
    }

    public CreateUserRequestDtoBuilder WithProfileImageFileName(string profileImageFileName)
    {
        _profileImageFileName = profileImageFileName;
        return this;
    }

    public CreateUserRequestDto Build()
    {
        var faker = new Faker("pt_BR");

        var document = _document ?? GenerateRandomDocument(faker);
        var name = _name ?? faker.Name.FullName();
        var email = _email ?? faker.Internet.Email();
        var address = _address ?? GenerateRandomAddress(faker);

        return new CreateUserRequestDto(
            document,
            name,
            email,
            address,
            _profileImageFileName!);
    }

    private static string GenerateRandomDocument(Faker faker)
    {
        var randomNumber = RandomNumberGenerator.GetInt32(0, 2);
        var document = randomNumber == 0
            ? faker.Person.Cpf()
            : faker.Company.Cnpj();

        return document
            .Replace(".", "", StringComparison.Ordinal)
            .Replace("-", "", StringComparison.Ordinal)
            .Replace("/", "", StringComparison.Ordinal);
    }

    private static AddressRequestDto GenerateRandomAddress(Faker faker)
    {
        return new AddressRequestDto(
            Street: faker.Address.StreetName(),
            Number: faker.Random.AlphaNumeric(4),
            Neighborhood: faker.Address.County(),
            City: faker.Address.City(),
            State: faker.Address.StateAbbr(),
            ZipCode: faker.Address.ZipCode().Replace("-", "", StringComparison.Ordinal),
            Complement: faker.Random.Bool() ? faker.Address.SecondaryAddress() : null);
    }
}
