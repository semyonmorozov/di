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

        public Result<Font> GetFont(int weight)
        {
            var font = new Font(fontName, weight);
            if(fontName!=font.Name)
                return Result.Fail<Font>("Font "+fontName+ " not found in our system. Please install it or choose another font");
            return new Font(fontName, weight);
        }

        public Brush GetStringBrush()
        {
            return stringBrush;
        }
    }
}