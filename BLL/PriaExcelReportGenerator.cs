using OfficeOpenXml;

namespace BLL;

public class PriaExcelReportGenerator
{
    public PriaExcelReportGenerator()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public class DataCol
    {
        public string Header { get; }
        public Func<MappedInvoice.InvoiceItem, object> DataParser { get; }
        public Func<ExcelRange, string>? FooterFormula { get; }

        public DataCol(string header, Func<MappedInvoice.InvoiceItem, object> dataParser, Func<ExcelRange,string>? footerFormula = null)
        {
            Header = header;
            DataParser = dataParser;
            FooterFormula = footerFormula;
        }
    }

    private readonly DataCol[] _columns = new[]
    {
        new DataCol("Arve kp   ", item => item.Invoice.Invoice.InvoiceDate.ToString("dd.MM.yyyy")),
        new DataCol("Arve väljastaja", item => item.Invoice.Invoice.InvoiceSender),
        new DataCol("Arve nr     ", item => item.Invoice.Invoice.InvoiceNo),
        new DataCol("Hind(ilma km.)", item => item.TotalBeforeVat, range => $"=SUM({range.Address})"),
        new DataCol("Kogus", item => item.AmountInKg, range => $"=SUM({range.Address})"),
    };

    public async Task<byte[]> GenerateExcel(PriaCategory category)
    {
        var xlsx = new ExcelPackage();
        var sheet = xlsx.Workbook.Worksheets.Add(category.CategoryName);
        var rowIter = RowIterator().GetEnumerator();

        AddHeader(sheet, rowIter.Aquire());
        AddItems(sheet, category, rowIter, out var dataStartRow, out var dataEndRow);
        AddFooter(sheet, rowIter.Aquire(), dataStartRow, dataEndRow);

        return await xlsx.GetAsByteArrayAsync();
    }

    private void AddFooter(ExcelWorksheet sheet, int row, int dataStartRow, int dataEndRow)
    {
        foreach (var (col, dataCol) in ColumnIterator())
        {
            if (dataCol.FooterFormula is { } formula)
            {
                if (dataStartRow < row)
                    sheet.Cells[row, col].Formula = formula(sheet.Cells[dataStartRow, col, dataEndRow, col]);
                else
                    sheet.Cells[row, col].Value = "0.00";
            }
        }
    }

    private void AddItems(ExcelWorksheet sheet, PriaCategory category, IEnumerator<int> rowIter, out int dataStartRow, out int dataEndRow)
    {
        dataStartRow = Int32.MaxValue;
        dataEndRow = Int32.MinValue;
        foreach (var item in category.Invoices.SelectMany(invoice => invoice.Items).Where(item => item.PriaCategory == category))
        {
            var rowNo = rowIter.Aquire();
            dataStartRow = Math.Min(dataStartRow, rowNo);
            dataEndRow = Math.Max(dataEndRow, rowNo);
            AddItem(sheet, item, rowNo);
        }
    }

    private void AddItem(ExcelWorksheet sheet, MappedInvoice.InvoiceItem item, int row)
    {
        foreach (var (col, dataCol) in ColumnIterator())
        {
            sheet.Cells[row, col].Value = dataCol.DataParser(item);
        }
    }

    private void AddHeader(ExcelWorksheet sheet, int row)
    {
        foreach (var (col,dataCol) in ColumnIterator())
        {
            sheet.Column(col).Width = dataCol.Header.Length * 1.25;
            sheet.Cells[row, col].Value = dataCol.Header;
        }
    }

    private IEnumerable<(int, DataCol)> ColumnIterator()
    {
        for (int i = 0; i < _columns.Length; i++)
        {
            yield return (i + 1, _columns[i]);
        }
    }

    private IEnumerable<int> RowIterator()
    {
        for (int i = 1; true ; i++)
        {
            yield return i;
        }
    }

}