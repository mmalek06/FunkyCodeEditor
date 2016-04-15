using CodeEditor.Core.DataStructures;
using CodeEditor.Views.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests.ScenarioTests.Views {
    [TestClass]
    public class TextViewAddingTextTests {
        private TextView tv;
        private TextView.TextViewInfo ti;

        [TestInitialize]
        public void InitializeTest() {
            tv = new TextView();
            ti = TextView.TextViewInfo.GetInstance(tv);
        }

        [TestMethod]
        public void FourCharactersEnteredMovedCaretToTheMiddle_AfterBreakLineLinesShouldBeText1Text2() {
            string text1 = "as";
            string text2 = "df";

            tv.EnterText(text1 + text2);
            tv.HandleCaretMove(this, new Events.CaretMovedEventArgs {
                NewPosition = new TextPosition(line: 0, column: 2)
            });
            tv.EnterText("\r");

            Assert.AreEqual(text1, ti.GetLine(0));
            Assert.AreEqual(text2, ti.GetLine(1));
        }

        [TestMethod]
        public void CreatedTwoLinesMovedCaretToTheFirstOneEnteredEmptyLine_LinesShouldBe4() {
            tv.EnterText("a");
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText("z");
            tv.HandleCaretMove(this, new Events.CaretMovedEventArgs {
                NewPosition = new TextPosition(line: 1, column: 0)
            });
            tv.EnterText("\r");

            Assert.AreEqual(4, ti.LinesCount);
            Assert.AreEqual("a", ti.GetLine(0));
            Assert.AreEqual(string.Empty, ti.GetLine(1));
            Assert.AreEqual(string.Empty, ti.GetLine(2));
            Assert.AreEqual("z", ti.GetLine(3));
        }

    }
}
