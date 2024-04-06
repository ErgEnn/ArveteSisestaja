using EInvoice;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using System.Xml;

namespace BLL;

public class InvoiceService
{
    private readonly AppDbContext _appDbContext;

    public InvoiceService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IReadOnlyCollection<MappedInvoice>> GetInvoices(DateOnly startDate, DateOnly endDate)
    {
        var invoices = await _appDbContext.Invoices.Where(invoice => invoice.InvoiceDate >= startDate && invoice.InvoiceDate <= endDate).ToArrayAsync();
        return invoices.Select(invoice =>
        {
            var mappedInvoice = new MappedInvoice(invoice);

            E_Invoice? einvoice = null;
            IReadOnlyCollection<MappedInvoice.InvoiceItem>? items = null;
            try
            {
                var reader = XmlReader.Create(invoice.XML.ToStream(),
                    new XmlReaderSettings() {ConformanceLevel = ConformanceLevel.Document});
                einvoice = new XmlSerializer(typeof(E_Invoice)).Deserialize(reader) as E_Invoice;
                var innerInvoice = einvoice.Invoice.Single();
                items = innerInvoice.InvoiceItem.InvoiceItemGroup.SelectMany(group => group.ItemEntry).Select(item => 
                    new MappedInvoice.InvoiceItem()
                {
                    Invoice = mappedInvoice,
                    EInvoiceItem = item
                }).ToList();
            }
            catch(Exception e)
            {
                mappedInvoice.EInvoiceParsingError = e;
                return mappedInvoice;
            }

            mappedInvoice.EInvoice = einvoice;
            mappedInvoice.Items = items;
            
            return mappedInvoice;
        }).ToList();
    }
}