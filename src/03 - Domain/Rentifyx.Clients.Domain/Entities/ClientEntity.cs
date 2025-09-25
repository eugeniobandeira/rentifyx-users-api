using Amazon.DynamoDBv2.DataModel;

namespace Rentifyx.Clients.Domain.Entities;

[DynamoDBTable("rentifyx-clients")]
public sealed class ClientEntity
{
    [DynamoDBHashKey]
    public string Document { get; set; } = string.Empty;

    [DynamoDBProperty("Name")]
    public string Name { get; set; } = string.Empty;

    [DynamoDBProperty("Email")]
    public string Email { get; set; } = string.Empty;
}
