using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using EInvoice;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using Invoice = InvoiceDownloader.Invoice;
using System.Globalization;

namespace BlazorApp
{
    public class AncHandler
    {
        private readonly IOptions<AncOptions> _options;
        private readonly IDbContextFactory<DbContext> _dbContextFactory;
        private CookieAwareWebClient _cawc = new();

        public AncHandler(IOptions<AncOptions> options, IDbContextFactory<DbContext> dbContextFactory)
        {
            _options = options;
            _dbContextFactory = dbContextFactory;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private async Task LogIn()
        {
            _cawc = new CookieAwareWebClient();
            await _cawc.DownloadDataTaskAsync("https://www.anckonsult.eu");
            string response = Encoding.UTF8.GetString(await _cawc.UploadValuesTaskAsync("https://www.anckonsult.eu/?class=user&do=login&method=json", new NameValueCollection() { { "uid", _options.Value.Username }, { "password", _options.Value.Password } }));
            await _cawc.DownloadDataTaskAsync("https://www.anckonsult.eu");
            if (!response.Contains("{\"ok\":1"))
                throw new AuthenticationFailureException($"ANC Response: {response}");
        }

        private async Task<bool> IsLoggedIn()
        {
            string page = await _cawc.DownloadStringTaskAsync("https://www.anckonsult.eu/");
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(page);
            var logoutNode = html
                .GetElementbyId("login_box")
                .ChildNodes
                .FirstOrDefault(node => node.Name == "form")
                ?.ChildNodes
                .FirstOrDefault(node => node.Name == "input" && node.GetAttributeValue("value", "") == "Logi välja");
            return logoutNode != null;
        }

        public async Task<AncClassifier[]> GetAncClassifiers()
        {
            if (!await IsLoggedIn())
                await LogIn();
            string page = await _cawc.DownloadStringTaskAsync("https://www.anckonsult.eu/?class=product&do=add_new");
            var classifiers = new List<AncClassifier>();
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(page);
            HtmlNodeCollection nodes = html.GetElementbyId("ingredient_list").ChildNodes;
            foreach (var node in nodes)
            {
                if (node.Name == "option" && int.Parse(node.Attributes["value"].Value) != 0)
                {
                    int id = int.Parse(node.Attributes["value"].Value);
                    string name = Encoding.UTF8.GetString(Encoding.Default.GetBytes(node.InnerText));
                    classifiers.Add(new AncClassifier(id, name, "kg", 1));
                }
            }
            string script = html.DocumentNode.Descendants()
                .Where(node => node.Name == "script" && node.InnerText.Contains("var pieces")).Single().InnerText;
            Regex regex = new Regex("var pieces2 = ({(?>(?>(?>\\d+):(?>\\d+\\.\\d+)),?\\W?)+)");
            var alteredIngredients = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(regex.Match(script).Groups[1].Value);
            return classifiers.Select(classifier =>
            {
                if (alteredIngredients.TryGetValue(classifier.Id, out var coef))
                    return classifier with { Unit = "tk", UnitCoefficient = coef };
                return classifier;
            }).ToArray();
        }

        private class CookieAwareWebClient : WebClient
        {
            public CookieAwareWebClient()
                : this(new CookieContainer()) { }
            public CookieAwareWebClient(CookieContainer c)
            {
                this.CookieContainer = c;
            }
            public CookieContainer CookieContainer { get; set; }

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);

                var castRequest = request as HttpWebRequest;
                if (castRequest != null)
                {
                    castRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    castRequest.CookieContainer = this.CookieContainer;
                }

                return request;
            }
        }

        public async Task UploadInvoices(IEnumerable<Invoice> invoices)
        {
            if (!await IsLoggedIn())
                await LogIn();
            var classifiers = await (await _dbContextFactory.CreateDbContextAsync()).AncClassifierMappings.ToDictionaryAsync(mapping => mapping.ProductName);
            foreach (var invoice in invoices)
            {
                var reader = XmlReader.Create(invoice.XML.ToStream(), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document });
                var einvoice = new XmlSerializer(typeof(E_Invoice)).Deserialize(reader) as E_Invoice;
                foreach (var item in einvoice.Invoice[0].InvoiceItem.InvoiceItemGroup.SelectMany(group => group.ItemEntry))
                {
                    var productName = item.Description.ReplaceLineEndings("");
                    productName = new Regex("[0-3]?\\d\\.[0-1]\\d\\.20\\d\\d").Replace(productName, "");
                    var classifier = classifiers[productName];
                    if (classifier.AncClassifierId == -1)
                        continue;
                    NameValueCollection nvc = new NameValueCollection
                            {
                                { "invoice", invoice.InvoiceNo },
                                { "date", invoice.InvoiceDate.ToString("dd.MM.yyyy") },
                                { "vendor", invoice.InvoiceSender },
                                { "vendors_menu", "" },
                                { "ingredient", classifier.AncClassifierId.ToString() },
                                { "ingredient_list", "" },
                                { "total_price", item.VAT.SumBeforeVAT.ToString(CultureInfo.GetCultureInfo("de-DE")) },
                                { "vat", item.VAT.VATRate.ToString(CultureInfo.GetCultureInfo("de-DE")) },
                                { "amount", (item.ItemDetailInfo[0].ItemAmount * classifier.Multiplier / 1000)?.ToString(CultureInfo.GetCultureInfo("de-DE")) },
                                { "deadline", "" },
                                { "category", "-" },
                                { "category_list", "-" }
                            };
                    _cawc.UploadValues(
                        new Uri("https://www.anckonsult.eu/?class=anc&do=check_dates&method=json"), nvc);
                    var response = Encoding.UTF8.GetString(_cawc.UploadValues(
                        new Uri("https://www.anckonsult.eu/?class=product&do=save&method=json"), nvc));
                    if (!response.Contains("\"ok\":1"))
                    {
                        Console.WriteLine(response);
                        Console.WriteLine(String.Join(", ", nvc.Cast<string>().Select(s => nvc[s])));
                    }
                }
            }
        }

        public async Task<IReadOnlyCollection<(string invoiceNo, DateOnly date, string vendor)>> GetInvoicesInANC(
            DateOnly start, DateOnly end)
        {
            if (!await IsLoggedIn())
                await LogIn();
            NameValueCollection nvc = new NameValueCollection
            {
                { "search_start", start.ToString("dd.MM.yyyy") },
                { "search_end", end.ToString("dd.MM.yyyy") },
                { "search_number", "" },
                { "search_vendor", "0" },
                { "class", "warehouse" },
                { "do", "index" }
            };
            string page = Encoding.UTF8.GetString(await _cawc.UploadValuesTaskAsync("https://www.anckonsult.eu/#warehouse", nvc));
            var ancInvoices = new List<(string,DateOnly, string)>();
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(page);
            HtmlNodeCollection nodes = html.GetElementbyId("warehouse_invoices").ChildNodes;
            if (nodes.Count > 3)
            {
                for (int i = 3; i < nodes.Count; i++)
                {
                    if (nodes[i].Name == "tr")
                    {
                        var invoiceNo = nodes[i].ChildNodes[1].InnerText.ToUpper();
                        var invoiceDate = DateOnly.ParseExact(nodes[i].ChildNodes[3].InnerText, "yyyy-MM-dd");
                        var vendor = nodes[i].ChildNodes[5].InnerText;
                        ancInvoices.Add((invoiceNo,invoiceDate,vendor));
                    }
                }
            }

            return ancInvoices;
        }
    }
}
