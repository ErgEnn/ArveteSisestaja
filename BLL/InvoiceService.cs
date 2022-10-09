using BLL.Entities;

namespace BLL;

public class InvoiceService
{
    private List<Invoice> _invoices = new();

    public InvoiceService()
    {
    }

    public IEnumerable<Invoice> GetAllInvoices()
    {
        return _invoices;
    }

    public void AddInvoiceRange(List<Invoice> invoices)
    {
        _invoices.AddRange(invoices);
    }

    public Invoice GetById(string identifier)
    {
        return _invoices.Single(i => i.Identifier == identifier);
    }
}