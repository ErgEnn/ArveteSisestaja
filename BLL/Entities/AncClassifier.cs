using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Entities
{
    public class AncClassifier
    {
        public string ExternalId { get; set; }
        public decimal Coefficient { get; set; }
    }

    public class NonProductAncClassifier : AncClassifier
    {

    }
}
