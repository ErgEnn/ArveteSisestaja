namespace BLL.Entities;

public class Product
{
    public string Name { get; set; }
    public AncClassifier? AncClassifier { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }

    public decimal GetAdjustedAmount() => Amount * AncClassifier.Coefficient;
    public bool HasClassifier() => AncClassifier != null;
}