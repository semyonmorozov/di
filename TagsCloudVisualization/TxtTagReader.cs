using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TagsCloudVisualization
{
    public class TxtTagReader : ITagsReader
    {
        public Dictionary<string, int> ReadTags(string filePath)
        {
            var tags = new Dictionary<string, int>();
            using (var file = new StreamReader(filePath))
            {
                while (true)
                {
                    var line = file.ReadLine();
                    if (line == null) break;
                    var splitedLine = line.Split(' ');
                    var word = splitedLine[0];
                    var frequency = Int32.Parse(splitedLine[1]);
                    tags.Add(word, frequency);
                }
            }
            return tags;
        }
    }
}