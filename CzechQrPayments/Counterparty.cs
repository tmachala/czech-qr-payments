namespace CzechQrPayments;

public class Counterparty
{
    /// <summary>
    /// The IBAN (International Bank Account Number) of the counterparty.
    /// </summary>
    public required string Iban { get; set; }
    
    /// <summary>
    /// The bank identification code (BIC) or SWIFT code of the counterparty's bank.
    /// </summary>
    public string? Bic { get; set; }
}