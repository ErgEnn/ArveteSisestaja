using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace PdfHighlighter;

public class PdfHighlighter
{
    public static byte[] HighlightTextInPdf(byte[] pdfContent, params string[] stringsToHighlight)
    {
        using (MemoryStream outputPdfStream = new MemoryStream())
        {
            PdfReader reader = new PdfReader(pdfContent);
            using (PdfStamper stamper = new PdfStamper(reader, outputPdfStream))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfContentByte canvas = stamper.GetOverContent(i);
                    canvas.SetColorFill(BaseColor.YELLOW);
                    canvas.SetGState(new PdfGState { FillOpacity = 0.5f });

                    foreach (var phrase in stringsToHighlight)
                    {
                        var strategy = new CustomTextExtractionStrategy(phrase);
                        PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                        foreach (var rect in strategy.TextLocations)
                        {
                            canvas.Rectangle(rect.Left, rect.Bottom, rect.Width, rect.Height);
                            canvas.Fill();
                        }

                    }
                }
            }
            return outputPdfStream.ToArray();
        }
    }
}