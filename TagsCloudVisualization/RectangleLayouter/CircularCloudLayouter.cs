using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.CloudDesign;
using TagsCloudVisualization.CloudShape;

namespace TagsCloudVisualization.RectangleLayouter
{
    class CircularCloudLayouter : IRectangleLayouter
    {
        private readonly ICloudShape cloudShape;
        private readonly Rectangle border;

        private readonly List<Rectangle> layout = new List<Rectangle>();

        public List<Rectangle> Layout()
        {
            var copyOfLayout = new Rectangle[layout.Count];
            layout.CopyTo(copyOfLayout);
            return copyOfLayout.ToList();
        }

        public CircularCloudLayouter(ICloudShape cloudShape, ICloudDesign cloudDesign)
        {
            this.cloudShape = cloudShape;
            border = new Rectangle(0, 0, cloudDesign.CloudWidth, cloudDesign.CloudHeight);
        }

        private static Rectangle CreateRecnagleByCenter(Point center, Size size)
        {
            var leftTopPointX = center.X - size.Width / 2;
            var leftTopPointY = center.Y - size.Height / 2;
            var leftTopPoint = new Point(leftTopPointX, leftTopPointY);
            return new Rectangle(leftTopPoint, size);
        }

        public Result<Rectangle> PutNextRectangle(Size rectangleSize)
        {
            cloudShape.MoveNext();
            var rectangle = CreateRecnagleByCenter(cloudShape.Current, rectangleSize);
            while (layout.Any(r => r.IntersectsWith(rectangle)))
            {
                cloudShape.MoveNext();
                rectangle = CreateRecnagleByCenter(cloudShape.Current, rectangleSize);
            }
            layout.Add(rectangle);
            cloudShape.Reset();
            if (!border.Contains(rectangle))
                return Result.Fail<Rectangle>("Cloud is too large for resolution " + border.Width + "x" + border.Height);
            return rectangle;
        }
    }
}
