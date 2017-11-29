using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Spiral
    {
        private readonly Point center;
        private readonly double spreading;

        public Spiral(ICloudDesign cloudDesign, double spreading=0.1)
        {
            center = new Point(cloudDesign.CloudWidth/2, cloudDesign.CloudHeight/2);
            this.spreading = spreading;
        }

        public Point GetPoint(double angle)
        {
            var x = (int) (spreading * angle * Math.Cos(angle)) + center.X;
            var y = (int) (spreading * angle * Math.Sin(angle)) + center.Y;
            return new Point(x, y);
        }
    }
}
