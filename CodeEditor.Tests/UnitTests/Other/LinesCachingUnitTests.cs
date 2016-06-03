using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CodeEditor.Extensions;
using CodeEditor.Visuals;
using CodeEditor.Visuals.Base;
using NUnit.Framework;

namespace CodeEditor.Tests.UnitTests.Other {
    [TestFixture]
    public class LinesCachingUnitTests {

        private FrameworkElement parent;
        private VisualCollection collection;

        [SetUp]
        public void InitializeTest() {
            parent = new FrameworkElement();
            collection = new VisualCollection(parent);
        }

        [Test]
        public void CacheThreeSingleLines_ShouldCacheThreeLines() {
            collection.Add(VisualTextLine.Create("asdf", 0));
            collection.Add(VisualTextLine.Create("{", 1));
            collection.Add(VisualTextLine.Create("", 2));
            collection.Add(VisualTextLine.Create("}", 3));

            var cachedResult = collection.ConvertToCachedLines(3, 1);

            Assert.AreEqual(cachedResult.Count, 3);
            Assert.AreEqual(cachedResult[0].RenderedText, "{");
            Assert.AreEqual(cachedResult[1].RenderedText, "");
            Assert.AreEqual(cachedResult[2].RenderedText, "}");
        }

        [Test]
        public void CacheCollapsedAndSingleLines_ShouldCacheThreeLines() {
            const string FirstLine = "asdf";
            const string Collapse = "{...}";
            const string LastLine = "zxcv";

            collection.Add(VisualTextLine.Create(FirstLine, 0));
            collection.Add(VisualTextLine.Create(new[] { "{", "", "}" }, "", "", 1, Collapse));
            collection.Add(VisualTextLine.Create(LastLine, 2));

            var cachedResult = collection.ConvertToCachedLines(3);

            Assert.AreEqual(cachedResult.Count, 3);
            Assert.AreEqual(cachedResult[0].RenderedText, FirstLine);
            Assert.AreEqual(cachedResult[1].RenderedText, Collapse);
            Assert.AreEqual(cachedResult[2].RenderedText, LastLine);
        }

        [Test]
        public void CacheAndRecreate_ShouldHaveThreeLines() {
            const string FirstLine = "asdf";
            const string Collapse = "{...}";
            const string LastLine = "zxcv";
            string[] collapsedContents = { "{ jh", "as", "cv }" };
            var cachedLines = new List<CachedVisualTextLine> {
                new CachedSingleVisualTextLine(FirstLine, 0),
                new CachedCollapsedVisualTextLine(collapsedContents, "", "", 1, Collapse),
                new CachedSingleVisualTextLine(LastLine, 2)
            };


            var visualLines = cachedLines.GetVisualLines().ToList();

            Assert.IsInstanceOf<SingleVisualTextLine>(visualLines[0]);
            Assert.IsInstanceOf<CollapsedVisualTextLine>(visualLines[1]);
            Assert.IsInstanceOf<SingleVisualTextLine>(visualLines[2]);
            Assert.That(visualLines[0].RenderedText, Is.EqualTo(FirstLine));
            Assert.That(visualLines[1].RenderedText, Is.EqualTo(Collapse));
            Assert.That(visualLines[2].RenderedText, Is.EqualTo(LastLine));
        }

    }
}
