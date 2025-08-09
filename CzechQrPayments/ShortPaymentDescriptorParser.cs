using System.Text.RegularExpressions;
using CzechQrPayments.Helpers;

namespace CzechQrPayments;

public static partial class ShortPaymentDescriptorParser
{
    public static ShortPaymentDescriptor Parse(string spdString)
    {
        // Make sure the string starts with 'SPD*1.0*'.
        EnsureSupportedHeader(spdString);
        
        // Everything that follows after the header has this format:
        // key1:value1*key2:value2*key3:value3*...
        var afterHeader = spdString[ExpectedHeader.Length..];
        
        var matches = SplitAttributesRegex().Matches(afterHeader);
        var attrs = new Dictionary<string, string>(StringComparer.Ordinal);
        
        // Make a dictionary of attributes.
        foreach (Match m in matches)
        {
            var key = m.Groups["key"].Value;
            var value = m.Groups["value"].Value.Trim();

            // Make sure that every key is present just once.
            if (!attrs.TryAdd(key, value))
                throw new FormatException($"Duplicate key found: {key}");
        }

        if (!attrs.TryGetValue("ACC", out var acc))
        {
            throw new FormatException("The mandatory 'ACC' attribute is missing!");
        }
        
        var descriptor = new ShortPaymentDescriptor
        {
            Counterparty = CounterpartyParser.Parse(acc),
        };
        
        if (attrs.TryGetValue("ALT-ACC", out var altAcc))
        {
            descriptor.AlternativeCounterparties.AddRange(altAcc.Split(',').Select(CounterpartyParser.Parse));
        }
        
        if (attrs.TryGetValue("AM", out var am))
        {
            descriptor.Amount = Parsing.ParseDecimal(am);
        }
        
        if (attrs.TryGetValue("CC", out var cur))
        {
            descriptor.Currency = Parsing.ParseCurrencyCode(cur);
        }

        if (attrs.TryGetValue("RF", out var rf))
        {
            descriptor.CreditorReference = Parsing.ParseNumericString(rf);
        }
        
        if (attrs.TryGetValue("RN", out var rn))
        {
            descriptor.CreditorName = Parsing.ParseString(rn);
        }
        
        if (attrs.TryGetValue("DT", out var dt))
        {
            descriptor.DueDate = Parsing.ParseDate(dt);
        }
        
        if (attrs.TryGetValue("PT", out var pt))
        {
            descriptor.PaymentType = Parsing.ParsePaymentType(pt);
        }
        
        if (attrs.TryGetValue("MSG", out var msg))
        {
            descriptor.Message = Parsing.ParseString(msg);
        }
        
        if (attrs.TryGetValue("NT", out var nt))
        {
            descriptor.NotificationType = Parsing.ParseNotificationType(nt);
        }
        
        if (attrs.TryGetValue("NTA", out var nta))
        {
            descriptor.NotificationAddress = Parsing.ParseString(nta);
        }
        
        if (attrs.TryGetValue("DL", out var dl))
        {
            descriptor.StandingOrderExpiryDate = Parsing.ParseDate(dl);
        }
        
        if (attrs.TryGetValue("FRQ", out var frq))
        {
            descriptor.PaymentFrequency = Parsing.ParsePaymentFrequency(frq);
        }
        
        if (attrs.TryGetValue("DH", out var dh))
        {
            descriptor.KeepExecutingAfterDeath = Parsing.ParseBoolean(dh);
        }
        
        if (attrs.TryGetValue("X-PER", out var xper))
        {
            descriptor.RetryCountLimit = Parsing.ParseNumber(xper);
        }
        
        if (attrs.TryGetValue("X-VS", out var xvs))
        {
            descriptor.VariableSymbol = Parsing.ParseNumericString(xvs);
        }
        
        if (attrs.TryGetValue("X-SS", out var xss))
        {
            descriptor.SpecificSymbol = Parsing.ParseNumericString(xss);
        }
        
        if (attrs.TryGetValue("X-KS", out var xks))
        {
            descriptor.ConstantSymbol = Parsing.ParseNumericString(xks);
        }
        
        if (attrs.TryGetValue("X-ID", out var xid))
        {
            descriptor.PayerInternalPaymentIdentifier = Parsing.ParseString(xid);
        }
        
        if (attrs.TryGetValue("X-URL", out var xurl))
        {
            descriptor.Url = Parsing.ParseString(xurl);
        }
        
        if (attrs.TryGetValue("X-SELF", out var xself))
        {
            descriptor.NoteToSelf = Parsing.ParseString(xself);
        }

        return descriptor;
    }

    private static void EnsureSupportedHeader(string spdString)
    {
        if (spdString.StartsWith(ExpectedHeader))
            return;
        
        // If it's a collection, not a payment descriptor ('Trvalý příkaz k úhradě' in Czech)
        if (spdString.StartsWith("SCD*"))
        {
            throw new NotSupportedException("Short collection descriptors (SCD) are not supported!");
        }

        // If it's a payment descriptor but of an unsupported version.
        if (spdString.StartsWith("SPD*") && !spdString.StartsWith(ExpectedHeader))
        {
            throw new NotSupportedException("Only short payment descriptors (SPD) version 1.0 are supported!");
        }
        
        // If it's a completely different code (such as, a milk carton EAN rather than a payment descriptor)
        throw new FormatException("The provided string does not looks like a valid short payment descriptor (SPD) or short collection descriptor (SCD).");
    }
    
    private const string ExpectedHeader = "SPD*1.0*";

    [GeneratedRegex(@"(?:^|\*)(?<key>[A-Z0-9-]+):(?<value>[^*]*)")]
    private static partial Regex SplitAttributesRegex();
}