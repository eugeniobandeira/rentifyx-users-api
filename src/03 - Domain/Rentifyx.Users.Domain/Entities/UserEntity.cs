using Amazon.DynamoDBv2.DataModel;
using Rentifyx.Users.Domain.Constants;

namespace Rentifyx.Users.Domain.Entities;

[DynamoDBTable(AwsContants.RENTIFYX_TABLE_NAME)]
public sealed class UserEntity
{
    [DynamoDBHashKey]
    public string Document { get; set; } = string.Empty;

    [DynamoDBProperty("Name")]
    public string Name { get; set; } = string.Empty;

    [DynamoDBProperty("Email")]
    public string Email { get; set; } = string.Empty;
}
