using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace BlazorApp
{
    public class AncHandler
    {
        private readonly IOptions<AncOptions> _options;
        private CookieAwareWebClient _cawc = new ();

        public AncHandler(IOptions<AncOptions> options)
        {
            _options = options;
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
                .FirstOrDefault(node => node.Name == "input" && node.GetAttributeValue("value","")=="Logi välja");
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
                    classifiers.Add(new AncClassifier(id, name,"kg", 1));
                }
            }
            string script = html.DocumentNode.Descendants()
                .Where(node => node.Name == "script" && node.InnerText.Contains("var pieces")).Single().InnerText;
            Regex regex = new Regex("var pieces2 = ({(?>(?>(?>\\d+):(?>\\d+\\.\\d+)),?\\W?)+)");
            var alteredIngredients = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(regex.Match(script).Groups[1].Value);
            return classifiers.Select(classifier =>
            {
                if (alteredIngredients.TryGetValue(classifier.Id, out var coef))
                    return classifier with {Unit = "tk", UnitCoefficient = coef};
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

    }
}
