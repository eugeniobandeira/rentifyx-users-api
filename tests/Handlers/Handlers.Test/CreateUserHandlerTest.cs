using CommomTestUtilities.Logger;
using CommomTestUtilities.Repository;
using CommomTestUtilities.Request;
using CommomTestUtilities.Validator;
using FluentAssertions;
using Rentifyx.Users.Application.Features.Users.Handler.Create;
using Rentifyx.Users.Domain.ValueObjects;
using Rentifyx.Users.Exceptions.MessageResource;

namespace Handlers.Test;

public class CreateUserHandlerTest
{
    private static CreateUserHandler CreateHandler()
    {
        var addOnlyRepository = UserAddOnlyRepositoryBuilder.Build();
        var validator = CreateUserValidatorBuilder.Build();
        var logger = LoggerBuilder<CreateUserHandler>.Build();

        return new CreateUserHandler(validator, addOnlyRepository, logger);
    }

    [Fact]
    public async Task Should_Create_User_When_Request_Is_Valid()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder().Build();
        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Document.Should().Be(request.Document);
        result.Value.Name.Should().Be(request.Name);
        result.Value.Email.Should().Be(request.Email);
        result.Value.Address.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Document_Is_Invalid()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithDocument("123456")
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors
            .Should()
            .ContainSingle(error =>
                error.Description == UserValidatorErrorMessageResource.DOCUMENT_LENGTH);
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Document_Is_WhiteSpace()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithDocument("   ")
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        var totalErros = 2;

        result.IsError.Should().BeTrue();

        result.Errors
            .Should()
            .Contain(error =>
                error.Description == UserValidatorErrorMessageResource.EMPTY_DOCUMENT)
            .And
            .Contain(error =>
                error.Description == UserValidatorErrorMessageResource.DOCUMENT_LENGTH);

        result.Errors.Count.Should().Be(totalErros);
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Name_Is_WhiteSpace()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithName("")
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        var totalErros = 2;

        result.IsError.Should().BeTrue();

        result.Errors
            .Should()
            .Contain(error =>
                error.Description == UserValidatorErrorMessageResource.EMPTY_NAME)
            .And
            .Contain(error =>
                error.Description == UserValidatorErrorMessageResource.NAME_MIN_LENGTH);

        result.Errors.Count.Should().Be(totalErros);
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Name_Min_Length_Is_Invalid()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithName("May")
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors
            .Should()
            .ContainSingle(error =>
                error.Description == UserValidatorErrorMessageResource.NAME_MIN_LENGTH);
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Name_Exceeds_100_Characters()
    {
        // Arrange
        string invalidLengthName = "fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name fake name";

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithName(invalidLengthName)
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors
            .Should()
            .ContainSingle(error =>
                error.Description == UserValidatorErrorMessageResource.NAME_MAX_LENGTH);
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Email_Is_WhiteSpace()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithEmail("   ")
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        var totalErros = 2;

        result.IsError.Should().BeTrue();

        result.Errors
            .Should()
            .Contain(error =>
                error.Description == UserValidatorErrorMessageResource.EMPTY_EMAIL)
            .And
            .Contain(error =>
                error.Description == UserValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS);

        result.Errors.Count.Should().Be(totalErros);
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Email_Is_Invalid()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithEmail("test.com.br")
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors
            .Should()
            .ContainSingle(error =>
                error.Description == UserValidatorErrorMessageResource.INVALID_EMAIL_ADDRESS);
    }

    [Fact]
    public async Task Should_Create_User_With_Address_When_Address_Is_Valid()
    {
        // Arrange
        var request = CreateUserRequestDtoBuilder.Builder().Build();
        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.Document.Should().Be(request.Document);
        result.Value.Name.Should().Be(request.Name);
        result.Value.Email.Should().Be(request.Email);
        result.Value.Address.Should().NotBeNull();
        result.Value.Address.Street.Should().Be(request.AddressRequestDto.Street);
        result.Value.Address.City.Should().Be(request.AddressRequestDto.City);
    }

    [Fact]
    public async Task Should_Create_User_With_ProfileImage_When_FileName_Is_Provided()
    {
        // Arrange
        var profileImageFileName = "profile.jpg";

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithProfileImageFileName(profileImageFileName)
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.ProfileImage.Should().NotBeNull();
        result.Value.ProfileImage!.Key.Should().EndWith(".jpg");
        result.Value.ProfileImage.BucketPath.Should().Contain("rentifyx-profile-images");
    }

    [Fact]
    public async Task Should_Create_User_With_Address_And_ProfileImage()
    {
        // Arrange
        var profileImageFileName = "avatar.png";

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithProfileImageFileName(profileImageFileName)
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.Address.Should().NotBeNull();
        result.Value.ProfileImage.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Address_Street_Is_Empty()
    {
        // Arrange & Act
        var act = () => Address.Builder()
            .WithStreet("")
            .WithNumber("123")
            .WithNeighborhood("Centro")
            .WithCity("São Paulo")
            .WithState("SP")
            .WithZipCode("01310100")
            .WithComplement(null)
            .Build();

        // Assert
        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage(UserValidatorErrorMessageResource.EMPTY_STREET);
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Address_City_Is_Empty()
    {
        // Arrange & Act
        var act = () => Address.Builder()
            .WithStreet("Avenida Paulista")
            .WithNumber("123")
            .WithNeighborhood("Bela Vista")
            .WithCity("")
            .WithState("SP")
            .WithZipCode("01310100")
            .WithComplement(null)
            .Build();

        // Assert
        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage(UserValidatorErrorMessageResource.EMPTY_CITY);
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Address_State_Is_Invalid()
    {
        // Arrange
        var invalidAddress = AddressRequestDtoBuilder.Builder()
            .WithStreet("Avenida Paulista")
            .WithNumber("123")
            .WithNeighborhood("Bela Vista")
            .WithCity("São Paulo")
            .WithState("São Paulo")
            .WithZipCode("01310100")
            .WithComplement(null)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(invalidAddress)
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors
            .Should()
            .ContainSingle(error => error.Description == UserValidatorErrorMessageResource.STATE_LENGTH);
    }

    [Fact]
    public async Task Should_Not_Create_User_When_Address_ZipCode_Is_Invalid()
    {
        // Arrange
        var invalidAddress = AddressRequestDtoBuilder.Builder()
            .WithStreet("Avenida Paulista")
            .WithNumber("123")
            .WithNeighborhood("Bela Vista")
            .WithCity("São Paulo")
            .WithState("SP")
            .WithZipCode("0131010")
            .WithComplement(null)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(invalidAddress)
            .Build();

        var handler = CreateHandler();

        // Act
        var result = await handler.CreateUsersAsync(request, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors
            .Should()
            .ContainSingle(error => error.Description == UserValidatorErrorMessageResource.ZIPCODE_LENGTH);
    }

}
