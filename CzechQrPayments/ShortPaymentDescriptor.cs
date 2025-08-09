namespace CzechQrPayments;

public class ShortPaymentDescriptor
{
    /// <summary>
    /// The counterparty identifier (IBAN plus an optional BIC).
    /// </summary>
    /// <example>ACC:CZ5855000000001265098001+RZBCCZPP</example>
    /// <remarks>Field name: ACC</remarks>
    public required Counterparty Counterparty { get; set; }

    /// <summary>
    /// An optional list of alternative counterparty identifiers (IBAN plus an optional BIC).
    /// This can be used to provide additional payment options, typically to optimize fees.
    /// </summary>
    /// <example>
    /// [
    ///     "CZ5855000000001265098001+RZBCCZPP",
    ///     "CZ5855000000001265098001"
    /// ]
    /// </example>
    /// <remarks>Field name: ALT-ACC</remarks>
    public List<Counterparty> AlternativeCounterparties { get; set; } = [];

    /// <summary>
    /// The amount to be paid. A value between 0 and 999,999,999.99, with up to two decimal places.
    /// </summary>
    /// <example>480.55</example>
    /// <remarks>Field name: AM</remarks>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// The currency of the payment, represented as a three-letter ISO 4217 code.
    /// </summary>
    /// <example>CZK</example>
    /// <remarks>CC</remarks>
    public string? Currency { get; set; }
    
    /// <summary>
    /// A numeric payment identifier visible to the recipient, up to 16 digits.
    /// </summary>
    /// <example>1234567890123456</example>
    /// <remarks>Field name: RF</remarks>
    public string? CreditorReference { get; set; }
    
    /// <summary>
    /// The name of the payment recipient (payee).
    /// </summary>
    /// <example>PETR DVORAK</example>
    /// <remarks>Field name: RN</remarks>
    public string? CreditorName { get; set; }

    /// <summary>
    /// The payment due date. If the FRQ field is not empty, the DT field is interpreted as the date of the first
    /// payment of a standing order. If the string begins with the header SCD*, the DT field is interpreted as
    /// the start date of the validity of the direct debit authorization.
    /// </summary>
    /// <example>20250809</example>
    /// <remarks>Field name: DT</remarks>
    public DateOnly? DueDate { get; set; }

    /// <summary>
    /// The payment type. Currently used only to indicate instant payments.
    /// </summary>
    /// <remarks>Field name: PT</remarks>
    public PaymentType PaymentType { get; set; }

    /// <summary>
    /// Message for the beneficiary. If the FRQ field is not empty, the MSG field is interpreted as the name
    /// of the standing order. If the string begins with the header SCD*, the MSG field is interpreted
    /// as the name of the direct debit authorization.
    /// </summary>
    /// <remarks>Field name: MSG</remarks>
    public string? Message { get; set; }

    /// <summary>
    /// Identification of the channel for sending a notification to the payment issuer.
    /// </summary>
    /// <remarks>Field name: NT</remarks>
    public NotificationType NotificationType { get; set; }

    /// <summary>
    /// Either a phone number or an email address for sending a notification to the payment issuer.
    /// See <see cref="NotificationType"/>.
    /// </summary>
    /// <example>004200123456789, +420123456789, frantisek.koudelka@abc.cz</example>
    /// <remarks>Field name: NTA</remarks>
    public string? NotificationAddress { get; set; }

    /// <summary>
    /// If the FRQ field is not empty, the DL field is interpreted as the expiry date of the standing order.
    /// </summary>
    /// <remarks>Field name: DL</remarks>
    public DateOnly StandingOrderExpiryDate { get; set; }

    /// <summary>
    /// The payment frequency. If the FRQ field is not empty, the entire SPAYD string is interpreted as an instruction
    /// for a standing order, and the FRQ field is interpreted as the payment frequency of the standing order.
    /// If the string begins with the header SCD*, the FRQ field is interpreted as the period for the direct debit
    /// limit, i.e., the period during which the amount of the executed direct debit must not exceed the specified
    /// limit (AM field).
    /// </summary>
    /// <remarks>Field name: FRQ</remarks>
    public PaymentFrequency? PaymentFrequency { get; set; }

    /// <summary>
    /// Whether to execute the standing order or payments based on the established direct debit
    /// authorization after the account holder’s death.
    /// </summary>
    /// <remarks>Field name: DH</remarks>
    public bool? KeepExecutingAfterDeath { get; set; }

    /// <summary>
    /// The number of days during which attempts should be made to reprocess a failed payment (e.g., due to insufficient
    /// funds in the payer’s account).
    /// </summary>
    /// <remarks>Field name: X-PER</remarks>
    public int? RetryCountLimit { get; set; }

    /// <summary>
    /// 'Variabilní symbol' - a payment identifier specific to Czech banking, often used for invoice references.
    /// </summary>
    /// <example>1234567890</example>
    /// <remarks>Field name: X-VS</remarks>
    public string? VariableSymbol { get; set; }
    
    /// <summary>
    /// 'Specifický symbol' - a payment identifier specific to Czech banking.
    /// </summary>
    /// <example>1234567890</example>
    /// <remarks>Field name: X-SS</remarks>
    public string? SpecificSymbol { get; set; }
    
    /// <summary>
    /// 'Konstantní symbol' - a payment identifier specific to Czech banking.
    /// </summary>
    /// <example>1234567890</example>
    /// <remarks>Field name: X-KS</remarks>
    public string? ConstantSymbol { get; set; }

    /// <summary>
    /// Payment identifier on the payer’s side. This is an internal ID whose use and interpretation depend
    /// on the payer’s bank. It can be used, for example, as an identifier for an e-commerce payment,
    /// or for statistical or marketing purposes.
    /// </summary>
    /// <example>ABCDEFGHIJ1234567890</example>
    /// <remarks>Field name: X-ID</remarks>
    public string? PayerInternalPaymentIdentifier { get; set; }

    /// <summary>
    /// A URL. The specification is very vague about this one. Also, it suggests that the URL should be capitalized
    /// which won't work well in practice.
    /// </summary>
    /// <example>HTTP://WWW.SOMEURL.COM</example>
    /// <remarks>Field name: X-URL</remarks>
    public string? Url { get; set; }

    /// <summary>
    /// A note for the payer’s own use.
    /// </summary>
    /// <example>PLATBA ZA TELCO SLUZBY</example>
    /// <remarks>Field name: X-SELF</remarks>
    public string? NoteToSelf { get; set; }
}