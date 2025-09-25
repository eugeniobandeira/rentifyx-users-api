using CommomTestUtilities.Request;
using FluentAssertions;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Request;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create.Validator;
using Rentifyx.Clients.Exceptions.MessageResource;

namespace Validators.Test;

public class ClientValidatorTest
{
    [Fact]
    public void Should_Be_Valid_When_Client_Is_Valid()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            client.Document,
            client.Name,
            client.Email);

        var validator = new CreateClientValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Client_Document_Is_WhiteSpace(string document)
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            document,
            client.Name,
            client.Email);

        var validator = new CreateClientValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        var totalErrors = 2;

        errorMessages.Should().Contain(ClientValidatorErrorMessageResource.EMPTY_DOCUMENT);
        errorMessages.Should().Contain(ClientValidatorErrorMessageResource.DOCUMENT_LENGTH);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Theory]
    [InlineData("1234567891")]
    [InlineData("12345678912345678")]
    [InlineData("123")]
    public void Should_Be_Invalid_When_Client_Document_Has_Invalid_Length(string document)
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            document,
            client.Name,
            client.Email);

        var validator = new CreateClientValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors
            .Should()
            .ContainSingle()
            .And
            .Contain(error => error.ErrorMessage == ClientValidatorErrorMessageResource.DOCUMENT_LENGTH);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Client_Name_Is_WhiteSpace(string name)
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            client.Document,
            name,
            client.Email);

        var validator = new CreateClientValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        var totalErrors = 2;

        errorMessages.Should().Contain(ClientValidatorErrorMessageResource.EMPTY_NAME);
        errorMessages.Should().Contain(ClientValidatorErrorMessageResource.NAME_MIN_LENGTH);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Fact]
    public void Should_Be_Invalid_When_Client_Name_Is_More_Than_100_Characters()
    {
        // Arrange
        var client = ClientBuilder.Build();
        string invalidLengthName = "fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name";

        var request = new CreateClientRequestDto(
            client.Document,
            invalidLengthName,
            client.Email);

        var validator = new CreateClientValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors
            .Should()
            .ContainSingle()
            .And
            .Contain(error => error.ErrorMessage == ClientValidatorErrorMessageResource.NAME_MAX_LENGTH);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Client_Email_Is_WhiteSpace(string email)
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            client.Document,
            client.Name,
            email);

        var validator = new CreateClientValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        var totalErrors = 2;

        errorMessages.Should().Contain(ClientValidatorErrorMessageResource.EMPTY_EMAIL);
        errorMessages.Should().Contain(ClientValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Fact]
    public void Should_Be_Invalid_When_Client_Email_Is_Not_Valid()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequestDto(
            client.Document,
            client.Name,
            "test.com.br");

        var validator = new CreateClientValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors
            .Should()
            .ContainSingle()
            .And
            .Contain(error => error.ErrorMessage == ClientValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS);
    }

}
