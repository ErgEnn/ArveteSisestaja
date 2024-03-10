using InvoiceDownloader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        public Invoice[] Invoices { get; set; } = new[]
        {
            new Invoice("24004985", "AS Kaupmees & Ko", DateOnly.FromDateTime(DateTime.Today), null, null),
            new Invoice("24004985", "Roverto OÜ", DateOnly.FromDateTime(DateTime.Today), null, null),
            new Invoice("PRN265418", "AS Kaupmees & Ko", DateOnly.FromDateTime(DateTime.Today), null, null),
            new Invoice("24006277", "Roverto OÜ", DateOnly.FromDateTime(DateTime.Today), null, null),
            new Invoice("925", "TSITRUS KAUBANDUS OÜ", DateOnly.FromDateTime(DateTime.Today), null, null),
        };


        public IndexModel()
        {
            
        }

        public void OnGet()
        {
            
        }
    }
}
