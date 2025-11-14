namespace Integration.Test.Infrastructure;

/// <summary>
/// Defines a test collection that shares the same fixture.
/// All tests marked with [Collection("Integration Tests")]
/// will share the same DynamoDB container.
///
/// IMPORTANT: Tests in the same collection do NOT run in parallel,
/// ensuring isolation and avoiding race conditions.
/// </summary>
[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollectionFixture : ICollectionFixture<IntegrationTestFixture>
{
    // This class has no code. It is just a marker for xUnit.
    // xUnit uses this marker to create a shared fixture.
}
