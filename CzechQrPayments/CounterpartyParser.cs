namespace CzechQrPayments;

internal static class CounterpartyParser
{
    public static Counterparty Parse(string ibanBicStr)
    {
        ArgumentNullException.ThrowIfNull(ibanBicStr);
        
        var parts = ibanBicStr.Split('+');
        
        if (parts.Length is 0 or > 2 || ibanBicStr.EndsWith('+'))
        {
            throw new FormatException(ErrorMessage);
        }

        var iban = parts[0];
        var bic = parts.Length > 1 ? parts[1] : null;

        if (string.IsNullOrEmpty(iban))
        {
            throw new FormatException(ErrorMessage);
        }
        
        return new Counterparty { Iban = iban, Bic = bic };
    }
    
    private const string ErrorMessage = "The IBAN+BIC string must be in the format 'IBAN+BIC' or just 'IBAN'!";
}