using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BLL.Entities
{
    public class Invoice
    {
        public string Vendor { get; init; }
        public string Identifier { get; init; }
        public DateOnly Date { get; init; }

        public bool ExistsInAnc { get; private set; }

        private IList<Product> _products = new List<Product>();

        public static Invoice FromEInvoiceXML(string xmlStr)
        {
            var xml = new XmlDocument();
            xml.LoadXml(xmlStr);
            var numberFormat = new NumberFormatInfo() {NumberDecimalSeparator = "."};
            var date = DateOnly.ParseExact(xml.SelectSingleNode("/E_Invoice/Invoice/InvoiceInformation/InvoiceDate").InnerText, "yyyy-MM-dd");
            var vendor = xml.SelectSingleNode("/E_Invoice/Invoice/InvoiceParties/SellerParty/Name").InnerText;
            var identifier = xml.SelectSingleNode("/E_Invoice/Invoice/InvoiceInformation/InvoiceNumber").InnerText;
            var products = xml.SelectNodes("/E_Invoice/Invoice/InvoiceItem/InvoiceItemGroup/ItemEntry")
                .Cast<XmlNode>()
                .Select(node => new Product()
                {
                    Amount = decimal.Parse(node["ItemDetailInfo"]["ItemAmount"].InnerText,numberFormat),
                    Price = decimal.Parse(node["VAT"]["SumBeforeVAT"].InnerText, numberFormat),
                    Name = node["Description"].InnerText,
                })
                .ToList();

            return new Invoice()
            {
                Vendor = vendor,
                Identifier = identifier,
                Date = date,
                _products = products
            };
        }

        public static Invoice FromRawData(string vendor, string identifier, DateOnly date, params object[] additionalData)
        {
            return new Invoice()
            {
                Vendor = vendor,
                Identifier = identifier,
                Date = date,
            };
        }

        public IEnumerable<Product> GetUnclassifiedProducts()
        {
            return _products.Where(product => !product.HasClassifier());
        }

        public bool HasUnclassifiedProducts()
        {
            return _products.Any(product => !product.HasClassifier());
        }

        public IEnumerable<Product> GetClassifiedProducts()
        {
            return _products.Where(product => product.HasClassifier());
        }
    }
}
