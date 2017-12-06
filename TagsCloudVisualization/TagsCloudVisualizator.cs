using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.TagHandler;
using TagsCloudVisualization.TagReader;

namespace TagsCloudVisualization
{
    class TagsCloudVisualizator
    {
        private readonly Bitmap tagCloud;
        private readonly IRectangleLayouter layouter;
        private readonly ICloudDesign cloudDesign;
        private readonly ITagsReader tagReader;
        private readonly IEnumerable<ITagsHandler> tagHandlerers;

        public TagsCloudVisualizator(IRectangleLayouter layouter, ICloudDesign cloudDesign, ITagsReader tagReader, IEnumerable<ITagsHandler> tagHandlerers)
        {
            this.tagReader = tagReader;
            this.tagHandlerers = tagHandlerers;
            this.layouter = layouter;
            this.cloudDesign = cloudDesign;
            tagCloud = new Bitmap(cloudDesign.CloudWidth, cloudDesign.CloudHeight);
        }

        private Bitmap Visualize(Dictionary<string, int>tags)
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

        public Bitmap Visualize(string filePath)
        {
            var tags = tagReader.ReadTags(filePath);
            var handledTags = HandleTags(tags);
            return Visualize(handledTags);
        }

        private Dictionary<string, int> HandleTags(Dictionary<string, int> tags)
        {
            return tagHandlerers.Aggregate(tags, (current, handler) => handler.Handle(current));
        }
    }
}
