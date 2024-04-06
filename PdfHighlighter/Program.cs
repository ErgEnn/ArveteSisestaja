using iTextSharp.text;

namespace PdfHighlighter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pdf = File.ReadAllBytes("D:\\Ergo\\Downloads\\PRN263811.pdf");
            var highlightedPdf = PdfHighlighter.HighlightTextInPdf(pdf, BaseColor.YELLOW,"ALMA Piim 2,5% 1l(kile)", out var _);
            File.WriteAllBytes("D:\\Ergo\\Downloads\\PRN263811-highlighted.pdf", highlightedPdf);
        }
    }
}
