using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudDesign;
using TagsCloudVisualization.CloudShape;

namespace TagsCloudVisualization.RectangleLayouter
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter layouter;
        private Rectangle screenBounds = Screen.PrimaryScreen.Bounds;

        private static double GetMaxDistance(Rectangle rectangle, Point center)
        {
            var extremePoints = new List<Point>
            {
                new Point(rectangle.Left, rectangle.Top),
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Left, rectangle.Bottom),
                new Point(rectangle.Right, rectangle.Top)
            };
            return extremePoints.Select(p => GetDistance(p, center)).Max();
        }

        private static double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        private static CircularCloudLayouter FillUpLayout(CircularCloudLayouter layouter, int numOfRectangles)
        {
            var rnd = new Random();
            for (var i = 0; i < numOfRectangles; i++)
                layouter.PutNextRectangle(new Size(rnd.Next(10, 100), rnd.Next(10, 100)));
            return layouter;
        }

        [SetUp]
        public void SetUp()
        {
            var cloudDesign = new SimpleCloudDesign(Color.FromName("White"), "", new SolidBrush(Color.FromName("Black")), screenBounds);
            layouter = new CircularCloudLayouter(new SpiralCloudShape(cloudDesign));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(150)]
        public void AddingRectanglesToLayout(int numOfRectangles)
        {
            layouter = FillUpLayout(layouter, numOfRectangles);
            layouter.Layout().Count.Should().Be(numOfRectangles);
        }

        [TestCase(10)]
        [TestCase(30)]
        public void LayOutRectangles_WithoutIntersection(int numOfRectangles)
        {
            layouter = FillUpLayout(layouter, numOfRectangles);
            var layout = layouter.Layout();
            foreach (var rectangleA in layout)
            foreach (var rectangleB in layout)
                if (rectangleA != rectangleB)
                    rectangleA.IntersectsWith(rectangleB).Should().Be(false);
        }

        [TestCase(0.4, 100)]
        [TestCase(0.5, 150)]
        [TestCase(0.6, 300)]
        public void LayOutRectangles_Tightly(double ratioOfAreas, int numOfRectangles)
        {
            layouter = FillUpLayout(layouter, numOfRectangles);
            var center = new Point(screenBounds.Width / 2, screenBounds.Height / 2);
            var radius = layouter.Layout().Select(rectangle => GetMaxDistance(rectangle, center)).Max();
            var areaOfRectangles = layouter.Layout().Sum(r => (double) r.Height * r.Width);
            var areaOfCircle = Math.PI * Math.Pow(radius, 2);
            areaOfRectangles.Should().BeGreaterThan(areaOfCircle * ratioOfAreas);
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.FailCount > 0)
            {
                var testName = TestContext.CurrentContext.Test.Name;
                var bitmap = new Bitmap(screenBounds.Width, screenBounds.Height);
                var drawer = Graphics.FromImage(bitmap);
                foreach (var r in layouter.Layout())
                    drawer.DrawRectangle(new Pen(Color.Black, 1), r);
                var path = Path.Combine(Path.GetTempPath(), testName + ".png");
                bitmap.Save(path);
                TestContext.WriteLine("Tag cloud visualization saved to file " + path);
            }
        }
    }
}