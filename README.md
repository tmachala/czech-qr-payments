# Czech QR Payments

A .NET library for parsing Czech payment order data, typically found in QR codes on invoices and payment slips.

This library does **not** handle QR code scanning or image recognition - it focuses solely on interpreting the payment order string extracted from the QR code.

Implementation follows the Czech Banking Association’s “Format for Sharing Payment Data in CZK – QR Codes” specification (version 1.2, 2021-06-01). See https://www.cbaonline.cz/clanky/format-pro-sdileni-platebnich-udaju-v-czk-qr-kody.

## Supported .NET Versions

|        | Architecture | MacOS | Linux | Windows |
|--------|--------------|:-----:|:-----:|:-------:|
| .NET 9 | ARM          |   ✅   |   ✅   |    ✅    |
| .NET 9 | x64          | ✅     | ✅     |  ✅      |

## Installation

Install the NuGet package:

```sh
dotnet add package CzechQrPayments
```

## Usage

### Parsing Payment QR Codes (SPAYD)

```cs
using CzechQrPayments;

var qrCode = "SPD*1.0*ACC:CZ3301000000000002970297*AM:1000*CC:CZK*RF:1234*X-VS:456*PT:IP";
var spd = ShortPaymentDescriptor.Parse(qrCode);

Console.WriteLine(spd.Counterparty.Iban);  // CZ3301000000000002970297
Console.WriteLine(spd.Amount);             // 1000
Console.WriteLine(spd.Currency);           // CZK
```

### Converting IBAN to Czech Bank Account

```cs
var iban = "CZ6508000000192000145399";
var bankAccount = CzechBankAccount.FromIban(iban);

Console.WriteLine(bankAccount)  // 19-2000145399/0800
```

## Limitations

* **CRC32 checksum –** The specification defines an optional CRC32 checksum, but this library does not implement it. I have not found any real-world examples using it, and QR codes already include error-correcting mechanisms.

* **Direct debit (“inkaso”) –** Not currently supported. Some partial implementation exists, but it has not been tested and is therefore disabled.

* **Bank-specific differences –** Each Czech bank may have slight deviations from the specification, so interoperability is not guaranteed.

## Release Notes

* **Version 1.1.0 -** Added `CzechBankAccount` with an IBAN parsing feature. Both IBAN and Czech bank accounts contain checksums that the library doesn't validate so far.
* **Version 1.0.2 -** The first version. 

## Contributing

Contributions are welcome!

Please open an issue to discuss potential changes or submit a pull request.

## License

MIT