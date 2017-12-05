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
            var options = new Options();
            if (Parser.Default.ParseArgumentsStrict(args, options))
            {
                Visualize(options).Save("tagsCloud.png");
                Console.WriteLine("Done");
            }
            else Console.WriteLine("Invalid arguments");
        }

        private static Bitmap Visualize(Options options)
        {
            var width = options.Width == 0 ? Screen.PrimaryScreen.Bounds.Width : options.Width;
            var height = options.Height == 0 ? Screen.PrimaryScreen.Bounds.Height : options.Height;

            var cloudDesign = new SimpleCloudDesign(
                Color.FromName(options.BackgroundColor),
                options.Font,
                new SolidBrush(Color.FromName(options.FontColor)),
                new Rectangle(0,0,width,height));

            var builder = new ContainerBuilder();
            builder.Register(c => cloudDesign).As<ICloudDesign>();
            var shape = new SpiralCloudShape(cloudDesign,options.Spreading);
            builder.Register(s=>shape).As<ICloudShape>();
            builder.RegisterType<CircularCloudLayouter>().As<IRectangleLayouter>();
            builder.RegisterType<TxtTagReader>().As<ITagsReader>();
            builder.RegisterType<TagsUnifier>().As<ITagsHandler>();
            if (options.ForbiddenWords!=null)
            {
                var filter = new WordsFilter(ReadStringsFromTxt(options.ForbiddenWords));
                builder.Register(f => filter).As<ITagsHandler>();
            }
            builder.RegisterType<TagsCloudVisualizator>();

            var container = builder.Build();

            var vizualizator = container.Resolve<TagsCloudVisualizator>();

            return vizualizator.Visualize(options.TagsFile);
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

        class Options
        {
            [Option('t', "tags-file", Required = true, HelpText = "Path to file with tags.")]
            public string TagsFile { get; set; }

            [Option('s', "spreading", DefaultValue = 0.1, HelpText = "Spreading of tags layout.")]
            public double Spreading { get; set; }

            [Option('b', "bg-color", DefaultValue = "white", HelpText = "Background color.")]
            public string BackgroundColor { get; set; }

            [Option('c', "font-color", DefaultValue = "black", HelpText = "Font color.")]
            public string FontColor { get; set; }

            [Option('f', "font", DefaultValue = "Tahoma", HelpText = "Font type.")]
            public string Font { get; set; }

            [Option('w', "width", HelpText = "Width of result image. Default value is your screen width.")]
            public int Width { get; set; }

            [Option('h', "height", HelpText = "Height of result image. Default value is your screen height.")]
            public int Height { get; set; }

            [Option("filter", HelpText = "Path to file with words which must be filtered. Each word must be on a separate line.")]
            public string ForbiddenWords { get; set; }
        }
    }

    
    
}
