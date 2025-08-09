namespace CzechQrPayments.UnitTests;

public class CounterpartyParserTests
{
    [Fact]
    public void CounterpartyParser_GivenNullThrows()
    {
        string input = null!;
        Assert.Throws<ArgumentNullException>(() => CounterpartyParser.Parse(input));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("CZ5855000000001265098001+")]
    [InlineData("+RZBCCZPP")]
    public void CounterpartyParser_GivenEmptyStringThrows(string? input)
    {
        Assert.Throws<FormatException>(() => CounterpartyParser.Parse(input!));
    }
    
    [Fact]
    public void CounterpartyParser_GivenJustIbanReturnCounterpartyWithIban()
    {
        var input = "CZ5855000000001265098001";
        var counterparty = CounterpartyParser.Parse(input);
        
        Assert.Equal(input, counterparty.Iban);
        Assert.Null(counterparty.Bic);
    }
    
    [Fact]
    public void CounterpartyParser_GivenIbanAndBicReturnCounterpartyWithIbanAndBic()
    {
        var input = "CZ5855000000001265098001+RZBCCZPP";
        var counterparty = CounterpartyParser.Parse(input);
        
        Assert.Equal("CZ5855000000001265098001", counterparty.Iban);
        Assert.Equal("RZBCCZPP", counterparty.Bic);
    }

    [Fact]
    public void CounterpartyParser_GivenStringWithTooManyPartsThrows()
    {
        var input = "iban+bic+something_else";
        Assert.Throws<FormatException>(() => CounterpartyParser.Parse(input));
    }
}