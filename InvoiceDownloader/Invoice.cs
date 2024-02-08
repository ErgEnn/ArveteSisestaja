namespace InvoiceDownloader
{
    public record Invoice(string InvoiceNo, string InvoiceSender, DateOnly InvoiceDate, string? XML, string PdfSrc);
}
