namespace CzechQrPayments.UnitTests;

public class ShortPaymentDescriptorParserTests
{
    [Fact]
    public void ShortPaymentDescriptorParser_CanParseSamplePayment1()
    {
        var input = "SPD*1.0*ACC:CZ3301000000000002970297*AM:555.55*CC:CZK*RF:7004139146*X-VS:0987654321*X-SS:1234567890*X-KS:0558*DT:20210430*MSG:PRISPEVEK NA NADACI";
        
        var result = ShortPaymentDescriptorParser.Parse(input);

        Assert.Equal("CZ3301000000000002970297", result.Counterparty.Iban);
        Assert.Null(result.Counterparty.Bic);
        Assert.Empty(result.AlternativeCounterparties);
        Assert.Equal(555.55m, result.Amount);
        Assert.Equal("CZK", result.Currency);
        Assert.Equal("7004139146", result.CreditorReference);
        Assert.Null(result.CreditorName);
        Assert.Equal(new DateOnly(2021, 4, 30), result.DueDate);
        Assert.Equal(PaymentType.Unspecified, result.PaymentType);
        Assert.Equal("PRISPEVEK NA NADACI", result.Message);
        Assert.Equal(NotificationType.Unspecified, NotificationType.Unspecified);
        Assert.Null(result.StandingOrderExpiryDate);
        Assert.Equal(PaymentFrequency.Unspecified, result.PaymentFrequency);
        Assert.Null(result.KeepExecutingAfterDeath);
        Assert.Null(result.RetryCountLimit);
        Assert.Equal("0987654321", result.VariableSymbol);
        Assert.Equal("1234567890", result.SpecificSymbol);
        Assert.Equal("0558", result.ConstantSymbol);
        Assert.Null(result.PayerInternalPaymentIdentifier);
        Assert.Null(result.Url);
        Assert.Null(result.NoteToSelf);
    }
    
    [Fact]
    public void ShortPaymentDescriptorParser_CanParseSamplePayment2()
    {
        var input = "SPD*1.0*ACC:CZ3301000000000002970297*AM:555.55*CC:CZK*RF:7004139146*X-VS:0987654321*X-SS:1234567890*X-KS:0558*PT:IP*MSG:PRISPEVEK NA NADACI";
        
        var result = ShortPaymentDescriptorParser.Parse(input);
        
        // The rest is omitted since it's the same as in the Sample 1.
        Assert.Equal(PaymentType.InstantPayment, result.PaymentType);
        Assert.Null(result.DueDate);
    }
    
    [Fact]
    public void ShortPaymentDescriptorParser_CanParseSamplePayment3()
    {
        var input = "SPD*1.0*ACC:CZ3301000000000002970297*AM:555.55*CC:CZK*FRQ:1M*DT:20210430*DL:20230430*DH:0*MSG:PRAVIDELNY PRISPEVEK NA NADACI";
        
        var result = ShortPaymentDescriptorParser.Parse(input);

        Assert.Equal("CZ3301000000000002970297", result.Counterparty.Iban);
        Assert.Null(result.Counterparty.Bic);
        Assert.Empty(result.AlternativeCounterparties);
        Assert.Equal(555.55m, result.Amount);
        Assert.Equal("CZK", result.Currency);
        Assert.Null(result.CreditorReference);
        Assert.Null(result.CreditorName);
        Assert.Equal(new DateOnly(2021, 4, 30), result.DueDate);
        Assert.Equal(PaymentType.Unspecified, result.PaymentType);
        Assert.Equal("PRAVIDELNY PRISPEVEK NA NADACI", result.Message);
        Assert.Equal(NotificationType.Unspecified, NotificationType.Unspecified);
        Assert.Equal(new DateOnly(2023, 4, 30), result.StandingOrderExpiryDate);
        Assert.Equal(PaymentFrequency.Monthly, result.PaymentFrequency);
        Assert.False(result.KeepExecutingAfterDeath);
        Assert.Null(result.RetryCountLimit);
        Assert.Null(result.VariableSymbol);
        Assert.Null(result.SpecificSymbol);
        Assert.Null(result.ConstantSymbol);
        Assert.Null(result.PayerInternalPaymentIdentifier);
        Assert.Null(result.Url);
        Assert.Null(result.NoteToSelf);
    }

    [Fact]
    public void ShortPaymentDescriptorParser_GivenScdThrows()
    {
        var input = "SCD*1.0*ACC:CZ3301000000000002970297*AM:555.55*CC:CZK*FRQ:1M*DT:20210430*DL:20260430*DH:0*MSG:PRAVIDELNY PRISPEVEK NA NADACI";
        Assert.Throws<NotSupportedException>(() => ShortPaymentDescriptorParser.Parse(input));
    }
    
    [Fact]
    public void ShortPaymentDescriptorParser_CanParseColonInAttrValue()
    {
        var input = "SPD*1.0*ACC:CZ3301000000000002970297*MSG:foo:bar";
        
        var result = ShortPaymentDescriptorParser.Parse(input);
        
        Assert.Equal("foo:bar", result.Message);
    }
    
    [Fact]
    public void ShortPaymentDescriptorParser_CanParseUrlEncodedStrings()
    {
        var input = "SPD*1.0*ACC:CZ3301000000000002970297*MSG:%2A%20P%C5%99%C3%ADli%C5%A1%20%C5%BElu%C5%A5ou%C4%8Dk%C3%BD%20k%C5%AF%C5%88%20%C3%BAp%C4%9Bl%20%C4%8F%C3%A1belsk%C3%A9%20%C3%B3dy%20%2A";
        
        var result = ShortPaymentDescriptorParser.Parse(input);
        
        Assert.Equal("* Příliš žluťoučký kůň úpěl ďábelské ódy *", result.Message);
    }
    
    [Fact]
    public void ShortPaymentDescriptorParser_CanParseNonAsciiStrings()
    {
        var input = "SPD*1.0*ACC:CZ3301000000000002970297*MSG:Příliš žluťoučký kůň úpěl ďábelské ódy";
        
        var result = ShortPaymentDescriptorParser.Parse(input);
        
        Assert.Equal("Příliš žluťoučký kůň úpěl ďábelské ódy", result.Message);
    }
    
    [Fact]
    public void ShortPaymentDescriptorParser_CanParseMixedNonAsciiAndUrlEncodedStrings()
    {
        var input = "SPD*1.0*ACC:CZ3301000000000002970297*MSG:%2A Příliš žluťoučký kůň úpěl ďábelské ódy %2A";
        
        var result = ShortPaymentDescriptorParser.Parse(input);
        
        Assert.Equal("* Příliš žluťoučký kůň úpěl ďábelské ódy *", result.Message);
    }
}