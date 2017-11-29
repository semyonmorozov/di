using System.Drawing;

namespace TagsCloudVisualization
{
    public class SimpleCloudDesign : ICloudDesign
    {
        public Color BackgroundColor { get; }
        public int CloudWidth { get; }
        public int CloudHeight { get; }
        private readonly Font font;
        private readonly Brush stringBrush;
        public SimpleCloudDesign(Color bgColor, Font font,Brush stringBrush,Rectangle drawingField)
        {
            CloudWidth = drawingField.Width;
            CloudHeight = drawingField.Height;
            BackgroundColor = bgColor;
            this.font = font;
            this.stringBrush = stringBrush;
        }

        public Font GetFont(int weight)
        {
            return font;
        }

        public Brush GetStringBrush()
        {
            return stringBrush;
        }
    }
}