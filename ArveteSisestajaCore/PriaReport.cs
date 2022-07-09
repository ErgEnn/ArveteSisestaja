using System.Globalization;
using OfficeOpenXml;

namespace ArveteSisestajaCore
{
	public sealed class PriaReport : IDisposable
	{
        private readonly DateTime _beginDateTime;
        private readonly DateTime _endDateTime;
        private IDictionary<string, ExcelPackage> _files = new Dictionary<string, ExcelPackage>();
		private IDictionary<string, ExcelWorksheet> _worksheets = new Dictionary<string,ExcelWorksheet>();
		private IDictionary<string, int> _worksheetIndex = new Dictionary<string,int>();

        private readonly ExcelCol[] _columns = new[]
        {
            //new ExcelCol("Nimi arvel                ", (invoice, product) => product.Name),
            new ExcelCol("Arve kp   ", (invoice, product) => invoice.GetDate().ToString("dd.MM.yyyy")),
            new ExcelCol("Arve väljastaja", (invoice, product) => invoice.GetVendor()),
            new ExcelCol("Arve nr     ", (invoice, product) => invoice.GetInvoiceNumber()),
            new ExcelCol("Hind(ilma km.)", (invoice, product) => decimal.Parse(product.PriceBeforeVat)),
            new ExcelCol("Kogus", (invoice, product) => decimal.Parse(product.GetAdjustedAmount(), CultureInfo.GetCultureInfo("de-DE"))),
            //new ExcelCol("Kilohind", (invoice, product) => decimal.Parse(product.PriceBeforeVat) / decimal.Parse(product.GetAdjustedAmount(), CultureInfo.GetCultureInfo("de-DE")))
        };

        public PriaReport(DateTime beginDateTime, DateTime endDateTime)
        {
            _beginDateTime = beginDateTime;
            _endDateTime = endDateTime;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

		public void Dispose()
		{
			foreach (var pair in _files)
            {
                pair.Value.Save();
            }
            foreach (var pair in _files)
            {
                pair.Value.Dispose();
            }
			//Process.Start("explorer.exe", $"/select, \"{_files.First().Value.File.Directory.FullName}\"");
		}

        private ExcelPackage GetFile(string name)
        {
            if (_files.TryGetValue(name, out var file))
                return file;
            var fileName = $"pria_{name}_{_beginDateTime:ddMMMyy}_{_endDateTime:ddMMMyy}.xlsx";
            file = new ExcelPackage(new FileInfo(fileName));
			_files.Add(name,file);
            return file;
        }

		private ExcelWorksheet GetSheet(string name) {
			if (_worksheets.TryGetValue(name, out var sheet))
				return sheet;
			var newSheet = GetFile(name).Workbook.Worksheets.Add(name);
			_worksheets.Add(name, newSheet);
			AddHeader(newSheet);
			return newSheet;
		}

		private int IncrAndGetIndex(string name) {
			if (_worksheetIndex.TryGetValue(name, out var val))
				return _worksheetIndex[name] = val + 1;
			_worksheetIndex.Add(name, 2);
			return 2;
		}

		private void AddHeader(ExcelWorksheet sheet)
		{
            for (int col = 0; col < _columns.Length; col++)
			{
				sheet.Column(col + 1).Width = _columns[col].Header.Length * 1.25;
				sheet.Cells[1, col + 1].Value = _columns[col].Header;
			}
		}

		public void AddRow(string name, Invoice invoice, Product product)
		{
			if (decimal.Parse(product.GetAdjustedAmount(), CultureInfo.GetCultureInfo("de-DE")) == 0)
			{
				return;
			}
			var sheet = GetSheet(name);
			var row = IncrAndGetIndex(name);
            for (int i = 0; i < _columns.Length; i++)
            {
				sheet.Cells[row, i+1].Value = _columns[i].DataParser(invoice,product);
			}
		}

        public class ExcelCol
        {
            public string Header { get; }
            public Func<Invoice, Product, object> DataParser { get; }

            public ExcelCol(string header, Func<Invoice,Product,object> dataParser)
            {
                Header = header;
                DataParser = dataParser;
            }
        }
	}
}