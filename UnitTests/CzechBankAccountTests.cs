namespace CzechQrPayments.UnitTests;

public class CzechBankAccountTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("000000")]
    [InlineData(" 00 0 ")]
    public void CzechBankAccount_Ctor_EmptyPrefixStoredAsNull(string? prefix)
    {
        var actual = new CzechBankAccount(prefix, "", "");
        Assert.Null(actual.Prefix);
    }

    [Fact]
    public void CzechBankAccount_Ctor_StripsLeadingZerosFromPrefixAndAccountNumberButNotBankCode()
    {
        var actual = new CzechBankAccount("000123", "04567890123", "0800");
        
        Assert.Equal("123", actual.Prefix);
        Assert.Equal("4567890123", actual.AccountNumber);
        Assert.Equal("0800", actual.BankCode);
    }

    [Theory]
    [InlineData("000123-04567890123/0800")]
    [InlineData(" 0  00  123 -   045 6 7 8 9 0  1 2 3 / 0  8 00  ")]
    public void CzechBankAccount_Ctor_ParsesFullAccountNumberCorrectly(string fullAccountNumber)
    {
        var actual = new CzechBankAccount(fullAccountNumber);
        
        Assert.Equal("123", actual.Prefix);
        Assert.Equal("4567890123", actual.AccountNumber);
        Assert.Equal("0800", actual.BankCode);
    }
    
    [Fact]
    public void CzechBankAccount_Ctor_ThrowOnAllZeroAccountNumber()
    {
        Assert.Throws<FormatException>(() => new CzechBankAccount(null, "0000000000", "3030"));
    }

    [Theory]
    [InlineData("00a123-04567890123/0800")]
    [InlineData("00a123-0456a890123/0800")]
    [InlineData("00a123-04567890123/08a0")]
    public void CzechBankAccount_Ctor_ThrowsOnNonNumericChar(string fullAccountNumber)
    {
        Assert.Throws<FormatException>(() => new CzechBankAccount(fullAccountNumber));
    }

    [Theory]
    [InlineData("0000123-04567890123/0800")]
    [InlineData("000123-004567890123/0800")]
    [InlineData("000123-04567890123/00800")]
    [InlineData("000123-0456789012/0800")]
    [InlineData("000123-04567890123/800")]
    public void CzechBankAccount_Ctor_ThrowsOnInvalidSegmentLength(string fullAccountNumber)
    {
        Assert.Throws<FormatException>(() => new CzechBankAccount(fullAccountNumber));
    }

    [Theory]
    [InlineData("CZ6508000000192000145399", "19-2000145399/0800")]
    [InlineData("CZ6907101781240000004159", "178124-4159/0710")]
    [InlineData("CZ4907100000000012345678", "12345678/0710")]
    public void CzechBankAccount_FromIban_ParsesValidIban(string iban, string expectedAccountNumber)
    {
        var expected = new CzechBankAccount(expectedAccountNumber);
        var actual = CzechBankAccount.FromIban(iban);
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CzechBankAccount_ToString_ReturnsFormattedBankAccount()
    {
        var sut = new CzechBankAccount("000123-04567890123/0800");
        Assert.Equal("123-4567890123/0800", sut.ToString());
    }
}