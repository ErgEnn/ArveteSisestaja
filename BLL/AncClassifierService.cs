using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Entities;

namespace BLL
{
    public class AncClassifierService
    {
        public void ClassifyInvoices(IEnumerable<Invoice> invoices)
        {
            foreach (var invoice in invoices)
            {
                foreach (var unclassifiedProduct in invoice.GetUnclassifiedProducts())
                {
                    
                }
            }
        }
    }
}
