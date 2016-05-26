using System.Linq;
using CodeEditor.Algorithms.TextManipulation;
using CodeEditor.Core.DataStructures;
using CodeEditor.Core.Enums;
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
            string text1 = "asdf ";
            string open = "{";
            string text2 = string.Empty;
            string close = "}";
            string text3 = " xzcv";
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
            string text1 = "asdf ";
            string open = "{";
            string text2 = string.Empty;
            string close = "}";
            string text3 = " xzcv";
            var lines = new[] { text1 + open + text2 + close + text3 };
            var line = tc.CollapseTextRange(
                new TextRange {
                    StartPosition = new TextPosition(column: 5, line: 0),
                    EndPosition = new TextPosition(column: 6, line: 0) }, 
                lines, 
                ra);
            string stringContents = line.GetStringContents()[0];

            Assert.AreEqual(text1, string.Join("", stringContents.Take(5)));
            Assert.AreEqual(text3, string.Join("", stringContents.Skip(7)));
            Assert.AreEqual(string.Join("", line.RenderedText.Take(5)), string.Join("", stringContents.Take(5)));
            Assert.AreEqual(string.Join("", line.RenderedText.Skip(10)), string.Join("", stringContents.Skip(7)));
        }

        [Test]
        public void BracketsInDifferentLinesThanText_ShouldHaveTextBeforeAndAfterFold() {
            string text1 = "asdf";
            string open = "{";
            string text2 = "";
            string close = "}";
            string text3 = "zxcv";
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
