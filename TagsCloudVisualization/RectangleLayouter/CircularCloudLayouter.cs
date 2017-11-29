using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.RectangleLayouter
{
    class CircularCloudLayouter : IRectangleLayouter
    {
        private readonly ICloudShape cloudShape;

        private readonly List<Rectangle> layout = new List<Rectangle>();

        public List<Rectangle> Layout()
        {
            var copyOfLayout = new Rectangle[layout.Count];
            layout.CopyTo(copyOfLayout);
            return copyOfLayout.ToList();
        }

        public CircularCloudLayouter(ICloudShape cloudShape)
        {
            this.cloudShape = cloudShape;
        }

        private static Rectangle CreateRecnagleByCenter(Point center, Size size)
        {
            var leftTopPointX = center.X - size.Width / 2;
            var leftTopPointY = center.Y - size.Height / 2;
            var leftTopPoint = new Point(leftTopPointX,leftTopPointY);
            return new Rectangle(leftTopPoint,size);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
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
            return rectangle;
        }
    }
}
