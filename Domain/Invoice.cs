using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Invoice
{
    [StringLength(256)]
    public string Vendor { get; set; }

    [StringLength(32)]
    public string Identifier { get; set; }

    public DateOnly Date { get; set; }

    public string? RawXML { get; set; }
}