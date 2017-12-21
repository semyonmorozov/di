using System.Drawing;

namespace TagsCloudVisualization.RectangleLayouter
{
    public interface IRectangleLayouter
    {
        Result<Rectangle> PutNextRectangle(Size textSize);
    }
}