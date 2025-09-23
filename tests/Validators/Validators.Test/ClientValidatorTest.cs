using CommomTestUtilities.Request;
using FluentAssertions;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create;
using Rentifyx.Clients.Application.Features.Clients.Request;
using Rentifyx.Clients.Domain.MessageResource;

namespace Validators.Test;

public class ClientValidatorTest
{
    [Fact]
    public void ShouldBeValidWhenClientIsValid()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequest(
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
    public void ShouldBeInvalidWhenClientDocumentIsNull(string document)
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequest(
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

        errorMessages.Should().Contain(ClientValidatorMessageResource.EMPTY_DOCUMENT);
        errorMessages.Should().Contain(ClientValidatorMessageResource.DOCUMENT_LENGTH);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Theory]
    [InlineData("1234567891")]
    [InlineData("12345678912345678")]
    [InlineData("123")]
    public void ShouldBeInvalidWhenClientDocumentHasInvalidLength(string document)
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequest(
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
            .Contain(error => error.ErrorMessage == ClientValidatorMessageResource.DOCUMENT_LENGTH);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void ShouldBeInvalidWhenClientNameIsNull(string name)
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequest(
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

        errorMessages.Should().Contain(ClientValidatorMessageResource.EMPTY_NAME);
        errorMessages.Should().Contain(ClientValidatorMessageResource.NAME_MIN_LENGTH);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Fact]
    public void ShouldBeInvalidWhenClientNameIsMoreThan100Digits()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequest(
            client.Document,
            "fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name",
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
            .Contain(error => error.ErrorMessage == ClientValidatorMessageResource.NAME_MAX_LENGTH);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void ShouldBeInvalidWhenClientEmailIsNull(string email)
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequest(
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

        errorMessages.Should().Contain(ClientValidatorMessageResource.EMPTY_EMAIL);
        errorMessages.Should().Contain(ClientValidatorMessageResource.INVALID_EMAIL_ADDRESS);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Fact]
    public void ShouldBeInvalidWhenClientEmailIsNotValid()
    {
        // Arrange
        var client = ClientBuilder.Build();

        var request = new CreateClientRequest(
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
            .Contain(error => error.ErrorMessage == ClientValidatorMessageResource.INVALID_EMAIL_ADDRESS);
    }

}
