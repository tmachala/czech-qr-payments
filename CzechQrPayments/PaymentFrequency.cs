#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace CzechQrPayments;

/// <summary>
/// The frequency at which the payment should be repeated.
/// </summary>
public enum PaymentFrequency
{
    Unspecified = 0,
    Daily = 1,
    Monthly = 2,
    Quarterly = 3,
    HalfYearly = 4,
    Yearly = 5
}