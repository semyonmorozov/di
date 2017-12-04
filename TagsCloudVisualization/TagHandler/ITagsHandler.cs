using System.Collections.Generic;

namespace TagsCloudVisualization.TagHandler
{
    public interface ITagsHandler
    {
        Dictionary<string, int> Handle(Dictionary<string, int> tags);
    }
}