namespace Integration.Test.Infrastructure;

/// <summary>
/// Classe base para testes de integração.
/// Usa a fixture compartilhada para reutilizar o container DynamoDB.
/// Cada teste recebe um HttpClient limpo e o banco é limpo após cada teste.
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
    /// Executado antes de cada teste individual.
    /// Cria um novo HttpClient para isolamento.
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
    /// Executado após cada teste individual.
    /// Limpa os dados do banco para garantir isolamento entre testes.
    /// O container NÃO é destruído (é reutilizado).
    /// </summary>
    public async Task DisposeAsync()
    {
        try
        {
            HttpClient?.Dispose();

            // Limpa apenas os dados, mantém o container rodando
            await _fixture.CleanupDatabaseAsync();
            Console.WriteLine("🧹 [Test] Database cleaned after test");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ [Test] Error during test cleanup: {ex.Message}");
        }
    }
}
