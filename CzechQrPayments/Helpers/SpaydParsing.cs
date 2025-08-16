using System.Globalization;
using System.Net;

namespace CzechQrPayments.Helpers;

internal static class SpaydParsing
{
    public static decimal ParseDecimal(string str)
    {
        var value = decimal.Parse(str, CultureInfo.InvariantCulture);
        
        if (value < 0)
            throw new FormatException("Only non-negative numbers are allowed!");
        
        return value;
    }

    public static string ParseCurrencyCode(string str)
    {
        if (str.Length != 3)
            throw new FormatException($"Invalid currency format: {str}");
        
        return str;
    }

    public static string ParseNumericString(string str)
    {
        if (string.IsNullOrEmpty(str) || str.Any(c => !char.IsDigit(c)))
        {
            throw new FormatException($"The value must be a non-empty string composed only of digits (0 to 9)!");
        }

        return str;
    }

    public static int ParseNumber(string str)
    {
        return int.Parse(str, CultureInfo.InvariantCulture);
    }

    public static DateOnly ParseDate(string str)
    {
        return DateOnly.ParseExact(str, "yyyyMMdd", CultureInfo.InvariantCulture);
    }

    public static PaymentType ParsePaymentType(string str)
    {
        return str switch
        {
            "IP" => PaymentType.InstantPayment,
            _ => throw new FormatException($"Invalid payment type: {str}")
        };
    }

    public static NotificationType ParseNotificationType(string str)
    {
        return str switch
        {
            "P" => NotificationType.Phone,
            "E" => NotificationType.Email,
            _ => throw new FormatException($"Invalid notification type: {str}")
        };
    }

    public static PaymentFrequency ParsePaymentFrequency(string str)
    {
        return str switch
        {
            "1D" => PaymentFrequency.Daily,
            "1M" => PaymentFrequency.Monthly,
            "3M" => PaymentFrequency.Quarterly,
            "6M" => PaymentFrequency.HalfYearly,
            "1Y" => PaymentFrequency.Yearly,
            _ => throw new FormatException($"Invalid payment frequency: {str}")
        };
    }

    public static bool ParseBoolean(string str)
    {
        return str switch
        {
            "0" => false,
            "1" => true,
            _ => throw new FormatException($"Invalid boolean value: {str}")
        };
    }

    public static string ParseString(string str)
    {
        return WebUtility.UrlDecode(str);
    }
}