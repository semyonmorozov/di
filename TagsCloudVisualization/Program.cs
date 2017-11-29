using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Autofac;
using TagsCloudVisualization.CloudDesign;
using TagsCloudVisualization.CloudShape;
using TagsCloudVisualization.RectangleLayouter;

namespace TagsCloudVisualization
{
    static class Program
    {
        static void Main(string[] args)
        {
            var cloudDesign = new SimpleCloudDesign(Color.DarkMagenta, "Tahoma", new SolidBrush(Color.White), Screen.PrimaryScreen.Bounds);

            var builder = new ContainerBuilder();
            builder.Register(c=>cloudDesign).As<ICloudDesign>();
            builder.RegisterType<SpiralCloudShape>().As<ICloudShape>();
            builder.RegisterType<CircularCloudLayouter>().As<IRectangleLayouter>();
            builder.RegisterType<TagsCloudVisualizator>();

            var container = builder.Build();

            var vizualizator = container.Resolve <TagsCloudVisualizator>();

            string pathToProjDir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

            var tagReader = new TxtTagReader();

            var tags = tagReader.ReadTags(String.Concat(pathToProjDir, @"\NEW_wordsStats.txt"));
            var bitmap = vizualizator.Visualize(tags);
            var path = Path.Combine(Path.GetTempPath(), "result.png");
            bitmap.Save(path);

            Console.WriteLine(path);
            Console.ReadKey();
        }
    }
}
