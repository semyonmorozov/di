using System.Collections.Generic;

namespace TagsCloudVisualization.TagReader
{
    public interface ITagsReader
    {
        Result<Dictionary<string, int>> ReadTags(string filePath);
    }
}