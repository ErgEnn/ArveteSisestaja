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
                Classifiers = [60797, 120821],
                Color = BaseColor.YELLOW
            },
            new PriaCategory
            {
                CategoryName = "keefir",
                Classifiers = [60718],
                Color = BaseColor.BLUE
            },
            new PriaCategory
            {
                CategoryName = "maits_jogurt",
                Classifiers = [60697],
                Color = BaseColor.PINK
            },
            new PriaCategory
            {
                CategoryName = "oun",
                Classifiers = [60792],
                Color = BaseColor.RED
            },
            new PriaCategory
            {
                CategoryName = "pirn",
                Classifiers = [60799],
                Color = BaseColor.GREEN
            },
            new PriaCategory
            {
                CategoryName = "marjad",
                Classifiers = [60773],
                Color = BaseColor.ORANGE
            }
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
            var pdfs = await _pdfGenerator.GeneratePDFs(invoices);
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
                dict.Add($"pria_{category.CategoryName}_{startDate:ddMMMyy}_{endDate:ddMMMyy}.xlsx", xlsx);
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
                if (category.Classifiers.Contains(item.Mapping.AncClassifierId))
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
        public string CategoryName { get; init; }
        public int[] Classifiers { get; init; }
        public BaseColor Color { get; init; }

        public ISet<MappedInvoice> Invoices { get; } = new HashSet<MappedInvoice>();
        public byte[]? Excel { get; set; }
    }
}
