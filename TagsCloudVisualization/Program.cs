using System;
using System.Drawing;
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
            builder.RegisterType<SpiralCloudShape>().As<ICloudShape>();
            builder.RegisterType<CircularCloudLayouter>().As<IRectangleLayouter>();
            builder.RegisterType<TxtTagReader>().As<ITagsReader>();
            builder.RegisterType<TagsUnifier>().As<ITagsHandler>();
            builder.RegisterType<WordsFilter>().As<ITagsHandler>();
            builder.RegisterType<TagsCloudVisualizator>();

            var container = builder.Build();

            var vizualizator = container.Resolve<TagsCloudVisualizator>();

            return vizualizator.Visualize(options.TagsFile);
        }

        class Options
        {
            [Option('t', "tags-file", Required = true, HelpText = "Path to file with tags.")]
            public string TagsFile { get; set; }

            [Option('b', "bg-color", DefaultValue = "white", HelpText = "Background color.")]
            public string BackgroundColor { get; set; }

            [Option('c', "font-color", DefaultValue = "black", HelpText = "Font color.")]
            public string FontColor { get; set; }

            [Option('f', "font", DefaultValue = "Tahoma", HelpText = "Font type.")]
            public string Font { get; set; }

            [Option('w', "width", HelpText = "Width of result image")]
            public int Width { get; set; }

            [Option('h', "height", HelpText = "Height of result image")]
            public int Height { get; set; }
        }
    }

    
    
}
