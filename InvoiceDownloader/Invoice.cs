namespace InvoiceDownloader
{
    public record Invoice(string InvoiceNo, string InvoiceSender, DateTime InvoiceDateTime, string? XML, string PdfSrc);
}
