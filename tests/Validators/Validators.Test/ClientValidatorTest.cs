using CommomTestUtilities.Request;
using Rentifyx.Clients.Application.Features.Clients.Handler.Create;
using Rentifyx.Clients.Application.Features.Clients.Request;

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
}
