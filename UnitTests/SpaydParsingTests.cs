using CzechQrPayments.Helpers;

namespace CzechQrPayments.UnitTests;

public class SpaydParsingTests
{
    [Theory]
    [InlineData("123.45", 123.45)]
    [InlineData("0", 0)]
    public void ParseDecimal_ValidInput_ReturnsDecimal(string input, double expected)
    {
        var result = SpaydParsing.ParseDecimal(input);
        Assert.Equal((decimal)expected, result);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("-10.5")]
    public void ParseDecimal_InvalidInput_ThrowsFormatException(string input)
    {
        Assert.Throws<FormatException>(() => SpaydParsing.ParseDecimal(input));
    }

    [Theory]
    [InlineData("CZK")]
    [InlineData("EUR")]
    public void ParseCurrency_ValidInput_ReturnsString(string input)
    {
        var result = SpaydParsing.ParseCurrencyCode(input);
        Assert.Equal(input, result);
    }

    [Theory]
    [InlineData("CZ")]
    [InlineData("EURO")]
    public void ParseCurrency_InvalidInput_ThrowsFormatException(string input)
    {
        Assert.Throws<FormatException>(() => SpaydParsing.ParseCurrencyCode(input));
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("0")]
    public void ParseNumericString_ValidInput_ReturnsString(string input)
    {
        var result = SpaydParsing.ParseNumericString(input);
        Assert.Equal(input, result);
    }

    [Theory]
    [InlineData("123a")]
    [InlineData("")]
    [InlineData(null)]
    public void ParseNumericString_InvalidInput_ThrowsFormatException(string? input)
    {
        Assert.Throws<FormatException>(() => SpaydParsing.ParseNumericString(input!));
    }

    [Theory]
    [InlineData("123", 123)]
    [InlineData("0", 0)]
    [InlineData("-10", -10)]
    public void ParseNumber_ValidInput_ReturnsInt(string input, int expected)
    {
        var result = SpaydParsing.ParseNumber(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseNumber_InvalidInput_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => SpaydParsing.ParseNumber("invalid"));
    }

    [Fact]
    public void ParseDate_ValidInput_ReturnsDateOnly()
    {
        var result = SpaydParsing.ParseDate("20250809");
        Assert.Equal(new DateOnly(2025, 8, 9), result);
    }

    [Theory]
    [InlineData("2025-08-09")]
    [InlineData("invalid")]
    public void ParseDate_InvalidInput_ThrowsFormatException(string input)
    {
        Assert.Throws<FormatException>(() => SpaydParsing.ParseDate(input));
    }

    [Fact]
    public void ParsePaymentType_ValidInput_ReturnsPaymentType()
    {
        var result = SpaydParsing.ParsePaymentType("IP");
        Assert.Equal(PaymentType.InstantPayment, result);
    }

    [Fact]
    public void ParsePaymentType_InvalidInput_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => SpaydParsing.ParsePaymentType("invalid"));
    }

    [Theory]
    [InlineData("P", NotificationType.Phone)]
    [InlineData("E", NotificationType.Email)]
    public void ParseNotificationType_ValidInput_ReturnsNotificationType(string input, NotificationType expected)
    {
        var result = SpaydParsing.ParseNotificationType(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseNotificationType_InvalidInput_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => SpaydParsing.ParseNotificationType("invalid"));
    }

    [Theory]
    [InlineData("1D", PaymentFrequency.Daily)]
    [InlineData("1M", PaymentFrequency.Monthly)]
    [InlineData("3M", PaymentFrequency.Quarterly)]
    [InlineData("6M", PaymentFrequency.HalfYearly)]
    [InlineData("1Y", PaymentFrequency.Yearly)]
    public void ParsePaymentFrequency_ValidInput_ReturnsPaymentFrequency(string input, PaymentFrequency expected)
    {
        var result = SpaydParsing.ParsePaymentFrequency(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParsePaymentFrequency_InvalidInput_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => SpaydParsing.ParsePaymentFrequency("invalid"));
    }

    [Theory]
    [InlineData("0", false)]
    [InlineData("1", true)]
    public void ParseBoolean_ValidInput_ReturnsBoolean(string input, bool expected)
    {
        var result = SpaydParsing.ParseBoolean(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("true")]
    [InlineData("invalid")]
    public void ParseBoolean_InvalidInput_ThrowsFormatException(string input)
    {
        Assert.Throws<FormatException>(() => SpaydParsing.ParseBoolean(input));
    }

    [Theory]
    [InlineData("test", "test")]
    [InlineData("hello%20world", "hello world")]
    [InlineData("příliš%20žluťoučký%20kůň", "příliš žluťoučký kůň")]
    [InlineData("příliš žluťoučký kůň", "příliš žluťoučký kůň")]
    public void ParseString_ValidInput_ReturnsString(string input, string expected)
    {
        var result = SpaydParsing.ParseString(input);
        Assert.Equal(expected, result);
    }
}