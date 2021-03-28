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
		private ExcelWorksheet _milkSheet;
		private int _milkRow = 3;
		private ExcelWorksheet _berrySheet;
		private int _berryRow = 3;
		public PriaReport()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_excelFile = new ExcelPackage(new FileInfo($"pria_{DateTime.Today:yy-MM-dd}.xlsx"));
			_milkSheet = _excelFile.Workbook.Worksheets.Add("Piim");
			AddHeader(_milkSheet, "Nimi arvel                ", "Arve väljastaja", "Arve kp   ", "Arve nr     ", "Kogus", "Hind(ilma km.)", "Kilohind", "Summa selle reani");
			_berrySheet = _excelFile.Workbook.Worksheets.Add("Marjad");
			AddHeader(_berrySheet, "Nimi arvel                ", "Arve väljastaja", "Arve kp   ", "Arve nr     ", "Kogus", "Hind(ilma km.)", "Kilohind", "Summa selle reani");
		}

		public void Dispose()
		{
			_excelFile.Save();
			Process.Start("explorer.exe", "/select, \"" + $"pria_{DateTime.Today:yy-MM-dd}.xlsx" + "\"");
			_milkSheet?.Dispose();
			_berrySheet?.Dispose();
			_excelFile?.Dispose();
		}

		private void AddHeader(ExcelWorksheet sheet, params string[] columns)
		{
			for (int col = 0; col < columns.Length; col++)
			{
				sheet.Column(col + 1).Width = columns[col].Length * 1.25;
				sheet.Cells[2, col + 1].Value = columns[col];
			}
		}

		public void AddMilk(Invoice invoice, Product product)
		{
			AddRow(_milkSheet,ref _milkRow, invoice, product);
		}

		public void AddBerry(Invoice invoice, Product product)
		{
			AddRow(_berrySheet,ref _berryRow, invoice, product);
		}

		private void AddRow(ExcelWorksheet sheet,ref int row, Invoice invoice, Product product)
		{
			sheet.Cells[row, 1].Value = product.Name;
			sheet.Cells[row, 2].Value = invoice.GetVendor();
			sheet.Cells[row, 3].Value = invoice.GetDate().ToString("dd.MM.yyyy");
			sheet.Cells[row, 4].Value = invoice.GetInvoiceNumber();
			sheet.Cells[row, 5].Value = decimal.Parse(product.GetAdjustedAmount(), CultureInfo.GetCultureInfo("de-DE"));
			sheet.Cells[row, 6].Value = decimal.Parse(product.PriceBeforeVat);
			sheet.Cells[row, 7].Value = decimal.Parse(product.PriceBeforeVat) / decimal.Parse(product.GetAdjustedAmount(), CultureInfo.GetCultureInfo("de-DE"));
			sheet.Cells[row, 8].Formula = $"=SUM(F3:F{row})";
			row++;
		}
	}
}