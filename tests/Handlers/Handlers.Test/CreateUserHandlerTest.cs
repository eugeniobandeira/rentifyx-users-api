using CommomTestUtilities.Configuration;
using CommomTestUtilities.Logger;
using CommomTestUtilities.Repository;
using CommomTestUtilities.Request;
using CommomTestUtilities.Validator;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Rentifyx.Users.Application.Features.Users.Handler.Create;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Request;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;
using Rentifyx.Users.Exceptions.ExceptionBase;
using Rentifyx.Users.Exceptions.MessageResource;
using System.ComponentModel.DataAnnotations;

namespace Handlers.Test;

public class CreateUserHandlerTest
{
    [Fact]
    public async Task Should_Create_Client_When_Request_Is_Valid()
    {
        // Arrange
        var client = UserBuilder.Build();

        var request = new CreateUserRequestDto(
            client.Document,
            client.Name,
            client.Email);

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Document.Should().Be(client.Document);
        result.Value.Name.Should().Be(client.Name);
        result.Value.Email.Should().Be(client.Email);
    }

    private static CreateUserHandler CreateHandler()
    {
        var addOnlyRepository = UserAddOnlyRepositoryBuilder.Build();
        var validator = CreateUserValidatorBuilder.Build();
        var logger = LoggerBuilder<CreateUserHandler>.Build();
        var configuration = ConfigurationBuilder.Build();

        return new CreateUserHandler(validator, addOnlyRepository, logger, configuration);
    }
}
