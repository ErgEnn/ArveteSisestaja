using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArveteSisestaja {
	public partial class mainForm : Form {
		private ANCHandler _ancHandler;
		private List<string> _uploadedInvoices;
		private List<Invoice> invoices;
		private Stopwatch omnivaStopwatch;
		private Stopwatch ancStopwatch;

		public mainForm() {
			InitializeComponent();
		}


		private void mainForm_Load(object sender, EventArgs e) {
			beginDateTimePicker.Value = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1);
			endDateTimePicker.Value = beginDateTimePicker.Value.AddMonths(1).AddDays(-1);
			SettingsHandler.LoadSettings();
			if (Directory.Exists("VIGASED")) {
				Directory.Delete("VIGASED",true);
			}
			Directory.CreateDirectory("VIGASED");
		}

		private void loadInvoicesBtn_Click(object sender, EventArgs e) {
			beginDateTimePicker.Enabled = false;
			endDateTimePicker.Enabled = false;
			loadInvoicesBtn.Enabled = false;
			OmnivaHandler omniva = new OmnivaHandler(beginDateTimePicker.Value, endDateTimePicker.Value);
			omniva.OmnivaWorker.ProgressChanged += OmnivaWorkerOnProgressChanged;
			omniva.OmnivaWorker.RunWorkerCompleted += OmnivaWorkerOnRunWorkerCompleted;
			omnivaStopwatch = new Stopwatch();
			omnivaStopwatch.Start();
			omniva.OmnivaWorker.RunWorkerAsync();

			ancStopwatch = new Stopwatch();
			_ancHandler = new ANCHandler();
			_ancHandler.LogIn(SettingsHandler.GetSetting(SettingsHandler.SETTING.ANC_USERNAME), SettingsHandler.GetSetting(SettingsHandler.SETTING.ANC_PASSWORD));
			_ancHandler.AncIngredientsLoader.RunWorkerCompleted += AncIngredientsLoaderOnRunWorkerCompleted;
			_ancHandler.AncIngredientsLoader.RunWorkerAsync();
			_ancHandler.AncUploadedInvoicesLoader.RunWorkerCompleted += AncUploadedInvoicesLoaderOnRunWorkerCompleted;
			_ancHandler.AncUploaderWorker.ProgressChanged += AncUploaderWorker_ProgressChanged;
			_ancHandler.AncUploaderWorker.RunWorkerCompleted += AncUploaderWorker_RunWorkerCompleted;


		}

		private void AncUploaderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			ancStopwatch.Stop();
			_ancHandler.AncUploadedInvoicesLoader.RunWorkerCompleted += (o, args) => populateGrid();
			_ancHandler.AncUploadedInvoicesLoader.RunWorkerAsync(new List<object> { beginDateTimePicker.Value, endDateTimePicker.Value });
			MessageBox.Show("Arved sisestatud. Aega läks " + (ancStopwatch.Elapsed.TotalSeconds).ToString("###,#") + " s");
			Process.Start("VIGASED");
		}

		private void AncUploadedInvoicesLoaderOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs) {
			_uploadedInvoices = (List<string>) runWorkerCompletedEventArgs.Result;
		}

		private void AncIngredientsLoaderOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs) {
			DefinitionsHandler.LoadDefinitions((Dictionary<string, Ingredient>) runWorkerCompletedEventArgs.Result);
			_ancHandler.AncUploadedInvoicesLoader.RunWorkerAsync(new List<object> { beginDateTimePicker.Value, endDateTimePicker.Value });
		}

		private void OmnivaWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs) {
			Console.WriteLine("Progress:"+progressChangedEventArgs.ProgressPercentage);
			if (progressChangedEventArgs.ProgressPercentage < 0) {
				mainProgressBar.Value = 100;
				mainProgressBar.MarqueeAnimationSpeed = 50;
				mainProgressBar.Style = ProgressBarStyle.Marquee;
			} else {
				mainProgressBar.Style = ProgressBarStyle.Continuous;
				mainProgressBar.MarqueeAnimationSpeed = 100;
				mainProgressBar.Value = progressChangedEventArgs.ProgressPercentage;
			}
		}

		private void OmnivaWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs) {
			invoices = (List<Invoice>) runWorkerCompletedEventArgs.Result;
			omnivaStopwatch.Stop();
			populateGrid();
			MessageBox.Show("Arved laetud. Aega läks " + (omnivaStopwatch.Elapsed.TotalSeconds).ToString("###,#") + " s");
	}

		private void populateGrid() {
			invoiceDataGrid.Rows.Clear();
			foreach(Invoice invoice in invoices) {
				invoice.ParseProducts();
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(invoiceDataGrid);
				row.Cells[0].Value = true;
				if (_uploadedInvoices.Contains(invoice.GetInvoiceNumber())) {
					invoice.SetAlreadyUploaded(true);
					invoice.SetToBeUploaded(false);
					row.Cells[0].Value = false;
				}
				row.Cells[0].ReadOnly = false;
				row.Cells[1].Value = invoice.GetVendor();
				row.Cells[2].Value = invoice.GetInvoiceNumber();
				row.Cells[3].Value = invoice.GetDate().ToString("dd.MM.yyyy");
				row.Cells[4].Value = invoice.GetStatus();
				row.DefaultCellStyle.BackColor = invoice.GetColor();
				invoiceDataGrid.Rows.Add(row);
			}
			uploadInvoices.Enabled = true;
		}

		private void uploadInvoices_Click(object sender, EventArgs e) {
			_ancHandler.AncIngredientsLoader.RunWorkerAsync();
			List<String> allInvalidProducts = new List<String>();
			foreach (DataGridViewRow row in invoiceDataGrid.Rows) {
				if ((bool) row.Cells[0].Value) {
					Invoice invoice = invoices.Where(i => i.GetInvoiceNumber() == row.Cells[2].Value.ToString()).First();
					List<Product> invalidProducts = invoice.ParseProducts();
					if (invalidProducts.Count > 0) {
						foreach (Product invalidProduct in invalidProducts) {
							if (!allInvalidProducts.Contains(invalidProduct.Name)) {
								allInvalidProducts.Add(invalidProduct.Name);
							}
						}
					}
				}
			}
			foreach (string invalidProduct in allInvalidProducts)
            {
                Console.WriteLine(invalidProduct);
            }
			MessageBox.Show("Tundmatuid tooteid: " + allInvalidProducts.Count);


			DialogResult dialogResult = DialogResult.OK;
			int totalInvalidProducts = 0;
			List<Invoice> toBeUploadedInvoices = new List<Invoice>();
			foreach (DataGridViewRow row in invoiceDataGrid.Rows) {
				if ((bool) row.Cells[0].Value) {
					Invoice invoice = invoices.Where(i => i.GetInvoiceNumber() == row.Cells[2].Value.ToString()).First();
					List<Product> invalidProducts = invoice.ParseProducts();
					if (invalidProducts.Count > 0) {
						totalInvalidProducts += invalidProducts.Count;
						foreach (Product invalidProduct in invalidProducts) {
							dialogResult = new DefinitionsForm(invalidProduct).ShowDialog();
							if (dialogResult == DialogResult.Abort) {
								break;
							}

							if (dialogResult == DialogResult.Ignore)
							{
								Console.WriteLine($"[SKIPPED]: {invalidProduct.Name} | Kogus arvel: {invalidProduct.Amount}");
							}
						}
					} else {
						toBeUploadedInvoices.Add(invoice);
					}
				}
				if (dialogResult == DialogResult.Abort)
				{
					break;
				}
			}
			populateGrid();
			if (totalInvalidProducts == 0) {
				ancStopwatch.Start();
				_ancHandler.AncUploaderWorker.RunWorkerAsync(toBeUploadedInvoices);
			}
		}

		private void AncUploaderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			mainProgressBar.Value = e.ProgressPercentage;
		}

		private void DateTimePicker_ValueChanged(object sender, EventArgs e) {
			if (beginDateTimePicker.Value > endDateTimePicker.Value) {
				loadInvoicesBtn.Enabled = false;
			} else {
				loadInvoicesBtn.Enabled = true;
			}
		}

		private void invoiceDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (invoices != null) {
				invoices.Where(invoice => invoice.GetInvoiceNumber() == invoiceDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString()).First().SetToBeUploaded((bool) invoiceDataGrid.Rows[e.RowIndex].Cells[0].Value);
			}
		}

		private void generatePriaReport_Click(object sender, EventArgs e) {
			using (var generator = new PriaReport())
			{
				foreach (var invoice in invoices.OrderBy(invoice => invoice.GetDate()))
				{
					foreach (var product in invoice.GetProducts())
					{
						if (product.Definition is null)
						{
							var dialogResult = new DefinitionsForm(product).ShowDialog();
							if (dialogResult == DialogResult.Abort)
							{
								break;
							}

							if (dialogResult == DialogResult.Ignore)
							{
								Console.WriteLine($"[SKIPPED]: {product.Name} | Kogus arvel: {product.Amount}");
								continue;
							}
						}
						if(product.Definition.AncIngredient.Name.Contains("Piim 3"))
							generator.AddRow("Piim",invoice, product);
						if (product.Definition.AncIngredient.Name.Contains("Keefir"))
							generator.AddRow("Keefir",invoice, product);
						if (product.Definition.AncIngredient.Name == "Õun")
							generator.AddRow("Õun",invoice, product);
						if (product.Definition.AncIngredient.Name == "Pirn")
							generator.AddRow("Pirn", invoice, product);
						if (product.Definition.AncIngredient.Name == "Nuikapsas")
							generator.AddRow("Nuikapsas",invoice, product);
						if (product.Definition.AncIngredient.Name == "Marjad (külmutatud)")
							generator.AddRow("Marjad",invoice, product);
					}
				}
			}
		}
	}
}
