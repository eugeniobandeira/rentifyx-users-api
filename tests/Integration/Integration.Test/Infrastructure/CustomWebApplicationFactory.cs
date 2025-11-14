using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rentifyx.Users.Domain.Constants;

namespace Integration.Test.Infrastructure;

public class CustomWebApplicationFactory(string dynamoDbServiceUrl) 
    : WebApplicationFactory<Program>
{
    private readonly string _dynamoDbServiceUrl = dynamoDbServiceUrl;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IAmazonDynamoDB>();
            services.RemoveAll<IDynamoDBContext>();

            // Use fake credentials for DynamoDB local (Testcontainers)
            // These credentials are never validated by DynamoDB local
            var fakeCredentials = new BasicAWSCredentials("FAKE_ACCESS_KEY", "FAKE_SECRET_KEY");

            var dynamoDbConfig = new AmazonDynamoDBConfig
            {
                ServiceURL = _dynamoDbServiceUrl,
                // Disable authentication signature for local DynamoDB
                AuthenticationRegion = "us-east-1"
            };

            var dynamoDbClient = new AmazonDynamoDBClient(fakeCredentials, dynamoDbConfig);

            services.AddSingleton<IAmazonDynamoDB>(dynamoDbClient);
            services.AddScoped<IDynamoDBContext>(sp =>
                new DynamoDBContext(sp.GetRequiredService<IAmazonDynamoDB>()));
        });

        builder.UseEnvironment("Development");
    }

    public async Task InitializeDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var dynamoDbClient = scope.ServiceProvider.GetRequiredService<IAmazonDynamoDB>();

        try
        {
            var tables = await dynamoDbClient.ListTablesAsync();
            if (!tables.TableNames.Contains(AwsContants.RENTIFYX_TABLE_NAME))
            {
                var createTableRequest = new CreateTableRequest
                {
                    TableName = AwsContants.RENTIFYX_TABLE_NAME,
                    AttributeDefinitions =
                    [
                        new AttributeDefinition
                        {
                            AttributeName = "Document",
                            AttributeType = ScalarAttributeType.S
                        }
                    ],
                    KeySchema =
                    [
                        new KeySchemaElement
                        {
                            AttributeName = "Document",
                            KeyType = KeyType.HASH
                        }
                    ],
                    BillingMode = BillingMode.PAY_PER_REQUEST
                };

                await dynamoDbClient.CreateTableAsync(createTableRequest);

                var tableStatus = string.Empty;
                var attempts = 0;
                const int maxAttempts = 60;

                do
                {
                    if (attempts >= maxAttempts)
                    {
                        throw new TimeoutException($"Table creation timed out after {maxAttempts * 500}ms");
                    }

                    await Task.Delay(500);
                    var describeTableResponse = await dynamoDbClient.DescribeTableAsync(AwsContants.RENTIFYX_TABLE_NAME);
                    tableStatus = describeTableResponse.Table.TableStatus;
                    attempts++;
                } while (tableStatus != "ACTIVE");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to initialize DynamoDB table for testing", ex);
        }
    }

    public async Task CleanupDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var dynamoDbClient = scope.ServiceProvider.GetRequiredService<IAmazonDynamoDB>();

        try
        {
            Dictionary<string, AttributeValue>? lastEvaluatedKey = null;

            do
            {
                var scanRequest = new ScanRequest
                {
                    TableName = AwsContants.RENTIFYX_TABLE_NAME,
                    ExclusiveStartKey = lastEvaluatedKey
                };

                var scanResponse = await dynamoDbClient.ScanAsync(scanRequest);

                foreach (var item in scanResponse.Items)
                {
                    var deleteRequest = new DeleteItemRequest
                    {
                        TableName = AwsContants.RENTIFYX_TABLE_NAME,
                        Key = new Dictionary<string, AttributeValue>
                        {
                            { "Document", item["Document"] }
                        }
                    };

                    await dynamoDbClient.DeleteItemAsync(deleteRequest);
                }

                lastEvaluatedKey = scanResponse.LastEvaluatedKey;
            } while (lastEvaluatedKey is { Count: > 0 });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to cleanup DynamoDB table after testing", ex);
        }
    }
}
