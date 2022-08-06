using Domain;

namespace DAL;

public class InvoiceRepository
{

    public IEnumerable<Invoice> GetAllInvoices()
    {
        return Enumerable.Empty<Invoice>();
    }

}