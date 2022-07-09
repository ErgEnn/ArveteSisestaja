using System.Globalization;

namespace ArveteSisestajaCore {
	public class Product {
		public string Name { get; }
		public string Amount { get; }
		public string PriceBeforeVat { get; }
		public Definition Definition { get; set; }
		public Product(string name, string amount, string priceBeforeVat,Definition definition) {
			Name = name;
			Amount = amount;
			PriceBeforeVat = priceBeforeVat;
			Definition = definition;
		}

		private string GetFixedLengthString(string s, int lenght=15) {
			if (s.Length <= lenght) {
				return s + String.Concat(Enumerable.Repeat(" ",(lenght - s.Length)));
			}
			return s.Substring(0, lenght - 3) + "...";
		}

		public override string ToString() {
			return $"{GetFixedLengthString(Name)}    {PriceBeforeVat} €    {Amount}    {GetFixedLengthString(Definition.ToString())}";
		}

		public string GetAdjustedAmount() {
			var am = decimal.Parse(Amount,CultureInfo.InvariantCulture);
			var mu = Definition.Multiplier;
			var mul = decimal.Multiply(am, mu);
			var div = decimal.Divide(mul, 1000);
			string s = div.ToString(CultureInfo.GetCultureInfo("de-DE"));
			return div.ToString(CultureInfo.GetCultureInfo("de-DE"));
		}
	}
}
