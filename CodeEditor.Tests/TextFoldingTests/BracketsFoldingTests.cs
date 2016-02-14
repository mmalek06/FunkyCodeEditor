using System.Linq;
using CodeEditor.Algorithms.Folding;
using CodeEditor.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests.TextFoldingTests {
    [TestClass]
    public class BracketsFoldingTests {

        private BracketsFoldingAlgorithm fa;
        private Views.TextView.View tv;
        private Views.TextView.TextInfo ti;

        [TestInitialize]
        public void InitializeTest() {
            tv = new Views.TextView.View();
            ti = new Views.TextView.TextInfo(tv);
            fa = new BracketsFoldingAlgorithm();
        }

        [TestMethod]
        public void OpeningAndClosingBracketEnteredInTheSameLine_ShouldBeNoFolding() {
            tv.EnterText("{");
            tv.EnterText("}");

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(0, 0), ti);

            Assert.AreEqual(0, collapsedLines.Count);
        }

        [TestMethod]
        public void OpeningAndClosingBracketEnteredInSeparateLines_ShouldFoldLine1and2() {
            tv.EnterText("{");
            tv.EnterText("\r");
            tv.EnterText("}");

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(0, 0), ti);

            Assert.AreEqual(2, collapsedLines.Count);
            Assert.AreEqual("}", collapsedLines[1]);
        }

        [TestMethod]
        public void ThreeBracketPairsPresent_ShouldFoldThreeTimes() {

        }
    }
}
