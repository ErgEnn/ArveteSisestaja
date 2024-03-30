using InvoiceDownloader;

namespace DownloaderRunner
{
    public class Program
    {
        static async Task Main()
        {
            var downloader = new Downloader(("", ""), false, Int32.MaxValue);
            var progress = new Progress<(int, int)>();
            progress.ProgressChanged += (_, p) => Console.WriteLine($"Progress {p.Item1}/{p.Item2}");
            await downloader.DownloadInvoices(new DateOnly(2024, 01, 01), DateOnly.FromDateTime(DateTime.Today),
                progress);
        }
    }
}
