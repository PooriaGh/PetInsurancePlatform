using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using PetInsurancePlatform.SharedKernel.Abstractions;
using PetInsurancePlatform.SharedKernel.Extensions;

namespace PetInsurancePlatform.Insurance.Domain.ValueObjects;

public sealed class StoredFile : ValueObject
{
    public static readonly StoredFile None = new();

    private const int _maximumLimit = 10 * 1024 * 1024; // 10 MB

    private static readonly string[] _supportedContenTypes = [".png", ".jpeg", ".svg"];

    // Used by EF Core
    private StoredFile()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public string Key { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    public string BucketName { get; private set; } = string.Empty;

    public long SizeInBytes { get; private set; }

    public DateTime UploadedAt { get; private set; }

    public static Result<StoredFile> Upload(
        IFormFile file,
        string bucketName,
        string directory = "")
    {
        if (file.Length == 0)
        {
            return Result.Invalid(new ValidationError("The file size must be greater thatn zero."));
        }

        if (file.Length > _maximumLimit)
        {
            return Result.Invalid(new ValidationError("the file size exceeds the maximum limit."));
        }

        if (!_supportedContenTypes.Contains(file.ContentType))
        {
            return Result.Invalid(new ValidationError($"The file content type {file.ContentType} is not supported."));
        }

        var key = directory + file.FileName + DateTime.UtcNow.ToUnixTimeMilliSeconds();

        return new StoredFile
        {
            Name = file.FileName,
            Key = key,
            ContentType = file.ContentType,
            BucketName = bucketName,
            SizeInBytes = file.Length,
            UploadedAt = DateTime.UtcNow,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Key;
    }
}
