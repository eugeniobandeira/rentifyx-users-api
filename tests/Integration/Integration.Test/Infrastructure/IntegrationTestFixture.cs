using Testcontainers.DynamoDb;

namespace Integration.Test.Infrastructure;

/// <summary>
/// Collection Fixture compartilhado entre todos os testes de integração.
/// O container DynamoDB é criado UMA VEZ e reutilizado por todos os testes.
/// Isso reduz drasticamente o tempo de execução no CI/CD.
/// </summary>
public class IntegrationTestFixture : IAsyncLifetime
{
    private DynamoDbContainer? _dynamoDbContainer;
    private CustomWebApplicationFactory? _factory;

    public CustomWebApplicationFactory Factory
    {
        get
        {
            if (_factory == null)
            {
                throw new InvalidOperationException("Factory not initialized. Call InitializeAsync first.");
            }
            return _factory;
        }
    }

    public string DynamoDbConnectionString
    {
        get
        {
            if (_dynamoDbContainer == null)
            {
                throw new InvalidOperationException("DynamoDB container not initialized. Call InitializeAsync first.");
            }
            return _dynamoDbContainer.GetConnectionString();
        }
    }

    public async Task InitializeAsync()
    {
        try
        {
            Console.WriteLine("🐳 [Fixture] Starting DynamoDB container (shared for all tests)...");

            _dynamoDbContainer = new DynamoDbBuilder()
                .WithImage("amazon/dynamodb-local:latest")
                .WithCleanUp(true)
                .Build();

            await _dynamoDbContainer.StartAsync();

            Console.WriteLine($"✅ [Fixture] DynamoDB container started: {_dynamoDbContainer.GetConnectionString()}");

            _factory = new CustomWebApplicationFactory(_dynamoDbContainer.GetConnectionString());
            await _factory.InitializeDatabaseAsync();

            Console.WriteLine("✅ [Fixture] Database initialized and ready for tests");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ [Fixture] Failed to initialize test infrastructure: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task DisposeAsync()
    {
        try
        {
            Console.WriteLine("🧹 [Fixture] Cleaning up test infrastructure...");

            if (_factory != null)
            {
                await _factory.DisposeAsync();
                Console.WriteLine("✅ [Fixture] Factory disposed");
            }

            if (_dynamoDbContainer != null)
            {
                await _dynamoDbContainer.DisposeAsync();
                Console.WriteLine("✅ [Fixture] DynamoDB container stopped");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ [Fixture] Error during cleanup: {ex.Message}");
        }
    }

    /// <summary>
    /// Limpa dados do banco entre testes para garantir isolamento
    /// </summary>
    public async Task CleanupDatabaseAsync()
    {
        if (_factory != null)
        {
            await _factory.CleanupDatabaseAsync();
        }
    }
}
