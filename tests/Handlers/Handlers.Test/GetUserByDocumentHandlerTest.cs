using CommomTestUtilities.Repository;
using CommomTestUtilities.Request;
using ErrorOr;
using FluentAssertions;
using Moq;
using Rentifyx.Users.Application.Features.Users.Handler.GetByDocument;
using Rentifyx.Users.Domain.Entities;
using Rentifyx.Users.Domain.Interfaces.User;

namespace Handlers.Test;

public class GetUserByDocumentHandlerTest
{
    [Fact]
    public async Task Should_Return_User_When_Document_Has_11_Characters_And_User_Exists()
    {
        // Arrange
        var document = "12345678901"; // 11 characters (CPF)
        var expectedUser = UserBuilder.Build();
        expectedUser.Document = document;

        var mockRepository = new Mock<IReadOnlyUserRepository>();
        mockRepository.Setup(repo => repo.GetByDocumentAsync(document, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var handler = new GetUserByDocumentHandler(mockRepository.Object);

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Document.Should().Be(document);
        mockRepository.Verify(repo => repo.GetByDocumentAsync(document, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_User_When_Document_Has_14_Characters_And_User_Exists()
    {
        // Arrange
        var document = "12345678901234"; // 14 characters (CNPJ)
        var expectedUser = UserBuilder.Build();
        expectedUser.Document = document;

        var mockRepository = new Mock<IReadOnlyUserRepository>();
        mockRepository.Setup(repo => repo.GetByDocumentAsync(document, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var handler = new GetUserByDocumentHandler(mockRepository.Object);

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Document.Should().Be(document);
        mockRepository.Verify(repo => repo.GetByDocumentAsync(document, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Error_When_Document_Is_Null()
    {
        // Arrange
        string document = null!;
        var handler = new GetUserByDocumentHandler(UserReadOnlyRepositoryBuilder.Build());

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Code.Should().Be("User.Document.Empty");
        result.FirstError.Description.Should().Be("The document cannot be empty.");
    }

    [Fact]
    public async Task Should_Return_Error_When_Document_Is_Empty()
    {
        // Arrange
        var document = string.Empty;
        var handler = new GetUserByDocumentHandler(UserReadOnlyRepositoryBuilder.Build());

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Code.Should().Be("User.Document.Empty");
        result.FirstError.Description.Should().Be("The document cannot be empty.");
    }

    [Fact]
    public async Task Should_Return_Error_When_Document_Is_WhiteSpace()
    {
        // Arrange
        var document = "   ";
        var handler = new GetUserByDocumentHandler(UserReadOnlyRepositoryBuilder.Build());

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Code.Should().Be("User.Document.Empty");
        result.FirstError.Description.Should().Be("The document cannot be empty.");
    }

    [Fact]
    public async Task Should_Return_Error_When_Document_Has_10_Characters()
    {
        // Arrange
        var document = "1234567890"; // 10 characters
        var handler = new GetUserByDocumentHandler(UserReadOnlyRepositoryBuilder.Build());

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Code.Should().Be("User.Document.InvalidFormat");
        result.FirstError.Description.Should().Be("The document must have 11 or 14 characters.");
    }

    [Fact]
    public async Task Should_Return_Error_When_Document_Has_12_Characters()
    {
        // Arrange
        var document = "123456789012"; // 12 characters
        var handler = new GetUserByDocumentHandler(UserReadOnlyRepositoryBuilder.Build());

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Code.Should().Be("User.Document.InvalidFormat");
        result.FirstError.Description.Should().Be("The document must have 11 or 14 characters.");
    }

    [Fact]
    public async Task Should_Return_Error_When_Document_Has_13_Characters()
    {
        // Arrange
        var document = "1234567890123"; // 13 characters
        var handler = new GetUserByDocumentHandler(UserReadOnlyRepositoryBuilder.Build());

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Code.Should().Be("User.Document.InvalidFormat");
        result.FirstError.Description.Should().Be("The document must have 11 or 14 characters.");
    }

    [Fact]
    public async Task Should_Return_Error_When_Document_Has_15_Characters()
    {
        // Arrange
        var document = "123456789012345"; // 15 characters
        var handler = new GetUserByDocumentHandler(UserReadOnlyRepositoryBuilder.Build());

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Code.Should().Be("User.Document.InvalidFormat");
        result.FirstError.Description.Should().Be("The document must have 11 or 14 characters.");
    }

    [Fact]
    public async Task Should_Return_Error_When_Document_Has_Only_1_Character()
    {
        // Arrange
        var document = "1"; // 1 character
        var handler = new GetUserByDocumentHandler(UserReadOnlyRepositoryBuilder.Build());

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Code.Should().Be("User.Document.InvalidFormat");
        result.FirstError.Description.Should().Be("The document must have 11 or 14 characters.");
    }

    [Fact]
    public async Task Should_Return_NotFound_Error_When_User_Does_Not_Exist()
    {
        // Arrange
        var document = "12345678901"; // Valid 11 character document
        var notFoundError = Error.NotFound("User.NotFound", "User not found.");

        var mockRepository = new Mock<IReadOnlyUserRepository>();
        mockRepository.Setup(repo => repo.GetByDocumentAsync(document, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notFoundError);

        var handler = new GetUserByDocumentHandler(mockRepository.Object);

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("User.NotFound");
        result.FirstError.Description.Should().Be("User not found.");
        mockRepository.Verify(repo => repo.GetByDocumentAsync(document, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_User_When_Document_Is_Valid_CPF()
    {
        // Arrange
        var document = "12345678901"; // Valid CPF format (11 digits)
        var expectedUser = UserBuilder.Build();
        expectedUser.Document = document;

        var mockRepository = new Mock<IReadOnlyUserRepository>();
        mockRepository.Setup(repo => repo.GetByDocumentAsync(document, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var handler = new GetUserByDocumentHandler(mockRepository.Object);

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().Be(expectedUser);
    }

    [Fact]
    public async Task Should_Return_User_When_Document_Is_Valid_CNPJ()
    {
        // Arrange
        var document = "12345678000190"; // Valid CNPJ format (14 digits)
        var expectedUser = UserBuilder.Build();
        expectedUser.Document = document;

        var mockRepository = new Mock<IReadOnlyUserRepository>();
        mockRepository.Setup(repo => repo.GetByDocumentAsync(document, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var handler = new GetUserByDocumentHandler(mockRepository.Object);

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().Be(expectedUser);
    }

    [Fact]
    public async Task Should_Not_Call_Repository_When_Document_Is_Invalid()
    {
        // Arrange
        var document = "123"; // Invalid length
        var mockRepository = new Mock<IReadOnlyUserRepository>();
        var handler = new GetUserByDocumentHandler(mockRepository.Object);

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        mockRepository.Verify(repo => repo.GetByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Should_Not_Call_Repository_When_Document_Is_Empty()
    {
        // Arrange
        var document = string.Empty;
        var mockRepository = new Mock<IReadOnlyUserRepository>();
        var handler = new GetUserByDocumentHandler(mockRepository.Object);

        // Act
        var result = await handler.GetUserByDocumentAsync(document, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        mockRepository.Verify(repo => repo.GetByDocumentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
