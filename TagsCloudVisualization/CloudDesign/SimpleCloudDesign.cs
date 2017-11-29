using System.Drawing;

namespace TagsCloudVisualization.CloudDesign
{
    public class SimpleCloudDesign : ICloudDesign
    {
        public Color BackgroundColor { get; }
        public int CloudWidth { get; }
        public int CloudHeight { get; }
        private readonly string fontName;
        private readonly Brush stringBrush;
        public SimpleCloudDesign(Color bgColor, string fontName, Brush stringBrush,Rectangle drawingField)
        {
            CloudWidth = drawingField.Width;
            CloudHeight = drawingField.Height;
            BackgroundColor = bgColor;
            this.fontName = fontName;
            this.stringBrush = stringBrush;
        }

        public Font GetFont(int weight)
        {
            return new Font(fontName,weight);
        }

        public Brush GetStringBrush()
        {
            return stringBrush;
        }
    }
}