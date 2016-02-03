using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Input;
using System.Windows.Interop;
using CodeEditor.Events;

namespace CodeEditor.Tests {
    [TestClass]
    public class TextViewRemovingTextTests {
        [TestMethod]
        public void CharEnteredAndBackspaced_CursorShouldBeAt00() {
            var tv = new CodeEditor.Views.TextView.View();

            tv.EnterText("s");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void EmptyLineEnteredAndBackspaced_CursorShouldBeAt00() {
            var tv = new CodeEditor.Views.TextView.View();
            var ti = new CodeEditor.Views.TextView.TextInfo(tv);

            tv.EnterText("\r");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void EmptyLineEnteredAndBackspaced_LinesShouldBe1() {
            var tv = new CodeEditor.Views.TextView.View();
            var ti = new CodeEditor.Views.TextView.TextInfo(tv);

            tv.EnterText("\r");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void EmptyLineEnteredAndDelPressed_CursorShouldBeAt00() {
            var tv = new CodeEditor.Views.TextView.View();

            tv.EnterText("\r");
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(0, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void EmptyLineEnteredAndDelPressed_LinesShouldBe1() {
            var tv = new CodeEditor.Views.TextView.View();
            var ti = new CodeEditor.Views.TextView.TextInfo(tv);

            tv.EnterText("\r");
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredDelPressedAtTheEndOfFirstOne_LinesShouldBe1() {
            var tv = new CodeEditor.Views.TextView.View();
            var ti = new CodeEditor.Views.TextView.TextInfo(tv);
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredDelPressedAtTheEndOfFirstOne_CursorShouldBeAt40() {
            var tv = new CodeEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(4, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecond_LinesShouldBe1() {
            var tv = new CodeEditor.Views.TextView.View();
            var ti = new CodeEditor.Views.TextView.TextInfo(tv);
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 1));
            tv.RemoveText(Key.Back);

            Assert.AreEqual(1, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void TwoNonEmptyLinesEnteredBackspacePressedAtTheBeginningOfSecond_CursorShouldBeAt40() {
            var tv = new CodeEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 1));
            tv.RemoveText(Key.Back);

            Assert.AreEqual(4, tv.ActivePosition.Column);
            Assert.AreEqual(0, tv.ActivePosition.Line);
        }

        [TestMethod]
        public void FourLinesEnteredDeletePressedAtTheEndOfFirst_LinesShouldBe3() {
            var tv = new CodeEditor.Views.TextView.View();
            var ti = new CodeEditor.Views.TextView.TextInfo(tv);
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

            Assert.AreEqual(3, ti.GetTextLinesCount());
        }

        [TestMethod]
        public void FourLinesEnteredDeletePressedAtTheEndOfFirst_LinesShouldBe2() {
            var tv = new CodeEditor.Views.TextView.View();
            var ti = new CodeEditor.Views.TextView.TextInfo(tv);
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

            Assert.AreEqual(2, ti.GetTextLinesCount());
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
            return new CaretMovedEventArgs { NewPosition = new CodeEditor.DataStructures.TextPosition { Column = col, Line = line } };
        }
    }
}
