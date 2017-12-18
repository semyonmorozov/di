using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization.TagHandler
{
    class WordsFilter : ITagsHandler
    {
        private readonly string filePath;

        public WordsFilter(string filePath)
        {
            this.filePath = filePath;
        }

        public Result<Dictionary<string, int>> Handle(Dictionary<string, int> tags)
        {
            return Result.Of(() => File.ReadAllLines(filePath).ToList())
                .Then(w=>Filter(w,tags))
                .RefineError("An error occurred with the file " + filePath);
        }

        private static Result<Dictionary<string, int>> Filter(List<string> forbiddenWords, Dictionary<string, int> tags)
        {
            var handledTags = new Dictionary<string, int>();
            foreach (var word in tags.Keys)
            {
                var lowWord = word.ToLower();
                if (!forbiddenWords.Contains(lowWord))
                {
                    handledTags.Add(lowWord, tags[word]);
                }
            }
            return handledTags;
        }
    }
}
