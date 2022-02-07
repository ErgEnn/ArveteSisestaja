using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace ArveteSisestaja
{
	public sealed class PriaReport : IDisposable
	{
		private ExcelPackage _excelFile;
		private IDictionary<string, ExcelWorksheet> _worksheets = new Dictionary<string,ExcelWorksheet>();
		private IDictionary<string, int> _worksheetIndex = new Dictionary<string,int>();
		private ExcelWorksheet _milkSheet;
		private int _milkRow = 2;
		private ExcelWorksheet _berrySheet;
		private int _berryRow = 2;
		public PriaReport()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_excelFile = new ExcelPackage(new FileInfo($"pria_{DateTime.Today:yy-MM-dd-HHmm}.xlsx"));
		}

		public void Dispose()
		{
			_excelFile.Save();
			Process.Start("explorer.exe", "/select, \"" + $"pria_{DateTime.Today:yy-MM-dd}.xlsx" + "\"");
			_milkSheet?.Dispose();
			_berrySheet?.Dispose();
			_excelFile?.Dispose();
		}

		private ExcelWorksheet GetSheet(string name) {
			if (_worksheets.TryGetValue(name, out var sheet))
				return sheet;
			var newSheet = _excelFile.Workbook.Worksheets.Add(name);
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
			string[] columns = new string[]
			{
				"Nimi arvel                ",
				"Arve väljastaja",
				"Arve kp   ",
				"Arve nr     ",
				"Kogus", "Hind(ilma km.)",
				"Kilohind"
			};
			for (int col = 0; col < columns.Length; col++)
			{
				sheet.Column(col + 1).Width = columns[col].Length * 1.25;
				sheet.Cells[1, col + 1].Value = columns[col];
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
			sheet.Cells[row, 1].Value = product.Name;
			sheet.Cells[row, 2].Value = invoice.GetVendor();
			sheet.Cells[row, 3].Value = invoice.GetDate().ToString("dd.MM.yyyy");
			sheet.Cells[row, 4].Value = invoice.GetInvoiceNumber();
			sheet.Cells[row, 5].Value = decimal.Parse(product.GetAdjustedAmount(), CultureInfo.GetCultureInfo("de-DE"));
			sheet.Cells[row, 6].Value = decimal.Parse(product.PriceBeforeVat);
			sheet.Cells[row, 7].Value = decimal.Parse(product.PriceBeforeVat) / decimal.Parse(product.GetAdjustedAmount(), CultureInfo.GetCultureInfo("de-DE"));
		}
	}
}