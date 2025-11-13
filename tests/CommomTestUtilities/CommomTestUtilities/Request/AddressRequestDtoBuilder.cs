using Bogus;
using Rentifyx.Users.Application.Commom.Dto;

namespace CommomTestUtilities.Request;

public class AddressRequestDtoBuilder
{
    private string? _street;
    private string? _number;
    private string? _neighborhood;
    private string? _city;
    private string? _state;
    private string? _zipCode;
    private string? _complement;

    public static AddressRequestDtoBuilder Builder() => new();

    public AddressRequestDtoBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }

    public AddressRequestDtoBuilder WithNumber(string number)
    {
        _number = number;
        return this;
    }

    public AddressRequestDtoBuilder WithNeighborhood(string neighborhood)
    {
        _neighborhood = neighborhood;
        return this;
    }

    public AddressRequestDtoBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }

    public AddressRequestDtoBuilder WithState(string state)
    {
        _state = state;
        return this;
    }

    public AddressRequestDtoBuilder WithZipCode(string zipCode)
    {
        _zipCode = zipCode;
        return this;
    }

    public AddressRequestDtoBuilder WithComplement(string? complement)
    {
        _complement = complement;
        return this;
    }

    public AddressRequestDto Build()
    {
        var faker = new Faker("pt_BR");

        return new AddressRequestDto(
            Street: _street ?? faker.Address.StreetName(),
            Number: _number ?? faker.Random.AlphaNumeric(4),
            Neighborhood: _neighborhood ?? faker.Address.County(),
            City: _city ?? faker.Address.City(),
            State: _state ?? faker.Address.StateAbbr(),
            ZipCode: _zipCode ?? faker.Address.ZipCode().Replace("-", "", StringComparison.Ordinal),
            Complement: _complement);
    }
}
