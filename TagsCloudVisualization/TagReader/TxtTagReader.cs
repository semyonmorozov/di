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
            return ReadStringsFromFile(filePath).Then(ParseTags);
        }

        public Result<List<string>> ReadStringsFromFile(string filePath)
        {
            try
            {
                var strings = new List<string>();
                using (var file = new StreamReader(filePath))
                {
                    while (true)
                    {
                        var line = file.ReadLine();
                        if (line == null) break;
                        strings.Add(line);
                    }
                }
                return strings;
            }
            catch (FileNotFoundException e)
            {
                return Result.Fail<List<string>>(e.Message);
            }
        }

        public Result<Dictionary<string, int>> ParseTags(List<string> strings)
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