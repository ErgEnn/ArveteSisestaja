using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace ArveteSisestaja {
	public class Invoice {
		private string _xmlText;
		private Screenshot screenshot;
		private bool _isValid;
		private XmlDocument _xml;
		private DateTime _date = DateTime.MinValue;
		private string _vendor = "VIGANE";
		private string _invoiceNumber = "VIGANE";
		private List<Product> _products = new List<Product>();
		private bool _isAlreadyUploaded = false;
		private bool _isUploadable = false;
		private bool _toBeUploaded = true;
		private int _invalidProductsAmount = -1;

		public Invoice(string xmlText) {
			this._xmlText = xmlText;
			_xml = new XmlDocument();
			_xml.LoadXml(_xmlText);

			_date = DateTime.Parse(_xml.SelectSingleNode("/E_Invoice/Invoice/InvoiceInformation/InvoiceDate").InnerText);
			_vendor = _xml.SelectSingleNode("/E_Invoice/Invoice/InvoiceParties/SellerParty/Name").InnerText;
			_invoiceNumber = _xml.SelectSingleNode("/E_Invoice/Invoice/InvoiceInformation/InvoiceNumber").InnerText;
		}

		public List<Product> ParseProducts() {
			_products.Clear();
			List<Product> invalidProducts = new List<Product>();
			try {
				XmlNodeList nodeList = _xml.SelectNodes("/E_Invoice/Invoice/InvoiceItem/InvoiceItemGroup/ItemEntry");
				foreach (XmlNode node in nodeList) {
					string name = node["Description"].InnerText;
					string amount = node["ItemDetailInfo"]["ItemAmount"].InnerText;
					string price = node["VAT"]["SumBeforeVAT"].InnerText;
					Definition definition = DefinitionsHandler.GetDefinition(name);
					Product product = new Product(name, amount, price, definition);
					if (definition == null) {
						invalidProducts.Add(product);
					}
					_products.Add(product);
				}
				_invalidProductsAmount = invalidProducts.Count;
				_isUploadable = invalidProducts.Count == 0;
				_isValid = _isUploadable && !_isAlreadyUploaded && _toBeUploaded;
			} catch (Exception) {
				_isValid = false;
				_isUploadable = false;
			}
			return invalidProducts;
		}

		public Invoice(Screenshot screenshot) {
			this.screenshot = screenshot;
			screenshot.SaveAsFile($"VIGASED/ERROR_{DateTime.Now:yyyy_MM_dd_HH_mm_ssff}.png");
			this._isValid = false;
			this._toBeUploaded = false;
		}

		public bool IsValid() {
			return _isValid;
		}

		public DateTime GetDate() {
			return _date;
		}

		public string GetVendor() {
			return _vendor;
		}

		public string GetInvoiceNumber() {
			return _invoiceNumber;
		}

		public List<Product> GetProducts() {
			return _products;
		}

		public void SetAlreadyUploaded(bool b) {
			this._isAlreadyUploaded = b;
		}

		public void SetToBeUploaded(bool b) {
			this._toBeUploaded = b;
		}

		public override string ToString() {
			return $"ASUTUS: {GetVendor()}    ARVE NR: {GetInvoiceNumber()}    KUUPÄEV: {GetDate():yyyy-M-d}    TOOTEID: {_products.Count}";
		}

		public string GetStatus() {
			if (_isAlreadyUploaded) {
				return "JUBA ANCs";
			}
			if (!_isUploadable) {
				return $"TEADMATA({_invalidProductsAmount}/{_products.Count})";
			}
			if(!_isValid) {
				return "LUGEMATU";
			}
			return "OK";
		}

		public Color GetColor() {
			if(_vendor == "VIGANE") {
				return Color.DarkRed;
			}
			if(_isAlreadyUploaded) {
				return Color.LightSkyBlue;
			}
			if(!_isUploadable) {
				return Color.Yellow;
			}
			return Color.LightGreen;
		}
	}
}
