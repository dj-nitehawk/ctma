using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Ctma;

public static partial class Extensions
{
    static readonly TextInfo _txt = new CultureInfo("en-US", false).TextInfo;

    internal static string TitleCase(this string value)
        => _txt.ToTitleCase(value.Trim());

    internal static string LowerCase(this string value)
        => _txt.ToLower(value.Trim());

    internal static string UpperCase(this string value)
        => _txt.ToUpper(value.Trim());

    internal static bool HasValue([NotNullWhen(true)] this string? value)
        => !string.IsNullOrEmpty(value);

    internal static bool HasNoValue([NotNullWhen(false)] this string? value)
        => string.IsNullOrEmpty(value);

    static readonly Regex _nicRx1 = NicRx1();
    static readonly Regex _nicRx2 = NicRx2();

    [GeneratedRegex(@"^\d{9}[x|X|v|V]$")]
    private static partial Regex NicRx1();

    [GeneratedRegex(@"^\d{12}$")]
    private static partial Regex NicRx2();

    internal static bool IsAValidNic(this string nicNumber)
        => !string.IsNullOrEmpty(nicNumber) && (_nicRx1.IsMatch(nicNumber) || _nicRx2.IsMatch(nicNumber));

    static readonly Regex _slmcRx = SlmcRx();

    [GeneratedRegex(@"^\d{1,5}$")]
    private static partial Regex SlmcRx();

    internal static bool IsAValidSlmcNo(this string slmcNumber)
        => !string.IsNullOrEmpty(slmcNumber) && _slmcRx.IsMatch(slmcNumber);

    internal static bool IsAValidMobileNumber(this string phoneNumber)
        => phoneNumber.Length == 10;

    internal static bool IsAValidBirthDay(this string birthday)
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

    internal static bool IsAValidGender(this string gender)
        => gender is "Male" or "Female" or "Other";
}