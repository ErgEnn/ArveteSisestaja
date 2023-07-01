using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AncIngredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(256)]
        public string Name { get; init; }
        public int AncId { get; init; }
        [StringLength(8)]
        public string? UnitName { get; init; }
    }
}
