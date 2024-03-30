using iTextSharp.text;
using iTextSharp.text.pdf.parser;

namespace PdfHighlighter
{
    internal class CustomTextExtractionStrategy : LocationTextExtractionStrategy
    {
        public List<Rectangle> TextLocations { get; private set; } = new List<Rectangle>();
        private string searchText;

        public CustomTextExtractionStrategy(string searchText)
        {
            this.searchText = searchText;
        }

        public override void RenderText(TextRenderInfo renderInfo)
        {
            base.RenderText(renderInfo);
            var txt = renderInfo.GetText();
            if (txt.Equals(searchText, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Found");
                var startPosition = renderInfo.GetBaseline().GetStartPoint();
                var endPosition = renderInfo.GetBaseline().GetEndPoint();
                var bottomLeft = renderInfo.GetDescentLine().GetStartPoint();
                var topRight = renderInfo.GetAscentLine().GetEndPoint();
                var rect = new Rectangle(
                    startPosition[Vector.I1],
                    bottomLeft[Vector.I2],
                    endPosition[Vector.I1],
                    topRight[Vector.I2]);
                TextLocations.Add(rect);
            }
        }
    }
}
