using CommomTestUtilities.Repository;
using CommomTestUtilities.Request;
using FluentAssertions;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentifyx.Clients.Exceptions.ExceptionBase;
using Rentifyx.Clients.Exceptions.MessageResource;

namespace Handlers.Test;

public class CreateClientHandlerTest
{
    [Fact]
    public async Task Should_Create_Client_When_Request_Is_Valid()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            client.Document,
            client.Name,
            client.Email);

        var handler = CreateHandler();

        // Act
        var result = await handler.RegisterClientAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Document.Should().Be(client.Document);
        result.Name.Should().Be(client.Name);
        result.Email.Should().Be(client.Email);
    }

    [Fact]
    public async Task Should_Not_Create_Client_When_Document_Is_Invalid()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            "123456",
            client.Name,
            client.Email);

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.RegisterClientAsync(request, CancellationToken.None);

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(error 
            => error.GetErrors().Count == 1
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.DOCUMENT_LENGTH));
    }

    [Fact]
    public async Task Should_Not_Create_Client_When_Document_Is_WhiteSpace()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            "   ",
            client.Name,
            client.Email);

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.RegisterClientAsync(request, CancellationToken.None);

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(error
            => error.GetErrors().Count == 2
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.EMPTY_DOCUMENT)
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.DOCUMENT_LENGTH));
    }

    [Fact]
    public async Task Should_Not_Create_Client_When_Name_Is_WhiteSpace()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            client.Document,
            "",
            client.Email);

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.RegisterClientAsync(request, CancellationToken.None);

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(error
            => error.GetErrors().Count == 2
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.EMPTY_NAME)
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.NAME_MIN_LENGTH));
    }

    [Fact]
    public async Task Should_Not_Create_Client_When_Name_Min_Length_Is_Invalid()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            client.Document,
            "May",
            client.Email);

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.RegisterClientAsync(request, CancellationToken.None);

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(error
            => error.GetErrors().Count == 1
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.NAME_MIN_LENGTH));
    }

    [Fact]
    public async Task Should_Not_Create_Client_When_Name_Exceeds_100_Characters()
    {
        // Arrange
        var client = ClientBuilder.Build();
        string invalidLengthName = "fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name";

        var request = new CreateClientRequestDto(
            client.Document,
            invalidLengthName,
            client.Email);

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.RegisterClientAsync(request, CancellationToken.None);

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(error
            => error.GetErrors().Count == 1
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.NAME_MAX_LENGTH));
    }

    [Fact]
    public async Task Should_Not_Create_Client_When_Email_Is_WhiteSpace()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            client.Document,
            client.Name,
            "   ");

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.RegisterClientAsync(request, CancellationToken.None);

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(error
            => error.GetErrors().Count == 2
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.EMPTY_EMAIL)
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS));
    }

    [Fact]
    public async Task Should_Not_Create_Client_When_Email_Is_Invalid()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            client.Document,
            client.Name,
            "test.com.br");

        var handler = CreateHandler();

        // Act
        var act = async () => await handler.RegisterClientAsync(request, CancellationToken.None);

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(error
            => error.GetErrors().Count == 1
            && error.GetErrors().Contains(ClientValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS));
    }

    private static CreateClientHandler CreateHandler()
    {
        var writeOnlyRepository = ClientWriteOnlyRepositoryBuilder.Build();

        return new CreateClientHandler(writeOnlyRepository);
    }
}
