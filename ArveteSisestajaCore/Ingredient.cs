namespace ArveteSisestajaCore
{
	public class Ingredient
	{
		public Ingredient(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public int Id { get; }
		public string Name { get; }

		public string Unit { get; set; } = "kg";
		public decimal UnitCoefficient { get; set; } = 1;
		public bool IsAltered => Unit != "kg";
	}
}