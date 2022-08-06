using System.Globalization;
using DAL;
using Domain;

namespace ArveteSisestajaCore {
	public class DefinitionsHandler {
        public static Dictionary<string, Ingredient> AncIngredients { get; set; }

		public static void LoadDefinitions(Dictionary<string,Ingredient> ancIngredients) {
            AncIngredients = ancIngredients;
            var cache = AppDbContext.Instance.AncIngredients.ToList();
            var actualData = ancIngredients.Select(pair => new AncIngredient()
                {AncId = pair.Value.Id, Name = pair.Value.Name, UnitName = pair.Value.Unit}).ToList();

			foreach (var ancIngredient in actualData)
            {
                var cachedIngredient = cache.SingleOrDefault(ci => ci.Name == ancIngredient.Name);
                if (cachedIngredient != null)
                {
                    AppDbContext.Instance.AncIngredients.Add(ancIngredient);
                }
                else
                {
                    if (cachedIngredient.UnitName != ancIngredient.UnitName)
                        AppDbContext.Instance.AncIngredients.Update(ancIngredient);
                }
            }

            AppDbContext.Instance.SaveChanges();
        }

		public static Definition? GetDefinition(string productName)
        {
            productName = productName.Replace(Environment.NewLine, "").ToLowerInvariant();
			if (AppDbContext.Instance.ItemAncRelations.SingleOrDefault(classifier => classifier.Name == productName) is {AncIngredientName:{}} classifier) {
				return new Definition(AncIngredients[classifier.AncIngredientName],classifier.Coefficient.Value);
			}
			return null;
		}

		public static Definition AddDefinition(Product product,string ancName, decimal multiplier)
        {
            var productName = product.Name.Replace(Environment.NewLine, "").ToLowerInvariant();

            if (GetDefinition(productName) is { } definition)
                return definition;
            var ingredient = AncIngredients[ancName];
            var newDefinition = new ItemAncClassifier()
                {AncIngredientName = ingredient.Name, Name = productName, Coefficient = multiplier};
            AppDbContext.Instance.ItemAncRelations.Update(newDefinition);
            AppDbContext.Instance.SaveChanges();
            
            return new Definition(ingredient,multiplier);
        }
	}
}
