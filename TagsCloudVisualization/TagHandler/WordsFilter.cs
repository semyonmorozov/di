using System.Collections.Generic;

namespace TagsCloudVisualization.TagHandler
{
    class WordsFilter : ITagsHandler
    {
        private readonly List<string> forbiddenWords;

        public WordsFilter(List<string>forbiddenWords)
        {
            this.forbiddenWords = forbiddenWords;
        }

        public Dictionary<string, int> Handle(Dictionary<string, int> tags)
        {
            var handledTags = new Dictionary<string,int>();
            foreach (var word in tags.Keys)
            {
                var lowWord = word.ToLower();
                if (!forbiddenWords.Contains(lowWord))
                {
                    handledTags.Add(lowWord,tags[word]);
                }
            }
            return handledTags;
        }
    }
}
