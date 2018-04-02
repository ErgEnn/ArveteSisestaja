using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ArveteSisestaja {
	public class DefinitionsHandler {

		private static readonly string _filedir = "definitions.csv";
		private static Dictionary<string, Definition> _definitions;
		private static Dictionary<string, int> _ancIngredients;

		public static void LoadDefinitions(Dictionary<string,int> ancIngredients) {
			_definitions = new Dictionary<string, Definition>();
			_ancIngredients = ancIngredients;

			if (File.Exists(_filedir)) {
				string[] lines = File.ReadAllLines(_filedir);
				foreach (string line in lines) {
					string[] vals = line.Split(';');// product name; ANC name; [amount multiplier]
					if (ancIngredients.ContainsKey(vals[1]) && !_definitions.ContainsKey(vals[0])) {
						try {
							_definitions.Add(vals[0], new Definition(vals[1], vals[2], ancIngredients[vals[1]]));
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

		public static Definition AddDefinition(Product product, string ancName, string multiplier) {
			if (GetDefinition(product.GetName()) == null) { 
				Definition newDefinition = new Definition(ancName, multiplier, _ancIngredients[ancName]);
				_definitions.Add(product.GetName(), newDefinition);
				File.AppendAllText(_filedir, product.GetName() + ";" + ancName + ";" + multiplier + "\n");
				return newDefinition;
			}
			return GetDefinition(product.GetName());

		}


		public static Dictionary<string, int> GetANCIngredients() {
			return _ancIngredients;
		}
	}
}
