using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Autofac;
using CommandLine;
using TagsCloudVisualization.CloudDesign;
using TagsCloudVisualization.CloudShape;
using TagsCloudVisualization.RectangleLayouter;
using TagsCloudVisualization.TagHandler;
using TagsCloudVisualization.TagReader;

namespace TagsCloudVisualization
{
    static class Program
    {
        static void Main(string[] args)
        {
            var options = new CloudOptions();
            if (Parser.Default.ParseArgumentsStrict(args, options))
            {
                Visualize(options).Save("tagsCloud.png");
                Console.WriteLine("Done");
            }
            else Console.WriteLine("Invalid arguments");
        }

        private static Bitmap Visualize(CloudOptions cloudOptions)
        {
            var width = cloudOptions.Width == 0 ? Screen.PrimaryScreen.Bounds.Width : cloudOptions.Width;
            var height = cloudOptions.Height == 0 ? Screen.PrimaryScreen.Bounds.Height : cloudOptions.Height;

            var cloudDesign = new SimpleCloudDesign(
                Color.FromName(cloudOptions.BackgroundColor),
                cloudOptions.Font,
                new SolidBrush(Color.FromName(cloudOptions.FontColor)),
                new Rectangle(0,0,width,height));

            var builder = new ContainerBuilder();
            builder.Register(c => cloudDesign).As<ICloudDesign>();
            var shape = new SpiralCloudShape(cloudDesign,cloudOptions.Spreading);
            builder.Register(s=>shape).As<ICloudShape>();
            builder.RegisterType<CircularCloudLayouter>().As<IRectangleLayouter>();
            builder.RegisterType<TxtTagReader>().As<ITagsReader>();
            builder.RegisterType<TagsUnifier>().As<ITagsHandler>();
            if (cloudOptions.ForbiddenWords!=null)
            {
                var filter = new WordsFilter(ReadStringsFromTxt(cloudOptions.ForbiddenWords));
                builder.Register(f => filter).As<ITagsHandler>();
            }
            builder.RegisterType<TagsCloudVisualizator>();

            var container = builder.Build();

            var vizualizator = container.Resolve<TagsCloudVisualizator>();

            return vizualizator.Visualize(cloudOptions.TagsFile);
        }

        private static List<string> ReadStringsFromTxt(string path)
        {
            var strings = new List<string>();
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while((line=sr.ReadLine())!=null)
                    strings.Add(line);
            }
            return strings;
        }
}

    
    
}
