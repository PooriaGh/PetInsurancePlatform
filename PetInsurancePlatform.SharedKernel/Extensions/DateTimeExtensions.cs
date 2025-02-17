using System.Globalization;

namespace PetInsurancePlatform.SharedKernel.Extensions;

public static class DateTimeExtensions
{
    public static string ToLocalString(
        this DateTime dateTime,
        CultureInfo culture,
        string format = "yyyy/MM/dd")
    {
        return dateTime.ToString(format, culture);
    }

    public static string ToUnixTimeMilliSeconds(this DateTime dateTime)
    {
        var dateTimeOffset = new DateTimeOffset(dateTime.ToUniversalTime());

        return dateTimeOffset
            .ToUnixTimeMilliseconds()
            .ToString();
    }
}
