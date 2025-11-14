using Amazon.DynamoDBv2.DataModel;
using System.Globalization;

namespace Rentifyx.Users.Domain.ValueObjects;

public sealed class ProfileImage
{
    [DynamoDBProperty] public string Year { get; set; } = string.Empty;
    [DynamoDBProperty] public string Month { get; set; } = string.Empty;
    [DynamoDBProperty] public string Day { get; set; } = string.Empty;
    [DynamoDBProperty] public string Key { get; set; } = string.Empty;
    [DynamoDBProperty] public string BucketPath { get; set; } = string.Empty;
    [DynamoDBProperty] public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public string FullPath => $"year={Year}/month={Month}/day={Day}/key={Key}";

    public ProfileImage() { }

    public static ProfileImage Create(string fileName, string bucketName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(bucketName);

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
