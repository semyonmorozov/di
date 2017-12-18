using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization.TagReader
{
    public class TxtTagReader : ITagsReader
    {
        public Result<Dictionary<string, int>> ReadTags(string filePath)
        {
            return Result.Of(() => File.ReadAllLines(filePath).ToList())
                .Then(ParseTags)
                .RefineError("An error occurred with the file " + filePath);
        }

        private Result<Dictionary<string, int>> ParseTags(List<string> strings)
        {
            var tags = new Dictionary<string, int>();
            var splitedStrings = strings.Select(s => s.Split(' ')).ToArray();
            for (var strNum=0;strNum<splitedStrings.Length;strNum++)
            {
                var splitedString = splitedStrings[strNum];
                try
                {
                    tags.Add(splitedString[0], int.Parse(splitedString[1]));
                }
                catch (FormatException)
                {
                    return Result.Fail<Dictionary<string, int>>("Wrong weight format in line " + strNum);
                }
                catch (IndexOutOfRangeException)
                {
                    return Result.Fail<Dictionary<string, int>>("Wrong format in line " + strNum);
                }
            }
            return tags;
        }
    }
}