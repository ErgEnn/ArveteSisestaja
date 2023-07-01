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
        [StringLength(256)]
        public string Name { get; init; }
        [StringLength(256)]
        public string? AncIngredientName { get; init; }
        public AncIngredient? AncIngredient { get; init; }
        public decimal? Coefficient { get; init; }
    }
}
