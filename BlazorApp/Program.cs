using BlazorApp.Components;
using InvoiceDownloader;
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

            builder.Services.Configure<AncOptions>(builder.Configuration.GetSection("Anc"));
            builder.Services.Configure<OmnivaOptions>(builder.Configuration.GetSection("Omniva"));
            builder.Services.AddSingleton<AncHandler>();
            builder.Services.AddTransient<Downloader>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<OmnivaOptions>>();
                return new Downloader((options.Value.Username, options.Value.Password));
            });
            builder.Services.AddSqlite<DbContext>(builder.Configuration.GetConnectionString("DB"));

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
