using System.ComponentModel;
using System.Diagnostics;
using BLL;
using DAL;

namespace ArveteSisestajaCore;

public partial class MainForm : Form
{
    private readonly OmnivaHandler _omnivaHandler;
    private readonly ANCHandler _ancHandler;
    private readonly InvoiceService _invoiceService;
    private readonly AncClassifierService _ancClassifierService;
    private readonly AncUploaderService _ancUploaderService;

    public MainForm(OmnivaHandler omnivaHandler, ANCHandler ancHandler, InvoiceService invoiceService)
    {
        _omnivaHandler = omnivaHandler;
        _ancHandler = ancHandler;
        _invoiceService = invoiceService;
        _ancClassifierService = new AncClassifierService();
        _ancUploaderService = new AncUploaderService(
            SettingsHandler.GetSetting(SettingsHandler.SETTING.ANC_USERNAME),
            SettingsHandler.GetSetting(SettingsHandler.SETTING.ANC_PASSWORD));
        InitializeComponent();
    }


    private void mainForm_Load(object sender, EventArgs e)
    {
        beginDateTimePicker.Value = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1);
        endDateTimePicker.Value = beginDateTimePicker.Value.AddMonths(1).AddDays(-1);
        
        if (Directory.Exists("VIGASED")) Directory.Delete("VIGASED", true);
        Directory.CreateDirectory("VIGASED");
    }

    private void loadInvoicesBtn_Click(object sender, EventArgs e)
    {
        beginDateTimePicker.Enabled = false;
        endDateTimePicker.Enabled = false;
        loadInvoicesBtn.Enabled = false;

        _omnivaHandler
            .LoadInvoices(beginDateTimePicker.Value, endDateTimePicker.Value)
            .ContinueWith(invoicesTask =>
            {
                _invoiceService.AddInvoiceRange(invoicesTask.Result);
                _ancClassifierService.ClassifyInvoices(_invoiceService.GetAllInvoices());
            }).ConfigureAwait(true);
        
    }

    private void AncUploaderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        _ancHandler.AncUploadedInvoicesLoader.RunWorkerCompleted += (o, args) => RefreshGrid();
        _ancHandler.AncUploadedInvoicesLoader.RunWorkerAsync(new List<object>
            {beginDateTimePicker.Value, endDateTimePicker.Value});
        MessageBox.Show("Arved sisestatud");
        Process.Start("VIGASED");
    }

    private void AncIngredientsLoaderOnRunWorkerCompleted(object sender,
        RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
    {
        DefinitionsHandler.LoadDefinitions((Dictionary<string, Ingredient>) runWorkerCompletedEventArgs.Result);
        _ancHandler.AncUploadedInvoicesLoader.RunWorkerAsync(new List<object>
            {beginDateTimePicker.Value, endDateTimePicker.Value});
    }

    private void RefreshGrid()
    {
        invoiceDataGrid.Rows.Clear();
        foreach (var invoice in _invoiceService.GetAllInvoices())
        {
            var row = new DataGridViewRow();
            row.CreateCells(invoiceDataGrid);
            row.Cells[0].Value = true;
            if (invoice.ExistsInAnc) row.Cells[0].Value = false;
            row.Cells[0].ReadOnly = false;
            row.Cells[1].Value = invoice.Vendor;
            row.Cells[2].Value = invoice.Identifier;
            row.Cells[3].Value = invoice.Date.ToString("dd.MM.yyyy");
            row.Cells[4].Value = "mingi staatus siia";
            row.DefaultCellStyle.BackColor = Color.BlueViolet;
            invoiceDataGrid.Rows.Add(row);
        }

        uploadInvoices.Enabled = true;
    }


    private void uploadInvoices_Click(object sender, EventArgs e)
    {
        _ancUploaderService.UploadInvoices(_invoiceService.GetAllInvoices()).ContinueWith(task => MessageBox.Show("Upload complete"));
    }

    private void AncUploaderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        mainProgressBar.Value = e.ProgressPercentage;
    }

    private void DateTimePicker_ValueChanged(object sender, EventArgs e)
    {
        if (beginDateTimePicker.Value > endDateTimePicker.Value)
            loadInvoicesBtn.Enabled = false;
        else
            loadInvoicesBtn.Enabled = true;
    }

    private void invoiceDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
#if false
			if (invoices != null) {
				invoices.Where(invoice => invoice.GetInvoiceNumber() == invoiceDataGrid.Rows[e.RowIndex].Cells[2].Value.ToString()).First().SetToBeUploaded((bool) invoiceDataGrid.Rows[e.RowIndex].Cells[0].Value);
			}
#endif
    }

    private void generatePriaReport_Click(object sender, EventArgs e)
    {
#if false
			using (var generator = new PriaReport(beginDateTimePicker.Value,endDateTimePicker.Value))
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
							generator.AddRow("piim",invoice, product);
						if (product.Definition.AncIngredient.Name.Contains("Keefir"))
							generator.AddRow("keefir",invoice, product);
                        if (product.Definition.AncIngredient.Name == "Jogurt 2,2% (maitsestamata)")
                            generator.AddRow("maits_jogurt", invoice, product);
						if (product.Definition.AncIngredient.Name == "Õun")
                            generator.AddRow("oun",invoice, product);
						if (product.Definition.AncIngredient.Name == "Pirn")
							generator.AddRow("pirn", invoice, product);
						if (product.Definition.AncIngredient.Name == "Marjad (külmutatud)")
							generator.AddRow("Marjad",invoice, product);
					}
				}
			}
#endif
    }

    private void showPricesBtn_Click(object sender, EventArgs e)
    {
#if false
			_ancHandler.AncIngredientsLoader.RunWorkerAsync();
			List<String> allInvalidProducts = new List<String>();
			foreach (DataGridViewRow row in invoiceDataGrid.Rows)
			{
				if ((bool)row.Cells[0].Value)
				{
					Invoice invoice =
 invoices.Where(i => i.GetInvoiceNumber() == row.Cells[2].Value.ToString()).First();
					List<Product> invalidProducts = invoice.ParseProducts();
					if (invalidProducts.Count > 0)
					{
						foreach (Product invalidProduct in invalidProducts)
						{
							if (!allInvalidProducts.Contains(invalidProduct.Name))
							{
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
			foreach (DataGridViewRow row in invoiceDataGrid.Rows)
			{
                Invoice invoice = invoices.Where(i => i.GetInvoiceNumber() == row.Cells[2].Value.ToString()).First();
				List<Product> invalidProducts = invoice.ParseProducts();
				if (invalidProducts.Count > 0)
				{
					totalInvalidProducts += invalidProducts.Count;
					foreach (Product invalidProduct in invalidProducts)
					{
						dialogResult = new DefinitionsForm(invalidProduct).ShowDialog();
						if (dialogResult == DialogResult.Abort)
						{
							break;
						}

						if (dialogResult == DialogResult.Ignore)
						{
							Console.WriteLine($"[SKIPPED]: {invalidProduct.Name} | Kogus arvel: {invalidProduct.Amount}");
						}
					}
				}
				else
				{
					toBeUploadedInvoices.Add(invoice);
				}
				if (dialogResult == DialogResult.Abort)
				{
					break;
				}
			}
			RefreshGrid();
			if (totalInvalidProducts == 0)
            {
                foreach (var tuple in toBeUploadedInvoices
                             .SelectMany(invoice => invoice.GetProducts())
                             .GroupBy(product => product.Definition.AncIngredient.Name,
                                 product =>
                                 {
                                     var price = Util.ToDecimal(product.PriceBeforeVat);
                                     var adjAmount = product.GetAdjustedAmount();
                                     var amount = Util.ToDecimal(adjAmount);
                                     if (price == 0 || amount == 0)
                                         return Decimal.Zero;
                                     if (product.Definition.AncIngredient.Name == "Banaan")
                                     {
                                         return price / amount;
                                     }
									 return price/amount;
                                 },
                                 (s, prices) => new Tuple<string,decimal>(s, prices.Where(val => val > 0).DefaultIfEmpty(Decimal.Zero).Average()))
                             .OrderBy(tuple => tuple.Item1))
                {
                    Console.WriteLine($"{tuple.Item1}: {tuple.Item2} €");
                }
            }
#endif
    }
}