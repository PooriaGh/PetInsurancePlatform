namespace PetInsurancePlatform.SharedKernel.Extensions;

public static class LongExtensions
{
    public static int GetLength(this long number)
    {
        return number switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(number)),
            0 => 1,
            _ => (int)Math.Floor(Math.Log10(number)) + 1
        };
    }
}
