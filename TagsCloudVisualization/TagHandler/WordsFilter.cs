using System.Collections.Generic;

namespace TagsCloudVisualization.TagHandler
{
    class WordsFilter : ITagsHandler
    {
        private readonly List<string> boringWords;

        public WordsFilter()
        {
            boringWords = new List<string>
            {
                "i",
                "you",
                "me",
                "he",
                "she",
                "a",
                "it",
                "and",
                "on",
                "of",
                "for",
                "to",
                "the",
                "that"
            };
        }

        public WordsFilter(List<string>boringWords)
        {
            this.boringWords = boringWords;
        }

        public Dictionary<string, int> Handle(Dictionary<string, int> tags)
        {
            var handledTags = new Dictionary<string,int>();
            foreach (var word in tags.Keys)
            {
                var lowWord = word.ToLower();
                if (!boringWords.Contains(lowWord))
                {
                    handledTags.Add(lowWord,tags[word]);
                }
            }
            return handledTags;
        }
    }
}
