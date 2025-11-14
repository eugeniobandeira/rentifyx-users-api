namespace Integration.Test.Infrastructure;

/// <summary>
/// Define uma coleção de testes que compartilham a mesma fixture.
/// Todos os testes marcados com [Collection("Integration Tests")]
/// compartilharão o mesmo container DynamoDB.
///
/// IMPORTANTE: Testes na mesma collection NÃO rodam em paralelo,
/// garantindo isolamento e evitando race conditions.
/// </summary>
[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
    // Esta classe não tem código. É apenas um marcador para o xUnit.
    // O xUnit usa este marcador para criar uma fixture compartilhada.
}
