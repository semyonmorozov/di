using System.Drawing;

namespace TagsCloudVisualization.RectangleLayouter
{
    public interface IRectangleLayouter
    {
        Rectangle PutNextRectangle(Size textSize);
    }
}