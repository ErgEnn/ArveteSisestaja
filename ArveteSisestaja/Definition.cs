using System;

namespace ArveteSisestaja {
	public class Definition {
		public decimal Multiplier { get; }
		public Ingredient AncIngredient { get;}

		public Definition(Ingredient ancIngredient, decimal multiplier) {
			this.Multiplier = multiplier;
			this.AncIngredient = ancIngredient;
		}

		public string GetAmount(string amount) {
			if (Multiplier == 0) {
				return amount;
			}
			var am = Decimal.Parse(amount);
			var mul = Decimal.Divide(Multiplier, 1000);
			return Decimal.Multiply(am, mul).ToString();
		}
	}
}
