using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Input;
using System.Windows.Interop;
using TextEditor.Events;

namespace TextEditorTests {
    [TestClass]
    public class TextViewRemovingTextTests {
        [TestMethod]
        public void CharEnteredAndBackspaced_CursorShouldBeAt00() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("s");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void EmptyLineEnteredAndBackspaced_CursorShouldBeAt00() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("\r");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void EmptyLineEnteredAndBackspaced_LinesShouldBe1() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("\r");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void EmptyLineEnteredAndDelPressed_CursorShouldBeAt00() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("\r");
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(0, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void EmptyLineEnteredAndDelPressed_LinesShouldBe1() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("\r");
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredDelPressedAtTheEndOfFirstOne_LinesShouldBe1() {
            var tv = new TextEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredDelPressedAtTheEndOfFirstOne_CursorShouldBeAt40() {
            var tv = new TextEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecond_LinesShouldBe1() {
            var tv = new TextEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 1));
            tv.RemoveText(Key.Back);

            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecond_CursorShouldBeAt40() {
            var tv = new TextEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 1));
            tv.RemoveText(Key.Back);

            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void FourLinesEnteredDeletePressedAtTheEndOfFirst_LinesShouldBe3() {
            var tv = new TextEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";
            string text3 = "qwer";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(3, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void FourLinesEnteredDeletePressedAtTheEndOfFirst_LinesShouldBe2() {
            var tv = new TextEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";
            string text3 = "qwer";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.EnterText("\r");
            tv.EnterText(text3);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(2, tv.GetTextLinesCount());
        }

        private KeyEventArgs CreateKeyEventArgs(Key key) {
            return new KeyEventArgs(Keyboard.PrimaryDevice, new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero), 0, key) {
                RoutedEvent = Keyboard.KeyDownEvent
            };
        }

        private TextCompositionEventArgs CreateTextCompositionEventArgs(string text) {
            return new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, Keyboard.FocusedElement, text));
        }

        private CaretMovedEventArgs CreateCaretMovedEventArgs(int col, int line) {
            return new CaretMovedEventArgs { NewPosition = new TextEditor.DataStructures.TextPosition { Column = col, Line = line } };
        }
    }
}
