using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommomTestUtilities.Request;
using FluentAssertions;
using Integration.Test.Infrastructure;
using Rentifyx.Users.Domain.Entities;

namespace Integration.Test.Endpoints;

/// <summary>
/// Testes de integração para a API de usuários.
/// Usa collection compartilhada para otimizar performance no CI/CD.
/// </summary>
[Collection("Integration Tests")]
public class UsersApiIntegrationTests : IntegrationTestBase
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UsersApiIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateUser_WithValidData_ShouldReturnCreatedAndPersistInDatabase()
    {
        var request = CreateUserRequestDtoBuilder.Builder().Build();

        var createResponse = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        createResponse.Headers.Location.Should().NotBeNull();
        createResponse.Headers.Location!.ToString().Should().Contain($"/api/v1/users/{request.Document}");

        var getResponse = await HttpClient.GetAsync($"/api/v1/users/{request.Document}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await getResponse.Content.ReadFromJsonAsync<UserEntity>(_jsonOptions);
        user.Should().NotBeNull();
        user!.Document.Should().Be(request.Document);
        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
        user.Address.Street.Should().Be(request.AddressRequestDto.Street);
        user.Address.City.Should().Be(request.AddressRequestDto.City);
    }

    [Fact]
    public async Task CreateUser_WithProfileImage_ShouldPersistProfileImage()
    {
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithProfileImageFileName("profile.jpg")
            .Build();

        var createResponse = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var getResponse = await HttpClient.GetAsync($"/api/v1/users/{request.Document}");
        var user = await getResponse.Content.ReadFromJsonAsync<UserEntity>(_jsonOptions);

        user!.ProfileImage.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateUser_WithCPF_ShouldAccept11Characters()
    {
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithDocument("12345678901")
            .Build();

        var response = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateUser_WithCNPJ_ShouldAccept14Characters()
    {
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithDocument("12345678901234")
            .Build();

        var response = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateUser_WithEmptyDocument_ShouldReturnValidationError()
    {
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithDocument(string.Empty)
            .Build();

        var response = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateUser_WithInvalidDocumentLength_ShouldReturnValidationError()
    {
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithDocument("123")
            .Build();

        var response = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateUser_WithEmptyName_ShouldReturnValidationError()
    {
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithName(string.Empty)
            .Build();

        var response = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateUser_WithInvalidEmail_ShouldReturnValidationError()
    {
        var request = CreateUserRequestDtoBuilder.Builder()
            .WithEmail("invalid-email")
            .Build();

        var response = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateUser_WithEmptyStreet_ShouldReturnValidationError()
    {
        var address = AddressRequestDtoBuilder.Builder()
            .WithStreet(string.Empty)
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var response = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateUser_WithInvalidZipCode_ShouldReturnValidationError()
    {
        var address = AddressRequestDtoBuilder.Builder()
            .WithZipCode("123")
            .Build();

        var request = CreateUserRequestDtoBuilder.Builder()
            .WithAddress(address)
            .Build();

        var response = await HttpClient!.PostAsJsonAsync("/api/v1/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task GetUserByDocument_WithExistingUser_ShouldReturnUser()
    {
        var createRequest = CreateUserRequestDtoBuilder.Builder().Build();
        var createResponse = await HttpClient!.PostAsJsonAsync("/api/v1/users", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var getResponse = await HttpClient.GetAsync($"/api/v1/users/{createRequest.Document}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var user = await getResponse.Content.ReadFromJsonAsync<UserEntity>(_jsonOptions);
        user.Should().NotBeNull();
        user!.Document.Should().Be(createRequest.Document);
        user.Name.Should().Be(createRequest.Name);
        user.Email.Should().Be(createRequest.Email);
    }

    [Fact]
    public async Task GetUserByDocument_WithNonExistingDocument_ShouldReturnNotFound()
    {
        var response = await HttpClient!.GetAsync("/api/v1/users/99999999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserByDocument_WithInvalidDocumentLength_ShouldReturnValidationError()
    {
        var response = await HttpClient!.GetAsync("/api/v1/users/123");

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task CreateMultipleUsers_ShouldPersistAllIndependently()
    {
        var user1 = CreateUserRequestDtoBuilder.Builder().Build();
        var user2 = CreateUserRequestDtoBuilder.Builder().Build();
        var user3 = CreateUserRequestDtoBuilder.Builder().Build();

        var response1 = await HttpClient!.PostAsJsonAsync("/api/v1/users", user1);
        var response2 = await HttpClient.PostAsJsonAsync("/api/v1/users", user2);
        var response3 = await HttpClient.PostAsJsonAsync("/api/v1/users", user3);

        response1.StatusCode.Should().Be(HttpStatusCode.Created);
        response2.StatusCode.Should().Be(HttpStatusCode.Created);
        response3.StatusCode.Should().Be(HttpStatusCode.Created);

        var getResponse1 = await HttpClient.GetAsync($"/api/v1/users/{user1.Document}");
        var getResponse2 = await HttpClient.GetAsync($"/api/v1/users/{user2.Document}");
        var getResponse3 = await HttpClient.GetAsync($"/api/v1/users/{user3.Document}");

        getResponse1.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse2.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse3.StatusCode.Should().Be(HttpStatusCode.OK);

        var retrievedUser1 = await getResponse1.Content.ReadFromJsonAsync<UserEntity>(_jsonOptions);
        var retrievedUser2 = await getResponse2.Content.ReadFromJsonAsync<UserEntity>(_jsonOptions);
        var retrievedUser3 = await getResponse3.Content.ReadFromJsonAsync<UserEntity>(_jsonOptions);

        retrievedUser1!.Document.Should().Be(user1.Document);
        retrievedUser2!.Document.Should().Be(user2.Document);
        retrievedUser3!.Document.Should().Be(user3.Document);
    }
}
