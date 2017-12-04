using System.Collections.Generic;

namespace TagsCloudVisualization.TagReader
{
    public interface ITagsReader
    {
        Dictionary<string, int> ReadTags(string filePath);
    }
}