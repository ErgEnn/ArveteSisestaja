namespace BLL
{
    public class PriaPDFGenerator
    {
        public async Task<IDictionary<string, byte[]>> GeneratePDFs(IReadOnlyCollection<MappedInvoice> invoices)
        {
            var dict = new Dictionary<string, byte[]>();
            foreach (var invoice in invoices)
            {
                var pdf = invoice.Invoice.Pdf;
                foreach (var priaItemsGrouping in invoice.Items.Where(item => item.PriaCategory is not null).GroupBy(item => item.PlainName))
                {
                    int matchesCount = 0;
                    var needle = priaItemsGrouping.Key;
                    while (needle.Length > 0)
                    {
                        var higlightedPdf = PdfHighlighter.PdfHighlighter.HighlightTextInPdf(pdf, priaItemsGrouping.First().PriaCategory.Color, needle, out matchesCount);
                        if (matchesCount == 0)
                        {
                            needle = needle.Substring(0, Math.Max(needle.LastIndexOf(' '), 0));
                        }else if (matchesCount > priaItemsGrouping.Count())
                        {
                            break;
                        }
                        else
                        {
                            pdf = higlightedPdf;
                            break;
                        }
                    }
                    invoice.PdfAlterDetails.HighlightingResult.Add((priaItemsGrouping.Key, priaItemsGrouping.Count(), matchesCount));
                }

                if (invoice.Invoice.Pdf != pdf)
                {
                    invoice.AlteredPdf = pdf;
                    dict.Add($"{invoice.Invoice.InvoiceSender}_{invoice.Invoice.InvoiceNo}.pdf", pdf);
                }
                
            }

            return dict;
        }
    }
}
