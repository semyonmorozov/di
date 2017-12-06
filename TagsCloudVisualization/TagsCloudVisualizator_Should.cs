using System.Collections.Generic;
using System.Drawing;
using Autofac;
using Moq;
using NUnit.Framework;
using TagsCloudVisualization.CloudDesign;
using TagsCloudVisualization.CloudShape;
using TagsCloudVisualization.RectangleLayouter;
using TagsCloudVisualization.TagHandler;
using TagsCloudVisualization.TagReader;

namespace TagsCloudVisualization
{
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