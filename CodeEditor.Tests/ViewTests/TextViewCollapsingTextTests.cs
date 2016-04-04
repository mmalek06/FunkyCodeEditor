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
            tv.HandleTextFolding(GetFoldClickedMessage(0, 1, 0, 3));

            Assert.AreEqual(text1, ti.Lines[0]);
            Assert.AreEqual(collapseRepresentation, ti.Lines[1]);
            Assert.AreEqual(text3, ti.Lines[2]);
        }

        private FoldClickedMessage GetFoldClickedMessage(int startingCol, int startingLine, int endingCol, int endingLine) {
            return new FoldClickedMessage {
                State = Algorithms.Folding.FoldingStates.FOLDED,
                Area = new TextPositionsPair {
                    StartPosition = new TextPosition(startingCol, startingLine),
                    EndPosition = new TextPosition(endingCol, endingLine)
                },
                OpeningTag = "{",
                ClosingTag = "}"
            };
        }
    }
}
