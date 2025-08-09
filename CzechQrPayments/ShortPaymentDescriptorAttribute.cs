namespace CzechQrPayments;

internal class ShortPaymentDescriptorAttribute
{
    public required string Key { get; init; }
    public required string Value { get; init; }

    public ShortPaymentDescriptorAttribute(string attribute)
    {
        var parts = attribute.Split(":");

        if (parts.Length != 2)
        {
            throw new FormatException("The SPD attribute must be in the format 'key:value'!");
        }

        Key = parts[0];
        Value = parts[1];

        if (string.IsNullOrEmpty(Key))
        {
            throw new FormatException("The SPD attribute key cannot be empty!");
        }

        if (string.IsNullOrEmpty(Value))
        {
            throw new FormatException("The SPD attribute value cannot be empty!");
        }
    }
}