using CodeEditor.Algorithms.Folding;
using CodeEditor.Core.DataStructures;
using CodeEditor.Messaging;
using CodeEditor.Views.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests.ScenarioTests.Views {
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
        public void EnterFourLinesFoldInFirstAndLast_StateAfterCollapseAndExpandShouldNotChange() {
            string text1 = "asdf {}";
            string text2 = "b";
            string text3 = "{}";
            var foldMessage = GetFoldClickedMessage(5, 1, 6, 1, FoldingStates.FOLDED);
            var unfoldMessage = GetFoldClickedMessage(5, 1, 6, 1, FoldingStates.EXPANDED);

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.HandleTextFolding(foldMessage);
            tv.HandleTextFolding(unfoldMessage);

            var renderedLines = ti.GetScreenLines();

            Assert.AreEqual(text1, renderedLines[0]);
            Assert.AreEqual(text2, renderedLines[2]);
            Assert.AreEqual(text3, renderedLines[3]);
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
