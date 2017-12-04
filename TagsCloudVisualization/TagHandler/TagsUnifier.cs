using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization.TagHandler
{
    public class TagsUnifier :ITagsHandler
    {
        public Dictionary<string, int> Handle(Dictionary<string, int> tags)
        {
            return tags.Keys.ToDictionary(word => word.ToLower(), word => tags[word]);
        }
    }
}