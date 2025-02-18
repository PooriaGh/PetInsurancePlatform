using Ardalis.Result;

namespace PetInsurancePlatform.Insurance.Domain.Errors;

public sealed class PetErrors
{
    public static readonly ValidationError EmptyOwner = new("The owner of pet is required.");

    public static readonly ValidationError EmptyTermsOfService = new("The terms of service is required.");

    public static readonly ValidationError DuplicateTermsOfService = new("The terms of service is already accepted.");

    public static readonly ValidationError EmptyBirthCertificatesPages = new("The images of birth certificates pages are required.");

    public static readonly ValidationError EmptyFrontView = new("The image of front view is required.");

    public static readonly ValidationError EmptyRearView = new("The image of rear view is required.");

    public static readonly ValidationError EmptyRightSideView = new("The image of right side view is required.");

    public static readonly ValidationError EmptyLeftSideOfView = new("The image of left side view is required.");

    public static readonly ValidationError EmptyWalkingVideo = new("The walking video is required.");

    public static readonly ValidationError EmptyHealthCertificate = new("The health certificate of pet is required.");

    public static string OutOfRangeAge(int age)
    {
        return $"The pet's age with value = {age} isn't in the acceptable range of the insurance company.";
    }

    public static string NotCoveredDisease(string name)
    {
        return $"The diease with name = {name} is not covered by the insurance company.";
    }
}
