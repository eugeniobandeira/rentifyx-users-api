namespace Integration.Test.Infrastructure;

/// <summary>
/// Base class for integration tests.
/// Uses shared fixture to reuse DynamoDB container.
/// Each test receives a clean HttpClient and database is cleaned after each test.
/// </summary>
public abstract class IntegrationTestBase : IAsyncLifetime
{
    private readonly IntegrationTestFixture _fixture;
    protected HttpClient? HttpClient;

    protected IntegrationTestBase(IntegrationTestFixture fixture)
    {
        _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
    }

    /// <summary>
    /// Executed before each individual test.
    /// Creates a new HttpClient for isolation.
    /// </summary>
    public Task InitializeAsync()
    {
        try
        {
            HttpClient = _fixture.Factory.CreateClient();
            Console.WriteLine($"🧪 [Test] HttpClient created for test");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ [Test] Failed to initialize test: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Executed after each individual test.
    /// Cleans database data to ensure isolation between tests.
    /// Container is NOT destroyed (it is reused).
    /// </summary>
    public async Task DisposeAsync()
    {
        try
        {
            HttpClient?.Dispose();

            await _fixture.CleanupDatabaseAsync();
            Console.WriteLine("🧹 [Test] Database cleaned after test");
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine($"⚠️ [Test] Object disposed during test cleanup: {ex.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"⚠️ [Test] Invalid operation during test cleanup: {ex.Message}");
            throw;
        }
    }
}
