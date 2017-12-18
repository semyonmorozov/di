using CommandLine;

namespace TagsCloudVisualization
{
    public class CloudOptions
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
        
        public Result<CloudOptions> IsValid()
        {
            if (Width < 0)
                return Result.Fail<CloudOptions>("Width must be pozitive");
            if (Height < 0)
                return Result.Fail<CloudOptions>("Height must be pozitive");
            if (Width > 10000)
                return Result.Fail<CloudOptions>("Width must be less than 10000");
            if (Height > 10000)
                return Result.Fail<CloudOptions>("Height must be less than 10000");

            return Result.Ok(this);
        }
    }
    
}