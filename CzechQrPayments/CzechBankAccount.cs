namespace CzechQrPayments;

/// <summary>
/// Represents a Czech bank account in the usual prefix-accountNumber/bankCode form.
/// </summary>
public record CzechBankAccount
{
    /// <summary>
    /// An optional account prefix. Up to 6 digits.
    /// </summary>
    public string? Prefix { get; }
    
    /// <summary>
    /// The account number. Up to 10 digits.
    /// </summary>
    public string AccountNumber { get; }
    
    /// <summary>
    /// The bank code. Always 4 digits.
    /// </summary>
    public string BankCode { get; }

    /// <summary>
    /// Creates a new instance of <see cref="CzechBankAccount" /> from a string in the usual prefix-accountNumber/bankCode format.
    /// </summary>
    /// <param name="fullAccountNumber">A Czech account number in the usual prefix-accountNumber/bankCode format. Prefix is optional.</param>
    /// <exception cref="FormatException">When given <paramref name="fullAccountNumber"/> doesn't look like a valid Czech bank account.</exception>
    public CzechBankAccount(string fullAccountNumber)
    {
        // TODO: implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a new instance of <see cref="CzechBankAccount"/> from an account number segments.
    /// </summary>
    /// <param name="prefix">An optional prefix. Up to 6 digits.</param>
    /// <param name="accountNumber">The account number. Up to 10 digits.</param>
    /// <param name="bankCode">The bank code. Always 4 digits.</param>
    /// <exception cref="FormatException">When given arguments don't make a valid Czech bank account.</exception>
    public CzechBankAccount(string? prefix, string accountNumber, string bankCode)
    {
        // TODO: implement
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a new instance of <see cref="CzechBankAccount"/> from an account number encoded as IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to parse</param>
    /// <exception cref="FormatException">When given <paramref name="iban"/> doesn't seem to represent a valid Czech bank account.</exception>
    public static CzechBankAccount FromIban(string iban)
    {
        // TODO: implement
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override string ToString() => $"{Prefix} {AccountNumber} {BankCode}";
}