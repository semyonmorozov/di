using System.Drawing;

namespace TagsCloudVisualization.CloudDesign
{
    public interface ICloudDesign
    {
        Color BackgroundColor { get; }
        int CloudWidth { get; }
        int CloudHeight { get; }

        Result<Font> GetFont(int weight);

        Brush GetStringBrush();
    }
}