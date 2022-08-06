using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ItemAncClassifier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; init; }

        public string? AncIngredientName { get; init; }
        public AncIngredient? AncIngredient { get; init; }
        public decimal? Coefficient { get; init; }
    }
}
