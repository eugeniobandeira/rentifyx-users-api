using Rentifyx.Users.Exceptions.MessageResource;

namespace Rentifyx.Users.Domain.ValueObjects;

public sealed class Address
{
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string? Complement { get; set; }

    public Address() { }

    public static AddressBuilder Builder() => new();

    public sealed class AddressBuilder
    {
        private readonly Address _address = new();

        public AddressBuilder WithStreet(string street)
        {
            _address.Street = street?.Trim() ?? string.Empty;
            return this;
        }

        public AddressBuilder WithNumber(string number)
        {
            _address.Number = number?.Trim() ?? string.Empty;
            return this;
        }

        public AddressBuilder WithNeighborhood(string neighborhood)
        {
            _address.Neighborhood = neighborhood?.Trim() ?? string.Empty;
            return this;
        }

        public AddressBuilder WithCity(string city)
        {
            _address.City = city?.Trim() ?? string.Empty;
            return this;
        }

        public AddressBuilder WithState(string state)
        {
            _address.State = state?.Trim() ?? string.Empty;
            return this;
        }

        public AddressBuilder WithZipCode(string zipCode)
        {
            _address.ZipCode = zipCode?.Trim() ?? string.Empty;
            return this;
        }

        public AddressBuilder WithComplement(string? complement)
        {
            _address.Complement = complement?.Trim();
            return this;
        }

        public Address Build()
        {
            if (string.IsNullOrWhiteSpace(_address.Street))
                throw new InvalidOperationException(UserValidatorErrorMessageResource.EMPTY_STREET);
            if (string.IsNullOrWhiteSpace(_address.Number))
                throw new InvalidOperationException(UserValidatorErrorMessageResource.EMPTY_NUMBER);
            if (string.IsNullOrWhiteSpace(_address.Neighborhood))
                throw new InvalidOperationException(UserValidatorErrorMessageResource.EMPTY_NEIGHBORHOOD);
            if (string.IsNullOrWhiteSpace(_address.City))
                throw new InvalidOperationException(UserValidatorErrorMessageResource.EMPTY_CITY);
            if (string.IsNullOrWhiteSpace(_address.State))
                throw new InvalidOperationException(UserValidatorErrorMessageResource.EMPTY_STATE);
            if (string.IsNullOrWhiteSpace(_address.ZipCode))
                throw new InvalidOperationException(UserValidatorErrorMessageResource.EMPTY_ZIPCODE);

            return _address;
        }
    }
}
