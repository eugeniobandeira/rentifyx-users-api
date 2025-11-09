using Amazon.DynamoDBv2.DataModel;
using System.Globalization;

namespace Rentifyx.Users.Domain.ValueObjects;

public sealed class ProfileImage
{
    [DynamoDBProperty] public string Year { get; private set; } = string.Empty;
    [DynamoDBProperty] public string Month { get; private set; } = string.Empty;
    [DynamoDBProperty] public string Day { get; private set; } = string.Empty;
    [DynamoDBProperty] public string Key { get; private set; } = string.Empty;
    [DynamoDBProperty] public string BucketPath { get; private set; } = string.Empty;
    [DynamoDBProperty] public DateTime UploadedAt { get; private set; } = DateTime.UtcNow;

    public string FullPath => $"year={Year}/month={Month}/day={Day}/key={Key}";

    private ProfileImage() { }

    public static ProfileImage Create(string fileName, string bucketName)
    {
        var now = DateTime.UtcNow;
        var extension = Path.GetExtension(fileName);
        var generatedKey = $"{Guid.NewGuid()}{extension}";

        var path = $"year={now.Year}/month={now.Month:D2}/day={now.Day:D2}/key={generatedKey}";
        var s3Path = $"s3://{bucketName}/{path}";

        return new ProfileImage
        {
            Year = now.Year.ToString(CultureInfo.InvariantCulture),
            Month = now.Month.ToString("D2", CultureInfo.InvariantCulture),
            Day = now.Day.ToString("D2", CultureInfo.InvariantCulture),
            Key = generatedKey,
            BucketPath = s3Path,
            UploadedAt = now
        };
    }
}
