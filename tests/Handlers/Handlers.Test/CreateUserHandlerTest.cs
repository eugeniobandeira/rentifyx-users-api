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

    //[Fact]
    //public async Task Should_Not_Create_Client_When_Document_Is_Invalid()
    //{
    //    // Arrange
    //    var client = UserBuilder.Build();

    //    var request = new CreateUserRequestDto(
    //        "123456",
    //        client.Name,
    //        client.Email);

    //    var handler = CreateHandler();

    //    // Act
    //    var act = async () => await handler.CreateUsersAsync(request, CancellationToken.None);

    //    // Assert
    //    var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

    //    result.Where(error 
    //        => error.GetErrors().Count == 1
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.DOCUMENT_LENGTH));
    //}

    //[Fact]
    //public async Task Should_Not_Create_Client_When_Document_Is_WhiteSpace()
    //{
    //    // Arrange
    //    var client = UserBuilder.Build();

    //    var request = new CreateUserRequestDto(
    //        "   ",
    //        client.Name,
    //        client.Email);

    //    var handler = CreateHandler();

    //    // Act
    //    var act = async () => await handler.CreateUsersAsync(request, CancellationToken.None);

    //    // Assert
    //    var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

    //    result.Where(error
    //        => error.GetErrors().Count == 2
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.EMPTY_DOCUMENT)
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.DOCUMENT_LENGTH));
    //}

    //[Fact]
    //public async Task Should_Not_Create_Client_When_Name_Is_WhiteSpace()
    //{
    //    // Arrange
    //    var client = UserBuilder.Build();

    //    var request = new CreateUserRequestDto(
    //        client.Document,
    //        "",
    //        client.Email);

    //    var handler = CreateHandler();

    //    // Act
    //    var act = async () => await handler.CreateUsersAsync(request, CancellationToken.None);

    //    // Assert
    //    var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

    //    result.Where(error
    //        => error.GetErrors().Count == 2
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.EMPTY_NAME)
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.NAME_MIN_LENGTH));
    //}

    //[Fact]
    //public async Task Should_Not_Create_Client_When_Name_Min_Length_Is_Invalid()
    //{
    //    // Arrange
    //    var client = UserBuilder.Build();

    //    var request = new CreateUserRequestDto(
    //        client.Document,
    //        "May",
    //        client.Email);

    //    var handler = CreateHandler();

    //    // Act
    //    var act = async () => await handler.CreateUsersAsync(request, CancellationToken.None);

    //    // Assert
    //    var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

    //    result.Where(error
    //        => error.GetErrors().Count == 1
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.NAME_MIN_LENGTH));
    //}

    //[Fact]
    //public async Task Should_Not_Create_Client_When_Name_Exceeds_100_Characters()
    //{
    //    // Arrange
    //    var client = UserBuilder.Build();
    //    string invalidLengthName = "fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name";

    //    var request = new CreateUserRequestDto(
    //        client.Document,
    //        invalidLengthName,
    //        client.Email);

    //    var handler = CreateHandler();

    //    // Act
    //    var act = async () => await handler.CreateUsersAsync(request, CancellationToken.None);

    //    // Assert
    //    var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

    //    result.Where(error
    //        => error.GetErrors().Count == 1
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.NAME_MAX_LENGTH));
    //}

    //[Fact]
    //public async Task Should_Not_Create_Client_When_Email_Is_WhiteSpace()
    //{
    //    // Arrange
    //    var client = UserBuilder.Build();

    //    var request = new CreateUserRequestDto(
    //        client.Document,
    //        client.Name,
    //        "   ");

    //    var handler = CreateHandler();

    //    // Act
    //    var act = async () => await handler.CreateUsersAsync(request, CancellationToken.None);

    //    // Assert
    //    var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

    //    result.Where(error
    //        => error.GetErrors().Count == 2
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.EMPTY_EMAIL)
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS));
    //}

    //[Fact]
    //public async Task Should_Not_Create_Client_When_Email_Is_Invalid()
    //{
    //    // Arrange
    //    var client = UserBuilder.Build();

    //    var request = new CreateUserRequestDto(
    //        client.Document,
    //        client.Name,
    //        "test.com.br");

    //    var handler = CreateHandler();

    //    // Act
    //    var act = async () => await handler.CreateUsersAsync(request, CancellationToken.None);

    //    // Assert
    //    var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

    //    result.Where(error
    //        => error.GetErrors().Count == 1
    //        && error.GetErrors().Contains(UserValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS));
    //}

    private static CreateUserHandler CreateHandler()
    {
        var addOnlyRepository = UserAddOnlyRepositoryBuilder.Build();
        var validator = CreateUserValidatorBuilder.Build();
        var logger  = LoggerBuilder<CreateUserHandler>.Build();
        var configuration = ConfigurationBuilder.Build();

        return new CreateUserHandler(validator, addOnlyRepository, logger, configuration);
    }
}
