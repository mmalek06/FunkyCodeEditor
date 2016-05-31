using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CodeEditor.Caching;
using CodeEditor.Extensions;
using CodeEditor.Visuals;
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

            var cachedResult = collection.ConvertToCachedLines(1, 3);

            Assert.AreEqual(cachedResult.Count, 3);
            Assert.AreEqual(cachedResult[0].RenderedContents, "{");
            Assert.AreEqual(cachedResult[1].RenderedContents, "");
            Assert.AreEqual(cachedResult[2].RenderedContents, "}");
        }

        [Test]
        public void CacheCollapsedAndSingleLines_ShouldCacheThreeLines() {
            const string FirstLine = "asdf";
            const string Collapse = "{...}";
            const string LastLine = "zxcv";

            collection.Add(VisualTextLine.Create(FirstLine, 0));
            collection.Add(VisualTextLine.Create(new[] { "{", "", "}" }, "", "", 1, Collapse));
            collection.Add(VisualTextLine.Create(LastLine, 2));

            var cachedResult = collection.ConvertToCachedLines(0, 3);

            Assert.AreEqual(cachedResult.Count, 3);
            Assert.AreEqual(cachedResult[0].RenderedContents, FirstLine);
            Assert.AreEqual(cachedResult[1].RenderedContents, Collapse);
            Assert.AreEqual(cachedResult[2].RenderedContents, LastLine);
        }

        [Test]
        public void CacheAndRecreate_ShouldHaveThreeLines() {
            const string FirstLine = "asdf";
            const string Collapse = "{...}";
            const string LastLine = "zxcv";
            string[] collapsedContents = new[] { "{ jh", "as", "cv }" };
            var cachedLines = new List<CachedLine>();

            cachedLines.Add(new CachedSingleLine { Index = 0, RenderedContents = FirstLine });
            cachedLines.Add(
                new CachedCollapsedLine {
                    CollapsedContents = collapsedContents,
                    CollapseRepresentation = Collapse,
                    PrecedingText = "",
                    FollowingText = "",
                    Index = 1,
                    RenderedContents = Collapse
                });
            cachedLines.Add(new CachedSingleLine { Index = 2, RenderedContents = LastLine });

            var visualLines = cachedLines.GetVisualLines().ToList();
            var collapsedLine = visualLines[1] as CollapsedVisualTextLine;

            Assert.IsInstanceOf<SingleVisualTextLine>(visualLines[0]);
            Assert.IsInstanceOf<CollapsedVisualTextLine>(visualLines[1]);
            Assert.IsInstanceOf<SingleVisualTextLine>(visualLines[2]);
            Assert.That(visualLines[0].RenderedText, Is.EqualTo(FirstLine));
            Assert.That(visualLines[1].RenderedText, Is.EqualTo(Collapse));
            Assert.That(visualLines[2].RenderedText, Is.EqualTo(LastLine));
            CollectionAssert.AreEqual(collapsedContents, collapsedLine.CollapsedContent);
        }

    }
}
