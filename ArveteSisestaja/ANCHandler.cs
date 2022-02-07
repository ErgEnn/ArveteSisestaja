using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace ArveteSisestaja {
	public class ANCHandler {

		static CookieAwareWebClient cawc;
		public BackgroundWorker AncIngredientsLoader;
		public BackgroundWorker AncUploadedInvoicesLoader;
		public BackgroundWorker AncUploaderWorker;

		public ANCHandler() {
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			AncIngredientsLoader = new BackgroundWorker();
			AncIngredientsLoader.DoWork += new DoWorkEventHandler(LoadIngredients);
			AncUploadedInvoicesLoader = new BackgroundWorker();
			AncUploadedInvoicesLoader.DoWork += new DoWorkEventHandler(LoadInvoices);
			AncUploaderWorker = new BackgroundWorker();
			AncUploaderWorker.WorkerReportsProgress = true;
			AncUploaderWorker.DoWork += new DoWorkEventHandler(UploadInvoices);
		}

		public bool LogIn(string username,string password) {
			cawc = new CookieAwareWebClient();
			cawc.DownloadData("https://www.anckonsult.eu");
			string response = Encoding.UTF8.GetString(cawc.UploadValues("https://www.anckonsult.eu/?class=user&do=login&method=json", new NameValueCollection() { { "uid", username}, { "password", password } }));
			cawc.DownloadData("https://www.anckonsult.eu");
			Console.WriteLine("ANC:"+response);
			return response.Contains("{\"ok\":1");
		}

		public void LoadIngredients(object sender, DoWorkEventArgs args) {
			string page = cawc.DownloadString("https://www.anckonsult.eu/?class=product&do=add_new");
			Dictionary<string, Ingredient> ingredients = new Dictionary<string, Ingredient>();
			ingredients.Add("!!!ÄRA SISESTA ANCsse!!!", new Ingredient(0, "!!!ÄRA SISESTA ANCsse!!!"));
			HtmlDocument html = new HtmlDocument();
			html.LoadHtml(page);
			HtmlNodeCollection nodes = html.GetElementbyId("ingredient_list").ChildNodes;
			foreach(var node in nodes) {
				if(node.Name == "option" && Int32.Parse(node.Attributes["value"].Value) != 0) {
					try {
						int id = Int32.Parse(node.Attributes["value"].Value);
						string name = Encoding.UTF8.GetString(Encoding.Default.GetBytes(node.InnerText));
						ingredients.Add(name, new Ingredient(id,name));
					} catch (Exception e) {
						MessageBox.Show("ANC kauba nimede laadimise viga!", "ERROR");
						Console.WriteLine("ANC laadimise viga!");
					}
				}
			}
			string script = html.DocumentNode.Descendants()
				.Where(node => node.Name == "script" && node.InnerText.Contains("var pieces")).Single().InnerText;
			Regex regex = new Regex("var pieces2 = ({(?>(?>(?>\\d+):(?>\\d+\\.\\d+)),?\\W?)+)");
			var alteredIngredients = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(regex.Match(script).Groups[1].Value);
			alteredIngredients.Join(ingredients,
				alt => alt.Key,
				ingr => ingr.Value.Id,
				(alt, ingr) =>
				{
					ingr.Value.UnitCoefficient = alt.Value;
					ingr.Value.Unit = "tk";
					Console.WriteLine($"Lisainfo: {ingr.Key}");
					return ingr;
				}).ToArray();
			Console.WriteLine("Laetud " + ingredients.Count + " toote nimetust");
			args.Result = ingredients;
		}

		private void UploadInvoices(object sender, DoWorkEventArgs e) {
			List<Invoice> invoices = (List<Invoice>) e.Argument;
			AncUploaderWorker.ReportProgress(0);
			int totalProductsToUpload = 0;
			foreach (Invoice invoice in invoices) {
				if (invoice.IsValid()) {
					totalProductsToUpload += invoice.GetProducts().Count;
				}
			}
			int index = 0;
			foreach(Invoice invoice in invoices) {
				if(invoice.IsValid()) {
					List<Product> products = invoice.GetProducts();
					foreach (Product product in products) {
						if (product.Definition.AncIngredient.Id != 0)
						{
							NameValueCollection nvc = new NameValueCollection();
							nvc.Add("invoice", invoice.GetInvoiceNumber());
							nvc.Add("date", invoice.GetDate().ToString("dd.MM.yyyy"));
							nvc.Add("vendor", invoice.GetVendor());
							nvc.Add("vendors_menu", "");
							nvc.Add("ingredient", product.Definition.AncIngredient.Id.ToString());
							nvc.Add("ingredient_list", "");
							nvc.Add("total_price", product.PriceBeforeVat);
							nvc.Add("vat", "20");
							nvc.Add("amount", product.GetAdjustedAmount());
							nvc.Add("deadline", "");
							nvc.Add("category", "-");
							nvc.Add("category_list", "-");
							cawc.UploadValues(
								new Uri("https://www.anckonsult.eu/?class=anc&do=check_dates&method=json"), nvc);
							var response = Encoding.UTF8.GetString(cawc.UploadValues(
								new Uri("https://www.anckonsult.eu/?class=product&do=save&method=json"), nvc));
							if (!response.Contains("\"ok\":1"))
							{
								Console.WriteLine(response);
								Console.WriteLine(String.Join(", ", nvc.Cast<string>().Select(s => nvc[s])));
							}
						}
						index++;
						AncUploaderWorker.ReportProgress((int)(((double)index/(double)totalProductsToUpload)*100));
					}
				}
			}
		}

		/*
		public static void InsertInvoices(List<Invoice> invoices, Dictionary<string, int> ingredients, Dictionary<string, Definition> definitions, List<string> warehouse) {
			int i = 1;
			int ic = invoices.Count;
			int pc = 0;
			Stopwatch sw = new Stopwatch();
			sw.Start();
			foreach(Invoice invoice in invoices) {
				if(invoice.IsValid() && invoice.IsUploadable && !warehouse.Contains(invoice.GetInvoiceNumber())) {
					List<Product> products = invoice.GetProducts();
					int p = 1;
					pc = products.Count;
					foreach(Product product in products) {
						Console.WriteLine("Arve: " + i + "/" + ic + "    Toode: " + p + "/" + pc);
						if(ingredients[definitions[product.GetName()].anc] == 0) {
							continue;
						}
						NameValueCollection nvc = new NameValueCollection();
						nvc.Add("invoice", invoice.GetInvoiceNumber());
						nvc.Add("date", invoice.GetDate());
						nvc.Add("vendor", invoice.GetVendor().GetANCName());
						nvc.Add("vendors_menu", "");
						nvc.Add("ingredient", ingredients[definitions[product.GetName()].anc].ToString());
						nvc.Add("ingredient_list", "");
						nvc.Add("total_price", product.GetPrice());
						nvc.Add("vat", "20");
						nvc.Add("amount", definitions[product.GetName()].GetAmount(product.GetAmount()));
						nvc.Add("deadline", "");
						nvc.Add("category", "-");
						nvc.Add("category_list", "-");
						cawc.UploadValues("https://www.anckonsult.eu/?class=anc&do=check_dates&method=json", nvc);
						cawc.UploadValues("https://www.anckonsult.eu/?class=product&do=save&method=json", nvc);
						p++;
					}
				} else {
					Console.WriteLine("Arve juba sisestatud: " + invoice.GetInvoiceNumber());
				}
				i++;
			}
			sw.Stop();
			Console.WriteLine("Arved sisestatud ANCsse");
			Console.WriteLine("Aega läks: " + sw.Elapsed.TotalSeconds + " sek");
		}*/

		internal void LoadInvoices(object sender, DoWorkEventArgs args) {
			List<object> argsList= args.Argument as List<object>;
			DateTime minDateTime = (DateTime) argsList[0];
			DateTime maxDateTime = (DateTime)argsList[1];
			NameValueCollection nvc = new NameValueCollection();
			nvc.Add("search_start", minDateTime.ToString("dd.MM.yyyy"));
			nvc.Add("search_end", maxDateTime.ToString("dd.MM.yyyy"));
			nvc.Add("search_number", "");
			nvc.Add("search_vendor", "0");
			nvc.Add("class", "warehouse");
			nvc.Add("do", "index");
			string page = Encoding.UTF8.GetString(cawc.UploadValues("https://www.anckonsult.eu/?", nvc));
			List<string> warehouse = new List<string>();
			HtmlDocument html = new HtmlDocument();
			html.LoadHtml(page);
			HtmlNodeCollection nodes = html.GetElementbyId("warehouse_invoices").ChildNodes;
			if(nodes.Count > 3) {
				for(int i = 3; i < nodes.Count; i++) {
					if(nodes[i].Name == "tr") {
						warehouse.Add(nodes[i].ChildNodes[1].InnerText.ToUpper());
					}
				}
			}
			args.Result = warehouse;
		}

		private class CookieAwareWebClient : WebClient {
			public CookieAwareWebClient()
				: this(new CookieContainer()) { }
			public CookieAwareWebClient(CookieContainer c) {
				this.CookieContainer = c;
			}
			public CookieContainer CookieContainer { get; set; }

			protected override WebRequest GetWebRequest(Uri address) {
				WebRequest request = base.GetWebRequest(address);

				var castRequest = request as HttpWebRequest;
				if(castRequest != null) {
					castRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
					castRequest.CookieContainer = this.CookieContainer;
				}

				return request;
			}
		}

	}
}
