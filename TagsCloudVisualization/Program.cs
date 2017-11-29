using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Autofac;

namespace TagsCloudVisualization
{
    static class Program
    {
        static void Main(string[] args)
        {
            var cloudDesign = new SimpleCloudDesign(Color.DarkMagenta, new Font("Tahoma", 20), new SolidBrush(Color.White), Screen.PrimaryScreen.Bounds);

            var builder = new ContainerBuilder();
            builder.Register(c=>cloudDesign).As<ICloudDesign>();
            builder.RegisterType<Spiral>();
            builder.RegisterType<UniquePositivePointsFromSpiral>();
            builder.RegisterType<CircularCloudLayouter>().As<IRectangleLayouter>();
            builder.RegisterType<TagsCloudVisualizator>();

            var container = builder.Build();

            var vizualizator = container.Resolve <TagsCloudVisualizator>();

            string pathToProjDir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var tags = ParseTagsFromFile(String.Concat(pathToProjDir, @"\wordsStats.txt"));
            var bitmap = vizualizator.Visualize(tags);
            var path = Path.Combine(Path.GetTempPath(), "result.png");
            bitmap.Save(path);

            Console.WriteLine(path);
            Console.ReadKey();
        }

        private static Dictionary<string, int> ParseTagsFromFile(string filePath)
        {
            var tags = new Dictionary<string, int>();
            using (var file = new StreamReader(filePath))
            {
                for (var i = 0; i < 400; i++)
                {
                    var line = file.ReadLine();
                    if (line == null) break;
                    line = Regex.Replace(line, "\t", " ");
                    var splitedLine = line.Split(' ');
                    var word = splitedLine[1];
                    var frequency = Int32.Parse(splitedLine[2]);
                    tags.Add(word, frequency);
                }
            }
            return tags;
        }
    }
}
