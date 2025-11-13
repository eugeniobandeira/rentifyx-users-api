using Testcontainers.DynamoDb;

namespace Integration.Test.Infrastructure;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    private DynamoDbContainer? _dynamoDbContainer;
    private CustomWebApplicationFactory? _factory;
    protected HttpClient? HttpClient;

    public async Task InitializeAsync()
    {
        _dynamoDbContainer = new DynamoDbBuilder()
            .WithImage("amazon/dynamodb-local:latest")
            .Build();

        await _dynamoDbContainer.StartAsync();

        _factory = new CustomWebApplicationFactory(_dynamoDbContainer.GetConnectionString());
        await _factory.InitializeDatabaseAsync();

        HttpClient = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        if (_factory != null)
        {
            await _factory.CleanupDatabaseAsync();
            await _factory.DisposeAsync();
        }

        if (_dynamoDbContainer != null)
        {
            await _dynamoDbContainer.DisposeAsync();
        }

        HttpClient?.Dispose();
    }
}
