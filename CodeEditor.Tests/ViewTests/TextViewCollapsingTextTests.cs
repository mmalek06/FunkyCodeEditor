using System.Linq;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.Views.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests.ViewTests {
    [TestClass]
    public class TextViewCollapsingTextTests {
        private TextView tv;
        private TextView.TextViewInfo ti;

        [TestInitialize]
        public void InitializeTest() {
            tv = new TextView();
            ti = TextView.TextViewInfo.GetInstance(tv);
        }

        [TestMethod]
        public void EnterOpeningAndClosingBracket_LineStateShouldNotChange() {
            string text1 = "{}";

            tv.EnterText(text1);
            tv.HandleTextFolding(GetFoldClickedMessage(0, 0, 1, 0, FoldingStates.FOLDED));
            tv.HandleTextFolding(GetFoldClickedMessage(0, 0, 1, 0, FoldingStates.EXPANDED));

            var renderedLines = ti.GetScreenLines();

            Assert.AreEqual(renderedLines[0], text1);
        }

        [TestMethod]
        public void EnteredFiveLines_AfterCollapseShouldBeText1CollapseText3() {
            string text1 = "asdf";
            string open = "{";
            string close = "}";
            string text3 = "zxcv";
            string collapseRepresentation = "{...}";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(open);
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText(close);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.HandleTextFolding(GetFoldClickedMessage(0, 1, 0, 3, FoldingStates.FOLDED));

            var renderedLines = ti.GetScreenLines();
            var actualLines = ti.GetActualLines();

            Assert.AreEqual(text1, renderedLines[0]);
            Assert.AreEqual(collapseRepresentation, renderedLines[1]);
            Assert.AreEqual(text3, renderedLines[2]);
            Assert.AreEqual(text1, actualLines[0]);
            Assert.AreEqual(open, actualLines[1]);
            Assert.AreEqual("", actualLines[2]);
            Assert.AreEqual(close, actualLines[3]);
            Assert.AreEqual(text3, actualLines[4]);
        }

        [TestMethod]
        public void CreateTwoSimpleFoldsAndClickThreeTimes_StateAfterCollapseAndExpandShouldNotChange() {
            string text1 = "{}";
            string text2 = "{}";
            var foldMessage = GetFoldClickedMessage(0, 0, 1, 0, FoldingStates.FOLDED);
            var unfoldMessage = GetFoldClickedMessage(0, 0, 1, 0, FoldingStates.EXPANDED);

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleTextFolding(foldMessage);
            tv.HandleTextFolding(unfoldMessage);
            tv.HandleTextFolding(foldMessage);
            tv.HandleTextFolding(unfoldMessage);
            tv.HandleTextFolding(foldMessage);
            tv.HandleTextFolding(unfoldMessage);

            var renderedLines = ti.GetScreenLines();

            Assert.AreEqual(text1, renderedLines[0]);
            Assert.AreEqual(text2, renderedLines[1]);
        }

        [TestMethod]
        public void CreateTwoFolds_StateAfterCollapseAndExpandShouldNotChange() {
            string text1 = "asdf {";
            string text2 = "";
            string text3 = "} qwer";
            string text4 = "{";
            string text5 = "";
            string text6 = "}";
            string text7 = "xzcv";

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
            tv.EnterText("\r");

            tv.HandleTextFolding(GetFoldClickedMessage(5, 0, 0, 2, FoldingStates.FOLDED));
            tv.HandleTextFolding(GetFoldClickedMessage(5, 0, 0, 2, FoldingStates.EXPANDED));

            var renderedLines = ti.GetScreenLines();
            var actualLines = ti.GetActualLines();

            Assert.IsTrue(Enumerable.SequenceEqual(renderedLines, actualLines));
        }

        private FoldClickedMessage GetFoldClickedMessage(int startingCol, int startingLine, int endingCol, int endingLine, FoldingStates state) {
            return new FoldClickedMessage {
                State = state,
                Area = new TextRange {
                    StartPosition = new TextPosition(startingCol, startingLine),
                    EndPosition = new TextPosition(endingCol, endingLine)
                },
                OpeningTag = "{",
                ClosingTag = "}"
            };
        }
    }
}
