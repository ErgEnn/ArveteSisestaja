namespace InvoiceDownloader
{
    public record Invoice(string InvoiceNo, string InvoiceSender, DateOnly InvoiceDateTime, string? XML, string PdfSrc);
}
