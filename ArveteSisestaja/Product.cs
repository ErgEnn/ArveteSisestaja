using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArveteSisestaja {
	public class Product {
		string _name;
		string _amount;
		string _price;
		public Definition definition { get; set; }
		public Product(string name, string amount, string price,Definition definition) {
			this._name = name;
			this._amount = amount;
			this._price = price;
			this.definition = definition;
		}

		public string GetName() {
			return this._name;
		}
		public string GetAmount() {
			return this._amount;
		}
		public string GetPrice() {
			return this._price;
		}

		private string GetFixedLengthString(string s, int lenght=15) {
			if (s.Length <= lenght) {
				return s + String.Concat(Enumerable.Repeat(" ",(lenght - s.Length)));
			}
			return s.Substring(0, lenght - 3) + "...";
		}

		public override string ToString() {
			return $"{GetFixedLengthString(_name)}    {_price} €    {_amount}    {GetFixedLengthString(definition.ToString())}";
		}

		public string GetAdjustedAmount() {
			var am = decimal.Parse(_amount,CultureInfo.InvariantCulture);
			var mu = decimal.Parse(definition.multiplier, CultureInfo.InvariantCulture);
			var mul = decimal.Multiply(am, mu);
			var div = decimal.Divide(mul, 1000);
			string s = div.ToString(CultureInfo.GetCultureInfo("de-DE"));
			return div.ToString(CultureInfo.GetCultureInfo("de-DE"));
		}
	}
}
