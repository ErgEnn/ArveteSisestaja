using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ArveteSisestaja {
	public class DefinitionsHandler {

		private static readonly string _filedir = "definitions.csv";
		private static Dictionary<string, Definition> _definitions;
		public static Dictionary<string, Ingredient> AncIngredients { get; set; }

		public static void LoadDefinitions(Dictionary<string,Ingredient> ancIngredients) {
			_definitions = new Dictionary<string, Definition>();
			AncIngredients = ancIngredients;

			if (File.Exists(_filedir)) {
				string[] lines = File.ReadAllLines(_filedir);
				foreach (string line in lines) {
					string[] vals = line.Split(';');// product name; ANC name; [amount multiplier]
					string productName =string.Join(";",vals.TakeWhile((_, i) => i<vals.Length-2));
					string ancName = vals[vals.Length-2];
					decimal multiplier = decimal.Parse(vals[vals.Length-1], CultureInfo.GetCultureInfo("de-DE"));
					if (ancIngredients.ContainsKey(ancName) && !_definitions.ContainsKey(productName)) {
						try {
							_definitions.Add(productName, new Definition(ancIngredients[ancName],multiplier));
						} catch (Exception e) {
							Console.WriteLine("ERROR definitsiooni laadimisel!");
						}
					}
				}
				Console.WriteLine($"Laetud {_definitions.Count} definitsiooni");
			}
		}

		public static Definition GetDefinition(string productName) {
			if (_definitions.ContainsKey(productName)) {
				return _definitions[productName];
			}
			return null;
		}

		public static Definition AddDefinition(Product product,string ancName, int multiplier) {
			if (GetDefinition(product.Name) == null)
			{
				Ingredient ingredient = AncIngredients[ancName];
				Definition newDefinition = new Definition(ingredient,multiplier);
				_definitions.Add(product.Name, newDefinition);
				File.AppendAllText(_filedir, product.Name + ";" + ancName + ";" + multiplier + "\n");
				return newDefinition;
			}
			return GetDefinition(product.Name);

		}
	}
}
