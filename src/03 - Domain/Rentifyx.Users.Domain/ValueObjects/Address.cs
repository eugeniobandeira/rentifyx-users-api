namespace Rentifyx.Users.Domain.ValueObjects;

public sealed class Address
{
    public string Street { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;
    public string Neighborhood { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public string? Complement { get; private set; }

    private Address() { }

    public static AddressBuilder Builder() => new();

    public sealed class AddressBuilder
    {
        private readonly Address _address = new();

        public AddressBuilder WithStreet(string street)
        {
            _address.Street = street;
            return this;
        }

        public AddressBuilder WithNumber(string number)
        {
            _address.Number = number;
            return this;
        }

        public AddressBuilder WithNeighborhood(string neighborhood)
        {
            _address.Neighborhood = neighborhood;
            return this;
        }

        public AddressBuilder WithCity(string city)
        {
            _address.City = city;
            return this;
        }

        public AddressBuilder WithState(string state)
        {
            _address.State = state;
            return this;
        }

        public AddressBuilder WithZipCode(string zipCode)
        {
            _address.ZipCode = zipCode;
            return this;
        }

        public AddressBuilder WithComplement(string? complement)
        {
            _address.Complement = complement;
            return this;
        }

        public Address Build()
        {
            if (string.IsNullOrWhiteSpace(_address.Street))
                throw new InvalidOperationException("Street is required.");
            if (string.IsNullOrWhiteSpace(_address.City))
                throw new InvalidOperationException("City is required.");
            if (string.IsNullOrWhiteSpace(_address.State))
                throw new InvalidOperationException("State is required.");

            return _address;
        }
    }
}
