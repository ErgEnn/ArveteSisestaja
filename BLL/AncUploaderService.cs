using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BLL.Entities;

namespace BLL
{
    public class AncUploaderService
    {
        private readonly string _username;
        private readonly string _password;

        static AncUploaderService()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public AncUploaderService(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public async Task UploadInvoices(IEnumerable<Invoice> invoices)
        {
            var cawc = await Login();
            foreach (var invoice in invoices)
            {
                foreach (var product in invoice.GetClassifiedProducts())
                {
                    if (product.AncClassifier is not NonProductAncClassifier)
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("invoice", invoice.Identifier);
                        nvc.Add("date", invoice.Date.ToString("dd.MM.yyyy"));
                        nvc.Add("vendor", invoice.Vendor);
                        nvc.Add("vendors_menu", "");
                        nvc.Add("ingredient", product.AncClassifier.ExternalId);
                        nvc.Add("ingredient_list", "");
                        nvc.Add("total_price", product.Price.ToString("##.###"));
                        nvc.Add("vat", "20");
                        nvc.Add("amount", product.GetAdjustedAmount().ToString("##.###"));
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
                }
            }
        }

        private async Task<CookieAwareWebClient> Login()
        {
            var cawc = new CookieAwareWebClient();
            cawc.DownloadData("https://www.anckonsult.eu");
            string response = Encoding.UTF8.GetString(cawc.UploadValues("https://www.anckonsult.eu/?class=user&do=login&method=json", new NameValueCollection() { { "uid", _username }, { "password", _password } }));
            cawc.DownloadData("https://www.anckonsult.eu");
            Console.WriteLine("ANC:" + response);
            if (!response.Contains("{\"ok\":1"))
                throw new Exception("ANC login failed");
            return cawc;
        }

    }

    class CookieAwareWebClient : WebClient
    {
        public CookieAwareWebClient()
            : this(new CookieContainer()) { }
        public CookieAwareWebClient(CookieContainer c)
        {
            CookieContainer = c;
        }
        public CookieContainer CookieContainer { get; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            var castRequest = request as HttpWebRequest;
            if (castRequest != null)
            {
                castRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                castRequest.CookieContainer = CookieContainer;
            }

            return request;
        }
    }
}
