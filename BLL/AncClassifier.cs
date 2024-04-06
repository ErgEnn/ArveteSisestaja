using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLL
{
    public record AncClassifier(
        [property: Key]
        [property: DatabaseGenerated(DatabaseGeneratedOption.None)]
        int Id, string Name, string Unit, decimal UnitCoefficient);
}
