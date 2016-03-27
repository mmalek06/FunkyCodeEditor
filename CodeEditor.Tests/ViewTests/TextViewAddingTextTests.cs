using CodeEditor.Core.DataStructures;
using CodeEditor.Views.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeEditor.Tests {
    [TestClass]
    public class TextViewAddingTextTests {
        private TextView tv;

        [TestInitialize]
        public void InitializeTest() {
            tv = new TextView();
        }

        [TestMethod]
        public void FourCharsPasted_LinesShouldBe1() {
            tv.EnterText("asdf");

            Assert.AreEqual(1, tv.LinesCount);
        }

        [TestMethod]
        public void FourCharsPasted_CursorShouldBe40() {
            tv.EnterText("asdf");

            Assert.AreEqual(4, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void FourCharsEntered_LineLengthShouldBe4() {
            tv.EnterText("a");
            tv.EnterText("b");
            tv.EnterText("c");
            tv.EnterText("d");

            Assert.AreEqual(4, tv.GetLineLength(0));
        }

        [TestMethod]
        public void FourCharsEntered_CursorShouldBe40() {
            tv.EnterText("a");
            tv.EnterText("b");
            tv.EnterText("c");
            tv.EnterText("d");
            
            Assert.AreEqual(4, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void ThreeLinesAddedCharsEntered_LinesShouldBe3() {
            tv.EnterText("a");
            tv.EnterText("s");
            tv.EnterText("d");
            tv.EnterText("f");
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText("z");
            tv.EnterText("x");
            tv.EnterText("c");
            tv.EnterText("v");

            Assert.AreEqual(3, tv.LinesCount);
        }

        [TestMethod]
        public void ThreeLinesAddedCharsEntered_CursorShouldBeAt42() {
            tv.EnterText("a");
            tv.EnterText("s");
            tv.EnterText("d");
            tv.EnterText("f");
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText("z");
            tv.EnterText("x");
            tv.EnterText("c");
            tv.EnterText("v");

            Assert.AreEqual(4, tv.ActivePosition.Column);
            Assert.AreEqual(2, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void ThreeLinesPasted_LinesShouldBe3() {
            tv.EnterText("asdf");
            tv.EnterText("\r");
            tv.EnterText("\rzxcv");

            Assert.AreEqual(3, tv.LinesCount);
        }

        [TestMethod]
        public void ThreeLinesPasted_CursorShouldBeAt42() {
            tv.EnterText("asdf");
            tv.EnterText("\r");
            tv.EnterText("\rzxcv");

            Assert.AreEqual(4, tv.ActivePosition.Column);
            Assert.AreEqual(2, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void LineBreakInTheMiddle_LinesShouldBeText1Text2() {
            string text1 = "as";
            string text2 = "df";

            tv.EnterText(text1 + text2);
            tv.HandleCaretMove(this, new Events.CaretMovedEventArgs {
                NewPosition = new TextPosition(line: 0, column: 2)
            });
            tv.EnterText("\r");

            Assert.AreEqual(text1, tv.GetLine(0));
            Assert.AreEqual(text2, tv.GetLine(1));
        }

        [TestMethod]
        public void EmptyLineAddedAfterSecondLine_LinesShouldBeOk() {
            tv.EnterText("a");
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText("z");
            tv.HandleCaretMove(this, new Events.CaretMovedEventArgs {
                NewPosition = new TextPosition(line: 1, column: 0)
            });
            tv.EnterText("\r");

            Assert.AreEqual(4, tv.LinesCount);
            Assert.AreEqual("a", tv.GetLine(0));
            Assert.AreEqual(string.Empty, tv.GetLine(1));
            Assert.AreEqual(string.Empty, tv.GetLine(2));
            Assert.AreEqual("z", tv.GetLine(3));
        }
    }
}
