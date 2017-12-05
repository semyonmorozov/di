using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Autofac;
using Moq;
using NUnit.Framework;
using TagsCloudVisualization.TagHandler;
using TagsCloudVisualization.TagReader;

namespace TagsCloudVisualization
{
    class TagsCloudVisualizator
    {
        private readonly Bitmap tagCloud;
        private readonly IRectangleLayouter layouter;
        private readonly ICloudDesign cloudDesign;
        private readonly ITagsReader tagReader;
        private readonly IEnumerable<ITagsHandler> tagHandlerers;

        public TagsCloudVisualizator(IRectangleLayouter layouter, ICloudDesign cloudDesign, ITagsReader tagReader, IEnumerable<ITagsHandler> tagHandlerers)
        {
            this.tagReader = tagReader;
            this.tagHandlerers = tagHandlerers;
            this.layouter = layouter;
            this.cloudDesign = cloudDesign;
            tagCloud = new Bitmap(cloudDesign.CloudWidth, cloudDesign.CloudHeight);
        }

        private Bitmap Visualize(Dictionary<string, int>tags)
        {
            var drawer = Graphics.FromImage(tagCloud);
            drawer.Clear(cloudDesign.BackgroundColor);

            foreach (var tag in tags)
            {
                var word = tag.Key;
                var weight = tag.Value;
                var font = cloudDesign.GetFont(weight);
                var textSize = Size.Ceiling(drawer.MeasureString(word, font));
                var rectangle = layouter.PutNextRectangle(textSize);
                var br = cloudDesign.GetStringBrush();
                drawer.DrawString(word, font, br, rectangle);
            }
            
            return tagCloud;
        }

        public Bitmap Visualize(string filePath)
        {
            var tags = tagReader.ReadTags(filePath);
            var handledTags = HandleTags(tags);
            return Visualize(handledTags);
        }

        private Dictionary<string, int> HandleTags(Dictionary<string, int> tags)
        {
            return tagHandlerers.Aggregate(tags, (current, handler) => handler.Handle(current));
        }
    }

    [TestFixture]
    public class TagsCloudVisualizator_Should
    {
        private ContainerBuilder builder;
        private IContainer container;
        private Mock<ICloudDesign> mockForDesign;
        private Mock<IRectangleLayouter> mockForLayouter;
        private Mock<ITagsReader> mockForReader;

        private Mock<ICloudDesign> ConfigureFakeDesign()
        {
            var mock = new Mock<ICloudDesign>();
            mock.Setup(d => d.CloudWidth).Returns(100);
            mock.Setup(d => d.CloudHeight).Returns(100);
            mock.Setup(d => d.GetFont(It.IsAny<int>())).Returns(new Font("", 1));
            mock.Setup(d => d.GetStringBrush()).Returns(new SolidBrush(Color.FromName("Black")));
            return mock;
        }

        private void BuildAndVizualize()
        {
            container = builder.Build();
            var vizualizator = container.Resolve<TagsCloudVisualizator>();
            vizualizator.Visualize("");
        }

        [SetUp]
        public void SetUp()
        {
            builder = new ContainerBuilder();

            mockForDesign = ConfigureFakeDesign();
            builder.Register(d=> mockForDesign.Object).As<ICloudDesign>();
            builder.Register(s=>Mock.Of<ICloudShape>()).As<ICloudShape>();

            mockForLayouter = new Mock<IRectangleLayouter>();
            builder.Register(l=>mockForLayouter.Object).As<IRectangleLayouter>();
            
            var tags = new Dictionary<string,int>
            {
                { "word1", 0 },
                { "word2", 0 },
                { "word3", 0 },
                { "word4", 0 }
            };
            mockForReader = new Mock<ITagsReader>();
            mockForReader.Setup(r => r.ReadTags(It.IsAny<string>())).Returns(tags);
            builder.Register(r=> mockForReader.Object).As<ITagsReader>();
            builder.RegisterType<TagsCloudVisualizator>();
        }

        [Test]
        public void RequestFont_ForEachTag()
        {
            BuildAndVizualize();
            mockForDesign.Verify(d=>d.GetFont(It.IsAny<int>()),Times.Exactly(4));
        }

        [Test]
        public void RequestBrush_ForEachTag()
        {
            BuildAndVizualize();
            mockForDesign.Verify(d => d.GetStringBrush(), Times.Exactly(4));
        }

        [Test]
        public void RequestBackgroundColor()
        {
            BuildAndVizualize();
            mockForDesign.Verify(d => d.BackgroundColor, Times.AtLeastOnce);
        }

        [Test]
        public void RequestRectangle_ForEachTag()
        {
            BuildAndVizualize();
            mockForLayouter.Verify(d => d.PutNextRectangle(It.IsAny<Size>()), Times.Exactly(4));
        }

        [Test]
        public void RequestTags()
        {
            BuildAndVizualize();
            mockForReader.Verify(d => d.ReadTags(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public void UseEachHendler()
        {
            var firstMock = new Mock<ITagsHandler>();
            firstMock.Setup(h => h.Handle(It.IsAny<Dictionary<string, int>>()))
                .Returns<Dictionary<string, int>>(tags => tags);

            var secondMock = new Mock<ITagsHandler>();
            secondMock.Setup(h => h.Handle(It.IsAny<Dictionary<string, int>>()))
                .Returns<Dictionary<string, int>>(tags => tags);

            builder.Register(h => firstMock.Object).As<ITagsHandler>();
            builder.Register(h => secondMock.Object).As<ITagsHandler>();
            BuildAndVizualize();

            firstMock.Verify(h=>h.Handle(It.IsAny<Dictionary<string,int>>()),Times.Once);
            secondMock.Verify(h => h.Handle(It.IsAny<Dictionary<string, int>>()), Times.Once);
        }
    }
}
