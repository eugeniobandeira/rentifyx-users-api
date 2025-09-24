using Bogus;
using Bogus.Extensions.Brazil;
using Rentifyx.Clients.Domain.Entities;
using System;
using System.Security.Cryptography;

namespace CommomTestUtilities.Request;

public static class ClientBuilder
{
    public static ClientEntity Build()
    {
        return new Faker<ClientEntity>("pt_BR")
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
            .Generate();
    }
}
