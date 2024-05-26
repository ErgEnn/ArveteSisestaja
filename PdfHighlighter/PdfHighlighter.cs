using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Security.Cryptography;
using System.Text;

namespace PdfHighlighter;

public class PdfHighlighter
{
    public static byte[] HighlightTextInPdf(byte[] pdfContent, string label, string stringToHighlight,
        out int matchesCount)
    {
        return HighlightTextInPdf(pdfContent, GenerateColor(label), stringToHighlight, out matchesCount);
    }
    public static byte[] HighlightTextInPdf(byte[] pdfContent, BaseColor color, string stringToHighlight, out int matchesCount)
    {
        matchesCount = 0;
        using (MemoryStream outputPdfStream = new MemoryStream())
        {
            PdfReader reader = new PdfReader(pdfContent);
            using (PdfStamper stamper = new PdfStamper(reader, outputPdfStream))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfContentByte canvas = stamper.GetOverContent(i);
                    canvas.SetColorFill(color);
                    canvas.SetGState(new PdfGState { FillOpacity = 0.5f });

                    var strategy = new CustomTextExtractionStrategy(stringToHighlight);
                    PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                    foreach (var rect in strategy.TextLocations)
                    {
                        matchesCount++;
                        canvas.Rectangle(rect.Left, rect.Bottom, rect.Width, rect.Height);
                        canvas.Fill();
                    }
                }
            }
            return outputPdfStream.ToArray();
        }
    }
    private static BaseFont font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
    public static byte[] AddLegendToPdf(byte[] pdfContent, params string[] legendEntries)
    {
        float fontSize = 12f;
        float padding = 10;
        using (MemoryStream outputPdfStream = new MemoryStream())
        {
            PdfReader reader = new PdfReader(pdfContent);
            using (PdfStamper stamper = new PdfStamper(reader, outputPdfStream))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfContentByte canvas = stamper.GetOverContent(i);
                    
                    float x = 10, y = 10;

                    foreach (string label in legendEntries)
                    {
                        float textWidth = font.GetWidthPoint(label, fontSize);
                        float rectWidth = textWidth + (2 * padding);
                        canvas.SetColorFill(/*entry.color*/ GenerateColor(label));
                        canvas.SetGState(new PdfGState { FillOpacity = 0.5f });
                        canvas.Rectangle(x, y - padding, rectWidth, fontSize + (2 * padding));
                        canvas.Fill();
                        canvas.BeginText();
                        canvas.SetColorFill(BaseColor.BLACK);
                        canvas.SetFontAndSize(font, fontSize);
                        canvas.SetTextMatrix(x + padding, y);
                        canvas.ShowText(label);
                        canvas.EndText();

                        x += rectWidth;
                    }
                    

                }
            }
            return outputPdfStream.ToArray();
        }
    }

    public static BaseColor GenerateColor(string input)
    {
        // Generate a hash from the input string
        using (var md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Extract RGB values
            int r = hashBytes[0];
            int g = hashBytes[1];
            int b = hashBytes[2];

            // Adjust color to ensure it's not too light or too dark
            const double minLuminance = 60;  // Minimum luminance threshold to ensure the color is not too light
            const double maxLuminance = 200; // Maximum luminance threshold to ensure the color is not too dark

            double luminance = 0.2126 * r + 0.7152 * g + 0.0722 * b;
            if (luminance > maxLuminance)
            {
                double scaleFactor = maxLuminance / luminance;
                r = (int)(r * scaleFactor);
                g = (int)(g * scaleFactor);
                b = (int)(b * scaleFactor);
            }
            else if (luminance < minLuminance)
            {
                double scaleFactor = minLuminance / luminance;
                r = Math.Min(255, (int)(r * scaleFactor));
                g = Math.Min(255, (int)(g * scaleFactor));
                b = Math.Min(255, (int)(b * scaleFactor));
            }

            return new BaseColor(r,g,b);
        }
    }
}