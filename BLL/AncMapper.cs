using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace BLL;

public class AncMapper
{
    private readonly AppDbContext _dbContext;

    public AncMapper(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task MapAncClassifiersToInvoiceItems(IReadOnlyCollection<MappedInvoice> invoices)
    {
        var existingMappings = await _dbContext.AncClassifierMappings.ToListAsync();
        bool shouldThrow = false;
        foreach (var item in invoices.AllItems())
        {
            var normalizedName = NormalizeProductName(item.PlainName);
            var mapping = existingMappings.SingleOrDefault(mapping => mapping.ProductName == normalizedName);
            if (mapping is null)
            {
                var entity = new AncClassifierMapping(0, normalizedName, null);
                _dbContext.AncClassifierMappings.Add(entity);
                existingMappings.Add(entity);
                shouldThrow = true;
            }

            item.Mapping = mapping;
        }
        if(shouldThrow)
            throw new Exception("Invoices have unmapped items");
    }

    private string NormalizeProductName(string name)
    {
        name = name.ReplaceLineEndings("");
        name = RemoveDates(name);

        return name;
    }


    private string RemoveDates(string name)
    {
        return new Regex("[0-3]?\\d\\.[0-1]\\d\\.20\\d\\d").Replace(name, "");
    }
}