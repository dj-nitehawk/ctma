using System.Text.RegularExpressions;

namespace Ctma;

public static partial class Extensions
{
    static readonly Regex _nicRx1 = NicRx1();
    static readonly Regex _nicRx2 = NicRx2();

    [GeneratedRegex(@"^\d{9}[x|X|v|V]$")]
    private static partial Regex NicRx1();

    [GeneratedRegex(@"^\d{12}$")]
    private static partial Regex NicRx2();

    public static bool IsAValidNic(this string nicNumber)
        => !string.IsNullOrEmpty(nicNumber) && (_nicRx1.IsMatch(nicNumber) || _nicRx2.IsMatch(nicNumber));

    static readonly Regex _slmcRx = SlmcRx();

    [GeneratedRegex(@"^\d{1,5}$")]
    private static partial Regex SlmcRx();

    public static bool IsAValidSlmcNo(this string slmcNumber)
        => !string.IsNullOrEmpty(slmcNumber) && _slmcRx.IsMatch(slmcNumber);

    public static bool IsAValidMobileNumber(this string phoneNumber)
        => int.TryParse(phoneNumber, out _) && phoneNumber.Length == 10;

    public static bool IsAValidBirthDay(this string birthday)
    {
        if (!DateTime.TryParse(birthday, out var birthDate))
            return false;

        var currentDate = DateTime.UtcNow;
        var age = currentDate.Year - birthDate.Year;

        if (birthDate > currentDate || age < 18)
            return false;

        if (birthDate > currentDate.AddYears(-age))
            age--;

        return age >= 18;
    }

    public static bool IsAValidGender(this string gender)
        => gender is "Male" or "Female" or "Other";
}