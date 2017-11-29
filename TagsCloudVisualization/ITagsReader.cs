using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public interface ITagsReader
    {
        Dictionary<string, int> ReadTags(string filePath);
    }
}