using System.Collections.Generic;

namespace TagsCloudVisualization.TagHandler
{
    public interface ITagsHandler
    {
        Result<Dictionary<string, int>> Handle(Dictionary<string, int> tags);
    }
}