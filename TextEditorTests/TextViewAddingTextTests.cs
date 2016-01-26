using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TextEditorTests {
    [TestClass]
    public class TextViewAddingTextTests {

        [TestMethod]
        public void PasteText() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("asdf");

            Assert.AreEqual(1, tv.GetTextLinesCount());
            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void EnterTextCharByChar() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("a");
            tv.EnterText("b");
            tv.EnterText("c");
            tv.EnterText("d");

            Assert.AreEqual(4, tv.GetTextLineLength(0));
            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void EnterTextCharByCharInLines() {
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
            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(2, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void PasteTextInLines() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("asdf");
            tv.EnterText("\r");
            tv.EnterText("\rzxcv");

            Assert.AreEqual(3, tv.GetTextLinesCount());
            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(2, tv.ActiveLineIndex);
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
