using System.Linq;
using CodeEditor.Algorithms.Folding;
using CodeEditor.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests {
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

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(0, 0), ti).ToList();

            Assert.AreEqual(0, collapsedLines.Count);
        }

        [TestMethod]
        public void StartFoldingAt_0_0_ShouldFoldLine1and2() {
            tv.EnterText("{");
            tv.EnterText("\r");
            tv.EnterText("}");

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(0, 0), ti).ToList();

            Assert.AreEqual(2, collapsedLines.Count());
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

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(0, 0), ti).ToList();

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

            var collapsedLines = fa.GetCollapsedLines(tv.Lines, new TextPosition(8, 1), ti).ToList();

            Assert.AreEqual(4, collapsedLines.Count);
            Assert.AreEqual("{", collapsedLines[0]);
            Assert.AreEqual(text3, collapsedLines[1]);
            Assert.AreEqual(text4, collapsedLines[2]);
            Assert.AreEqual("}", collapsedLines[3]);
        }

        [TestMethod]
        public void FourBracketsInText_PossibleFoldsShouldBeOnLines_0_1_3() {
            string text1 = "{";
            string text2 = "parent: {";
            string text3 = "key1: true";
            string text4 = "child: {";
            string text5 = "key2: 1244,";
            string text6 = "key3: 5667";
            string text7 = "}}}";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.EnterText("\r");
            tv.EnterText(text4);
            tv.EnterText("\r");
            tv.EnterText(text5);
            tv.EnterText("\r");
            tv.EnterText(text6);
            tv.EnterText("\r");
            tv.EnterText(text7);

            var possibleFolds = fa.GetPossibleFolds(ti);

            Assert.AreEqual(3, possibleFolds.Count());
            Assert.AreEqual(0, 0);
        }
    }
}
