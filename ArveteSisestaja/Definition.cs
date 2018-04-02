using System;

namespace ArveteSisestaja {
	public class Definition {
		public string anc { get; set; }
		public string multiplier { get; set; }
		public int ancIndex { get; set; }

		public Definition(string anc, string multiplier,int ancIndex) {
			this.anc = anc;
			this.multiplier = multiplier;
			this.ancIndex = ancIndex;
		}

		public string GetAmount(string amount) {
			if (multiplier == "") {
				return amount;
			}
			var am = Decimal.Parse(amount);
			var mul = Decimal.Divide(Decimal.Parse(multiplier), 1000);
			return Decimal.Multiply(am, mul).ToString();
		}
	}
}
