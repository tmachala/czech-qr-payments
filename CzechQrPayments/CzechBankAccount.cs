namespace CzechQrPayments;

/// <summary>
/// Represents a Czech bank account in the usual prefix-accountNumber/bankCode form.
/// </summary>
public record CzechBankAccount
{
    /// <summary>
    /// An optional account prefix. Up to 6 digits.
    /// </summary>
    public string? Prefix { get; private set; }
    
    /// <summary>
    /// The account number. Up to 10 digits.
    /// </summary>
    public string AccountNumber { get; private set; } = null!;  // Always set in the Initialize method

    /// <summary>
    /// The bank code. Always 4 digits.
    /// </summary>
    public string BankCode { get; private set; } = null!;  // Always set in the Initialize method

    /// <summary>
    /// Creates a new instance of <see cref="CzechBankAccount" /> from a string in the usual prefix-accountNumber/bankCode format.
    /// </summary>
    /// <param name="fullAccountNumber">A Czech account number in the usual prefix-accountNumber/bankCode format. Prefix is optional.</param>
    /// <exception cref="FormatException">When given <paramref name="fullAccountNumber"/> doesn't look like a valid Czech bank account.</exception>
    public CzechBankAccount(string fullAccountNumber)
    {
        var bySlash = fullAccountNumber.Split('/');
        if (bySlash.Length != 2)
            throw new FormatException("Invalid bank account format!");

        var byDash = bySlash[0].Split('-');
        
        string? prefix;
        string? accNum;
        var bankCode = bySlash[1];

        switch (byDash.Length)
        {
            case 1:
                (prefix, accNum) = (null, byDash[0]);
                break;
            case 2:
                (prefix, accNum) = (byDash[0], byDash[1]);
                break;
            default:
                throw new FormatException("Invalid bank account format!");
        }

        Initialize(prefix, accNum, bankCode);
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
        Initialize(prefix, accountNumber, bankCode);
    }

    /// <summary>
    /// Creates a new instance of <see cref="CzechBankAccount"/> from an account number encoded as IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to parse</param>
    /// <exception cref="FormatException">When given <paramref name="iban"/> doesn't seem to represent a valid Czech bank account.</exception>
    public static CzechBankAccount FromIban(string iban)
    {
        if (iban == null) throw new ArgumentNullException(nameof(iban));
        
        var sanitizedIban = new string(iban.Where(char.IsLetterOrDigit).ToArray());

        if (sanitizedIban.Length != 24 || !sanitizedIban.StartsWith("CZ"))
        {
            throw new FormatException("Not a valid Czech IBAN!");
        }

        var bankCode = sanitizedIban.Substring(4, 4);
        var prefix = sanitizedIban.Substring(8, 6);
        var accountNumber = sanitizedIban.Substring(14, 10);

        return new CzechBankAccount(prefix, accountNumber, bankCode);
    }

    private void Initialize(string? prefix, string accountNumber, string bankCode)
    {
        Prefix = NormalizePrefix(prefix);
        AccountNumber = NormalizeAccountNumber(accountNumber);
        BankCode = NormalizeBankCode(bankCode);
    }

    private static string? NormalizePrefix(string? prefix)
    {
        var pfx = (prefix ?? "").Replace(" ", "");
        if (pfx.All(char.IsDigit) && pfx.Length <= 6)
        {
            // Trim leading zeros only now, after checking the length
            var trimmed = pfx.TrimStart('0');
            return string.IsNullOrEmpty(trimmed) ? null : trimmed;
        }

        throw new FormatException("Invalid bank account format!");
    }
    
    private static string NormalizeAccountNumber(string accountNumber)
    {
        var accNum = accountNumber.Replace(" ", "");

        if (accNum.Length > 0 && accNum.All(c => c == '0'))
            throw new FormatException("The account number cannot be all zeros!");
        
        // Trim leading zeros only now, after checking the length
        if (accNum.All(char.IsDigit) && accNum.Length is >= 2 and <= 10)
            return accNum.TrimStart('0');
        
        throw new FormatException("Invalid bank account format!");
    }

    private static string NormalizeBankCode(string bankCode)
    {
        var code = bankCode.Replace(" ", "");
        if (code.All(char.IsDigit) && code.Length == 4)
            return code;
        
        throw new FormatException("Invalid bank account format!");
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return string.IsNullOrEmpty(Prefix)
            ? $"{AccountNumber}/{BankCode}"
            : $"{Prefix}-{AccountNumber}/{BankCode}";
    }
}