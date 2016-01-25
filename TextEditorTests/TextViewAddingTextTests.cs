using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TextEditorTests {
    [TestClass]
    public class TextViewAddingTextTests {

        [TestMethod]
        public void LinesCountShouldBe_1() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("asdf");

            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void CursorShouldBeOn_0_4() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("asdf");

            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void SingleCharAtATime_LinesCountShouldBe_3() {
            var tv = new TextEditor.Views.TextView.View();

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

            Assert.AreEqual(3, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void SingleCharAtATime_CursorShouldBeOn_2_4() {
            var tv = new TextEditor.Views.TextView.View();

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

            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(2, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void WholeLines_LinesShouldBe_3() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("asdf");
            tv.EnterText("\r");
            tv.EnterText("\rzxcv");

            Assert.AreEqual(3, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void WholeLines_CursorShouldBeOn_2_4() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("asdf");
            tv.EnterText("\r");
            tv.EnterText("\rzxcv");

            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(2, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void SingleCharAtATime_LineShouldBe_4_CharsLong() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("a");
            tv.EnterText("b");
            tv.EnterText("c");
            tv.EnterText("d");

            Assert.AreEqual(4, tv.GetTextLineLength(0));
        }

        [TestMethod]
        public void BreakLineInTheMiddle() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("asdf");
            tv.HandleCaretMove(this, new TextEditor.Events.CaretMovedEventArgs {
                NewPosition = new TextEditor.DataStructures.TextPosition {
                    Line = 0,
                    Column = 2
                }
            });
            tv.EnterText("\r");

            Assert.AreEqual("as", tv.GetTextLine(0));
            Assert.AreEqual("df", tv.GetTextLine(1));
        }

        [TestMethod]
        public void AddEmptyLine() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("a");
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText("z");
            tv.HandleCaretMove(this, new TextEditor.Events.CaretMovedEventArgs {
                NewPosition = new TextEditor.DataStructures.TextPosition {
                    Line = 1,
                    Column = 0
                }
            });
            tv.EnterText("\r");

            Assert.AreEqual(4, tv.GetTextLinesCount());
            Assert.AreEqual("a", tv.GetTextLine(0));
            Assert.AreEqual(string.Empty, tv.GetTextLine(1));
            Assert.AreEqual(string.Empty, tv.GetTextLine(2));
            Assert.AreEqual("z", tv.GetTextLine(3));
        }
    }
}
