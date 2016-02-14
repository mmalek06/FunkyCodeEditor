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
        public void StartFoldingAt_0_0_ShouldBeNoFolding() {
            tv.EnterText("{");
            tv.EnterText("}");

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(0, 0), ti);

            Assert.AreEqual(0, collapsedLines.Count);
        }

        [TestMethod]
        public void StartFoldingAt_0_0_ShouldFoldLine1and2() {
            tv.EnterText("{");
            tv.EnterText("\r");
            tv.EnterText("}");

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(0, 0), ti);

            Assert.AreEqual(2, collapsedLines.Count);
            Assert.AreEqual("}", collapsedLines[1]);
        }

        [TestMethod]
        public void StartFoldingAt_0_0_ShouldFoldTopToBottom() {
            string text1 = "{I saw";
            string text2 = "Susie in{";
            string text3 = "Shoe";
            string text4 = "{Shine shop}";
            string text5 = "}}";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.EnterText("\r");
            tv.EnterText(text4);
            tv.EnterText("\r");
            tv.EnterText(text5);

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(0, 0), ti);

            Assert.AreEqual(5, collapsedLines.Count);
            Assert.AreEqual(text1, collapsedLines[0]);
            Assert.AreEqual(text2, collapsedLines[1]);
            Assert.AreEqual(text3, collapsedLines[2]);
            Assert.AreEqual(text4, collapsedLines[3]);
            Assert.AreEqual(text5, collapsedLines[4]);
        }

        [TestMethod]
        public void StartFoldingAt_1_8_ShouldFoldFromLine1ToTheLastLine() {
            string text1 = "{I saw";
            string text2 = "Susie in{";
            string text3 = "Shoe";
            string text4 = "{Shine shop}";
            string text5 = "}}";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.EnterText("\r");
            tv.EnterText(text4);
            tv.EnterText("\r");
            tv.EnterText(text5);

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(8, 1), ti);

            Assert.AreEqual(4, collapsedLines.Count);
            Assert.AreEqual("{", collapsedLines[0]);
            Assert.AreEqual(text3, collapsedLines[1]);
            Assert.AreEqual(text4, collapsedLines[2]);
            Assert.AreEqual("}", collapsedLines[3]);
        }
    }
}
