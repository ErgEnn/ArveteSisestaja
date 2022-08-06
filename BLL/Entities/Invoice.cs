using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Entities
{
    public class Invoice
    {
        public string Vendor { get; init; }
        public string Identifier { get; init; }
        public DateOnly Date { get; init; }

        public bool ExistsInAnc { get; private set; }

    }
}
