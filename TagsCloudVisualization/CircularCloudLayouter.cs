using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter : IRectangleLayouter
    {
        private readonly UniquePositivePointsFromSpiral uniquePositivePoints;

        private readonly List<Rectangle> layout = new List<Rectangle>();

        public List<Rectangle> Layout()
        {
            var copyOfLayout = new Rectangle[layout.Count];
            layout.CopyTo(copyOfLayout);
            return copyOfLayout.ToList();
        }

        public CircularCloudLayouter(UniquePositivePointsFromSpiral uniquePositivePoints)
        {
            this.uniquePositivePoints = uniquePositivePoints;
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
            uniquePositivePoints.MoveNext();
            var rectangle = CreateRecnagleByCenter((Point)uniquePositivePoints.Current, rectangleSize);
            while (layout.Any(r => r.IntersectsWith(rectangle)))
            {
                uniquePositivePoints.MoveNext();
                rectangle = CreateRecnagleByCenter((Point)uniquePositivePoints.Current, rectangleSize);
            }
            layout.Add(rectangle);
            uniquePositivePoints.Reset();
            return rectangle;
        }
    }
}
