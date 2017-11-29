using System;
using System.Drawing;

namespace TagsCloudVisualization.CloudShape
{
    class SpiralCloudShape : ICloudShape
    {
        
        private int index=-1;
        private readonly Point center;
        private readonly double spreading;

        public Point Current => GetPointFromSpiral(index);

        public SpiralCloudShape(ICloudDesign cloudDesign, double spreading = 0.1)
        {
            center = new Point(cloudDesign.CloudWidth / 2, cloudDesign.CloudHeight / 2);
            this.spreading = spreading;
        }

        public bool MoveNext()
        {
            var oldPoint = Current;
            index++;
            while(oldPoint.Equals(Current)||!IsPositive(Current))
                    index++;
            return true;
        }

        private bool IsPositive(Point point)
        {
            return point.X > 0 && point.Y > 0;
        }

        public void Reset()
        {
            index = -1;
        }

        private Point GetPointFromSpiral(double angle)
        {
            var x = (int)(spreading * angle * Math.Cos(angle)) + center.X;
            var y = (int)(spreading * angle * Math.Sin(angle)) + center.Y;
            return new Point(x, y);
        }
    }
}
