using iTextSharp.text;
using System.IO.Compression;
using System.Text;

namespace BLL
{
    public class PriaReportService
    {
        private readonly InvoiceService _invoiceService;
        private readonly AncMapper _ancMapper;
        private readonly PriaExcelReportGenerator _excelReportGenerator;
        private readonly PriaPDFGenerator _pdfGenerator;

        private PriaCategory[] _priaCategories =
        [
            new PriaCategory
            {
                CategoryName = "piim",
                ItemMatcher = item => (new []{ 60797, 120821 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "keefir",
                ItemMatcher = item => (new[] { 60718 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "maits_jogurt",
                ItemMatcher = item => (new[] { 60697 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "oun",
                ItemMatcher = item => (new[] { 60792 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "pirn",
                ItemMatcher = item => (new[] { 60799 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "marjad",
                ItemMatcher = item => (new[] { 60773 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "astelpaju",
                ItemMatcher = item => (new[] { 145296 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "kreeka_jogurt",
                ItemMatcher = item => (new[] { 253325 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "paprika",
                ItemMatcher = item =>
                {
                    return (new[] { 60793 }).Contains(item.Mapping.AncClassifierId) 
                           && item.Invoice.Invoice.InvoiceSender == "TSITRUS KAUBANDUS OÜ";
                }
            },
            new PriaCategory
            {
                CategoryName = "redis",
                ItemMatcher = item => (new[] { 60808 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "kurk",
                ItemMatcher = item => (new[] { 60739 }).Contains(item.Mapping.AncClassifierId)
            },
            new PriaCategory
            {
                CategoryName = "porgand",
                ItemMatcher = item =>
                {
                    return (new[] { 60802 }).Contains(item.Mapping.AncClassifierId)
                           && item.Invoice.Invoice.InvoiceSender == "osaühing Lemel RH";
                }
            },
        ];

        public PriaReportService(
            InvoiceService invoiceService,
            AncMapper ancMapper,
            PriaExcelReportGenerator excelReportGenerator,
            PriaPDFGenerator pdfGenerator)
        {
            _invoiceService = invoiceService;
            _ancMapper = ancMapper;
            _excelReportGenerator = excelReportGenerator;
            _pdfGenerator = pdfGenerator;
        }

        public async Task<byte[]> GenerateReports(DateOnly startDate, DateOnly endDate)
        {
            var invoices = await _invoiceService.GetInvoices(startDate, endDate);
            await _ancMapper.MapAncClassifiersToInvoiceItems(invoices);
            MapInvoiceItemsToPriaCategories(invoices);
            var excels = await GenerateExcelsForCategories(startDate, endDate);
            var pdfs = _pdfGenerator.GeneratePDFs(invoices, _priaCategories);
            var files = excels.Union(pdfs).ToDictionary(pair => pair.Key, pair => pair.Value);
            var pdfReport = GeneratePdfDetailsReport(invoices);
            files.Add("pdf_report.txt",pdfReport);
            return CreateZipArchive(files);
        }

        private byte[] GeneratePdfDetailsReport(IReadOnlyCollection<MappedInvoice> invoices)
        {
            var sb = new StringBuilder();
            foreach (var invoice in invoices)
            {
                var results = invoice.PdfAlterDetails.HighlightingResult;
                foreach ((string itemName, int expectedRows, int realRows) tuple in results)
                {
                    if (tuple.expectedRows != tuple.realRows)
                        sb.AppendLine($"{invoice.Invoice.InvoiceSender}_{invoice.Invoice.InvoiceNo}.pdf     {tuple.itemName}    {tuple.realRows}/{tuple.expectedRows}");
                }
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        byte[] CreateZipArchive(Dictionary<string, byte[]> files)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var fileEntry in files)
                    {
                        var zipEntry = archive.CreateEntry(fileEntry.Key, CompressionLevel.Fastest);
                        using (var zipEntryStream = zipEntry.Open())
                        {
                            zipEntryStream.Write(fileEntry.Value, 0, fileEntry.Value.Length);
                        }
                    }
                }
                return memoryStream.ToArray();
            }
        }

        private async Task<IDictionary<string, byte[]>> GenerateExcelsForCategories(DateOnly startDate, DateOnly endDate)
        {
            var dict = new Dictionary<string, byte[]>();
            foreach (var category in _priaCategories)
            {
                var xlsx = await _excelReportGenerator.GenerateExcel(category);
                category.Excel = xlsx;
                dict.Add($"pria_{category.CategoryName}_{startDate:ddMMMyy}_{endDate:ddMMMyy}.xlsx", category.Excel);
            }

            return dict;
        }

        private void MapInvoiceItemsToPriaCategories(IReadOnlyCollection<MappedInvoice> invoices)
        {
            foreach (var item in invoices.AllItems())
            {
                item.PriaCategory = GetCategory(item);
            }
        }

        private PriaCategory? GetCategory(MappedInvoice.InvoiceItem item)
        {
            foreach (var category in _priaCategories)
            {
                if (category.ItemMatcher(item))
                {
                    category.Invoices.Add(item.Invoice);
                    return category;
                }
                    
            }

            return null;
        }
    }

    public class PriaCategory
    {
        public required string CategoryName { get; init; }
        public required Predicate<MappedInvoice.InvoiceItem> ItemMatcher { get; init; }

        public ISet<MappedInvoice> Invoices { get; } = new HashSet<MappedInvoice>();
        public byte[]? Excel { get; set; }
    }
}
