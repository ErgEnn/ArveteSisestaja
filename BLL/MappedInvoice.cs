using InvoiceDownloader;

namespace BLL
{
    public class MappedInvoice(Invoice invoice)
    {
        public Invoice Invoice { get; } = invoice;
        public EInvoice.E_Invoice? EInvoice { get; set; }
        public Exception? EInvoiceParsingError { get; set; }
        public bool? ExistsInAnc { get; init; }
        public IReadOnlyCollection<InvoiceItem>? Items { get; set; }

        public bool? HasUnmappedItems => Items?.Any(item => item.Mapping is {AncClassifierId:0});

        public byte[]? AlteredPdf { get; set; }
        public PdfAlterDetails PdfAlterDetails { get; set; } = new PdfAlterDetails();

        public class InvoiceItem
        {
            public EInvoice.ItemEntry EInvoiceItem { get; init; }
            public AncClassifierMapping? Mapping { get; set; }
            public decimal TotalBeforeVat => EInvoiceItem.ItemSum;
            public decimal TotalInclVat => EInvoiceItem.ItemTotal;
            public decimal UnitPriceOnInvoiceBeforeVat => EInvoiceItem.ItemDetailInfo.Single().ItemPrice;
            public decimal KgPrice => AmountInKg.Value == 0 ? 0: TotalBeforeVat / AmountInKg.Value;
            public decimal Amount => EInvoiceItem.ItemDetailInfo.Single().ItemAmount;
            public decimal? AmountInKg => (Mapping?.Multiplier).Mul(Amount).Div(1000);
            public string PlainName => EInvoiceItem.Description.ReplaceLineEndings("");
            public MappedInvoice Invoice { get; init; }
            public PriaCategory? PriaCategory { get; set; }

        }
    }

    public class PdfAlterDetails
    {
        public IList<(string itemName, int expectedRows, int realRows)> HighlightingResult { get; set; } =
            new List<(string itemName, int expectedRows, int realRows)>();
    }
}
