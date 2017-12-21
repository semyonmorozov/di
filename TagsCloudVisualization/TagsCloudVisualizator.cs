using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.CloudDesign;
using TagsCloudVisualization.RectangleLayouter;
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

        private Result<Bitmap> Visualize(Dictionary<string, int>tags)
        {
            var drawer = Graphics.FromImage(tagCloud);
            drawer.Clear(cloudDesign.BackgroundColor);
            
            foreach (var tag in tags)
            {
                var getFontResult = cloudDesign.GetFont(tag.Value);
                var layoutResult = getFontResult
                    .Then(font => drawer.MeasureString(tag.Key, font))
                    .Then(Size.Ceiling)
                    .Then(layouter.PutNextRectangle)
                    .Then(r=> drawer.DrawString(tag.Key, getFontResult.Value, cloudDesign.GetStringBrush(), r));
                if (!layoutResult.IsSuccess)
                    return Result.Fail<Bitmap>(layoutResult.Error);
            }
            
            return tagCloud;
        }

        public Result<Bitmap> Visualize(string filePath)
        {
            return tagReader
                .ReadTags(filePath)
                .Then(HandleTags)
                .Then(Visualize);
        }

        private Result<Dictionary<string, int>> HandleTags(Dictionary<string, int> tags)
        {
            return tagHandlerers.Aggregate(Result.Ok(tags), (current, handler) => current.Then(handler.Handle));
        }
    }
}
