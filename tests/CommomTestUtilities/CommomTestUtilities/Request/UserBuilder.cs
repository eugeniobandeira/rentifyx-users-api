using Bogus;
using Bogus.Extensions.Brazil;
using Rentifyx.Users.Domain.Entities;

using Rentifyx.Users.Domain.ValueObjects;
using System.Security.Cryptography;

namespace CommomTestUtilities.Request;

public static class UserBuilder
{
    public static UserEntity Build()
    {
        var faker = new Faker<UserEntity>("pt_BR")
            .RuleFor(c => c.Document, f =>
            {
                var randomNumber = RandomNumberGenerator.GetInt32(0, 2);
                var document = randomNumber == 0
                    ? f.Person.Cpf()
                    : f.Company.Cnpj();

                return document
                    .Replace(".", "", StringComparison.Ordinal)
                    .Replace("-", "", StringComparison.Ordinal)
                    .Replace("/", "", StringComparison.Ordinal);
            })
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Address, f =>
            {

                return Address.Builder()
                    .WithStreet(f.Address.StreetName())
                    .WithNumber(("10"))
                    .WithNeighborhood(f.Address.County())
                    .WithCity(f.Address.City())
                    .WithState(f.Address.StateAbbr())
                    .WithZipCode(f.Address.ZipCode().Replace("-", "", StringComparison.Ordinal))
                    .WithComplement(f.Random.Bool() ? f.Address.SecondaryAddress() : null)
                    .Build();
            })
            .RuleFor(c => c.ProfileImage, f => ProfileImage.Create("profile.jpg", "rentifyx-bucket"));

        return faker.Generate();
    }
}
