using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
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
            // Remove the existing DynamoDB registrations
            services.RemoveAll<IAmazonDynamoDB>();
            services.RemoveAll<IDynamoDBContext>();

            // Register test DynamoDB client pointing to Testcontainer
            var dynamoDbConfig = new AmazonDynamoDBConfig
            {
                ServiceURL = _dynamoDbServiceUrl
            };

            var dynamoDbClient = new AmazonDynamoDBClient(dynamoDbConfig);

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
            // Check if table exists
            var tables = await dynamoDbClient.ListTablesAsync();
            if (!tables.TableNames.Contains(AwsContants.RENTIFYX_TABLE_NAME))
            {
                // Create the table
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

                // Wait for table to be active
                var tableStatus = string.Empty;
                do
                {
                    await Task.Delay(500);
                    var describeTableResponse = await dynamoDbClient.DescribeTableAsync(AwsContants.RENTIFYX_TABLE_NAME);
                    tableStatus = describeTableResponse.Table.TableStatus;
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
            // Scan all items
            var scanRequest = new ScanRequest
            {
                TableName = AwsContants.RENTIFYX_TABLE_NAME
            };

            var scanResponse = await dynamoDbClient.ScanAsync(scanRequest);

            // Delete all items
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
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to cleanup DynamoDB table after testing", ex);
        }
    }
}
