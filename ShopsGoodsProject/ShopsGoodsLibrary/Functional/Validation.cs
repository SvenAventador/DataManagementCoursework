using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace ShopsGoodsLibrary.Functional;

public static partial class Validation
{
    [GeneratedRegex(@"^[-\w.]+@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,4}$")]
    private static partial Regex EmailRegex();
    public static bool ValidEmail(string email)
    {
        var regex = EmailRegex();
        return regex.IsMatch(email);
    }


    [GeneratedRegex(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?\d{3}[\- ]?\d{2}[\- ]?\d{2}$")]
    private static partial Regex PhoneRegex();
    public static bool ValidPhone(string phone)
    {
        var regex = PhoneRegex();
        return regex.IsMatch(phone);
    }

    public static string GetHashString(string password)
    {
        var bytes = Encoding.Unicode.GetBytes(password);
        var byteHash = MD5.HashData(bytes);
        return byteHash.Aggregate("", (current, b) => current + $"{b:x2}");
    }
}