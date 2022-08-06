namespace Domain;

public class Invoice
{
    public string Vendor { get; set; }
    public string Identifier { get; set; }
    public DateOnly Date { get; set; }
}