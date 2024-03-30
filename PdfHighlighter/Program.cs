namespace PdfHighlighter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pdf = File.ReadAllBytes("D:\\Ergo\\Downloads\\PRN263811.pdf");
            var highlightedPdf = PdfHighlighter.HighlightTextInPdf(pdf, "ALMA Piim 2,5% 1l(kile)");
            File.WriteAllBytes("D:\\Ergo\\Downloads\\PRN263811-highlighted.pdf", highlightedPdf);
        }
    }
}
