using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class TagsCloudVisualizator
    {
        private readonly Bitmap tagCloud;
        private readonly IRectangleLayouter layouter;
        private readonly ICloudDesign cloudDesign;


        public TagsCloudVisualizator(IRectangleLayouter layouter, ICloudDesign cloudDesign)
        {
            this.layouter = layouter;
            this.cloudDesign = cloudDesign;
            tagCloud = new Bitmap(cloudDesign.CloudWidth, cloudDesign.CloudHeight);
        }

        public Bitmap Visualize(Dictionary<string, int>tags)
        {
            var drawer = Graphics.FromImage(tagCloud);
            drawer.Clear(cloudDesign.BackgroundColor);

            foreach (var tag in tags)
            {
                var word = tag.Key;
                var weight = tag.Value;
                var font = cloudDesign.GetFont(weight);
                var textSize = Size.Ceiling(drawer.MeasureString(word, font));
                var rectangle = layouter.PutNextRectangle(textSize);
                var br = cloudDesign.GetStringBrush();
                drawer.DrawString(word, font, br, rectangle);
            }
            
            return tagCloud;
        }
    }
}
