using BlazorApp.Components;
using BLL;
using InvoiceDownloader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddBlazorBootstrap();

            builder.Services.AddDbContextFactory<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DB")));

            builder.Services.Configure<AncOptions>(builder.Configuration.GetSection("Anc"));
            builder.Services.Configure<OmnivaOptions>(builder.Configuration.GetSection("Omniva"));
            builder.Services.AddSingleton<AncHandler>();
            builder.Services.AddTransient<Downloader>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<OmnivaOptions>>();
                return new Downloader((options.Value.Username, options.Value.Password));
            });
            builder.Services.AddTransient<PriaReportService>();
            builder.Services.AddTransient<PriaExcelReportGenerator>();
            builder.Services.AddTransient<PriaPDFGenerator>();
            builder.Services.AddTransient<InvoiceService>();
            builder.Services.AddTransient<AncMapper>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
