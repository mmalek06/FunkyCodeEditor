using System.Linq;
using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.DataStructures;
using CodeEditor.Enums;
using NUnit.Framework;

namespace CodeEditor.Tests.UnitTests.Algorithms {
    [TestFixture]
    public class TextWithBracketsCollapsingUnitTests {
        private TextCollapsingAlgorithm tc;

        private ICollapseRepresentation ra;

        [SetUp]
        public void InitializeTest() {
            tc = new TextCollapsingAlgorithm();
            ra = CollapseRepresentationAlgorithmFactory.GetAlgorithm(FormattingType.BRACKETS);
        }

        [Test]
        public void BracketsInTheSameLineAsText_ShouldHaveTextBeforeAndAfterFold() {
            var text1 = "asdf ";
            var open = "{";
            var text2 = string.Empty;
            var close = "}";
            var text3 = " xzcv";
            var lines = new[] { text1 + open, text2, close + text3 };
            var line = tc.CollapseTextRange(
                new TextRange {
                    StartPosition = new TextPosition(column: 5, line: 0),
                    EndPosition = new TextPosition(column: 0, line: 2) }, 
                lines, 
                ra);

            Assert.AreEqual(text1, string.Join("", line.RenderedText.Take(5)));
            Assert.AreEqual(text3, string.Join("", line.RenderedText.Skip(10)));
        }

        [Test]
        public void OneLineWithBracketsAndText_ActualTextShouldBeEqualToRenderedText() {
            var text1 = "asdf ";
            var open = "{";
            var text2 = string.Empty;
            var close = "}";
            var text3 = " xzcv";
            var lines = new[] { text1 + open + text2 + close + text3 };
            var line = tc.CollapseTextRange(
                new TextRange {
                    StartPosition = new TextPosition(column: 5, line: 0),
                    EndPosition = new TextPosition(column: 6, line: 0) }, 
                lines, 
                ra);
            var stringContents = line.GetStringContents()[0];

            Assert.AreEqual(text1, string.Join("", stringContents.Take(5)));
            Assert.AreEqual(text3, string.Join("", stringContents.Skip(7)));
            Assert.AreEqual(string.Join("", line.RenderedText.Take(5)), string.Join("", stringContents.Take(5)));
            Assert.AreEqual(string.Join("", line.RenderedText.Skip(10)), string.Join("", stringContents.Skip(7)));
        }

        [Test]
        public void BracketsInDifferentLinesThanText_ShouldHaveTextBeforeAndAfterFold() {
            var text1 = "asdf";
            var open = "{";
            var text2 = "";
            var close = "}";
            var text3 = "zxcv";
            var lines = new[] { text1, open, text2, close, text3 };
            var line = tc.CollapseTextRange(
                new TextRange {
                    StartPosition = new TextPosition(column: 0, line: 1),
                    EndPosition = new TextPosition(column: 0, line: 3) }, 
                lines, 
                ra);

            Assert.AreEqual("{...}", line.RenderedText);
        }
    }
}
