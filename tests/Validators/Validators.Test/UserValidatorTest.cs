using CommomTestUtilities.Request;
using FluentAssertions;
using Rentifyx.Users.Application.Features.Users.Handler.Create.Validator;
using Rentifyx.Users.Exceptions.MessageResource;

namespace Validators.Test;

public class UserValidatorTest
{
    [Fact]
    public void Should_Be_Valid_When_Client_Is_Valid()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithProfileImageFileName("profile-image")
            .Build();

        var validator = new CreateUserValidator();

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
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithDocument(document)
            .WithProfileImageFileName("profile-image")
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        var totalErrors = 2;

        errorMessages.Should().Contain(UserValidatorErrorMessageResource.EMPTY_DOCUMENT);
        errorMessages.Should().Contain(UserValidatorErrorMessageResource.DOCUMENT_LENGTH);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Theory]
    [InlineData("1234567891")]
    [InlineData("12345678912345678")]
    [InlineData("123")]
    public void Should_Be_Invalid_When_Client_Document_Has_Invalid_Length(string document)
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithDocument(document)
            .WithProfileImageFileName("profile-image")
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors
            .Should()
            .ContainSingle()
            .And
            .Contain(error => error.ErrorMessage == UserValidatorErrorMessageResource.DOCUMENT_LENGTH);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Client_Name_Is_WhiteSpace(string name)
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithName(name)
            .WithProfileImageFileName("profile-image")
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        var totalErrors = 2;

        errorMessages.Should().Contain(UserValidatorErrorMessageResource.EMPTY_NAME);
        errorMessages.Should().Contain(UserValidatorErrorMessageResource.NAME_MIN_LENGTH);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Fact]
    public void Should_Be_Invalid_When_Client_Name_Is_More_Than_100_Characters()
    {
        // Arrange
        string invalidLengthName = "fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name";

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithName(invalidLengthName)
            .WithProfileImageFileName("profile-image")
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors
            .Should()
            .ContainSingle()
            .And
            .Contain(error => error.ErrorMessage == UserValidatorErrorMessageResource.NAME_MAX_LENGTH);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Client_Email_Is_WhiteSpace(string email)
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithEmail(email)
            .WithProfileImageFileName("profile-image")
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        var totalErrors = 2;

        errorMessages.Should().Contain(UserValidatorErrorMessageResource.EMPTY_EMAIL);
        errorMessages.Should().Contain(UserValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Fact]
    public void Should_Be_Invalid_When_Client_Email_Is_Not_Valid()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithEmail("test.com.br")
            .WithProfileImageFileName("profile-image")
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        result.Errors
            .Should()
            .ContainSingle()
            .And
            .Contain(error => error.ErrorMessage == UserValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Address_Street_Is_WhiteSpace(string street)
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithStreet(street)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(error => error.ErrorMessage == UserValidatorErrorMessageResource.EMPTY_STREET);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Address_Number_Is_WhiteSpace(string number)
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithNumber(number)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(error => error.ErrorMessage == UserValidatorErrorMessageResource.EMPTY_NUMBER);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Address_Neighborhood_Is_WhiteSpace(string neighborhood)
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithNeighborhood(neighborhood)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(error => error.ErrorMessage == UserValidatorErrorMessageResource.EMPTY_NEIGHBORHOOD);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Address_City_Is_WhiteSpace(string city)
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithCity(city)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(error => error.ErrorMessage == UserValidatorErrorMessageResource.EMPTY_CITY);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Address_State_Is_WhiteSpace(string state)
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithState(state)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        var totalErrors = 2;

        errorMessages.Should().Contain(UserValidatorErrorMessageResource.EMPTY_STATE);
        errorMessages.Should().Contain(UserValidatorErrorMessageResource.STATE_LENGTH);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Theory]
    [InlineData("S")]
    [InlineData("SPP")]
    [InlineData("São Paulo")]
    public void Should_Be_Invalid_When_Address_State_Length_Is_Not_2(string state)
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithState(state)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(error => error.ErrorMessage == UserValidatorErrorMessageResource.STATE_LENGTH);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    public void Should_Be_Invalid_When_Address_ZipCode_Is_WhiteSpace(string zipCode)
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithZipCode(zipCode)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();

        var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
        var totalErrors = 2;

        errorMessages.Should().Contain(UserValidatorErrorMessageResource.EMPTY_ZIPCODE);
        errorMessages.Should().Contain(UserValidatorErrorMessageResource.ZIPCODE_LENGTH);

        result.Errors.Should().HaveCount(totalErrors);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("123456789")]
    [InlineData("12345")]
    public void Should_Be_Invalid_When_Address_ZipCode_Length_Is_Not_8(string zipCode)
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithZipCode(zipCode)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors
            .Should()
            .ContainSingle(error => error.ErrorMessage == UserValidatorErrorMessageResource.ZIPCODE_LENGTH);
    }

    [Fact]
    public void Should_Be_Valid_When_Address_Is_Complete_And_Valid()
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithStreet("Avenida Paulista")
            .WithNumber("1000")
            .WithNeighborhood("Bela Vista")
            .WithCity("São Paulo")
            .WithState("SP")
            .WithZipCode("01310100")
            .WithComplement("Apto 101")
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_Be_Valid_When_Address_Has_No_Complement()
    {
        // Arrange
        var address = AddressRequestDtoBuilder.Builder()
            .WithStreet("Avenida Paulista")
            .WithNumber("1000")
            .WithNeighborhood("Bela Vista")
            .WithCity("São Paulo")
            .WithState("SP")
            .WithZipCode("01310100")
            .WithComplement(null)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var validator = new CreateUserValidator();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

}
