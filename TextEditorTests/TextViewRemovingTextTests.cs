using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Input;
using System.Windows.Interop;
using TextEditor.Commands;
using TextEditor.Events;

namespace TextEditorTests {
    [TestClass]
    public class TextViewRemovingTextTests {
        [TestMethod]
        public void BackspaceWhenOneCharacter() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("s");
            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void BackspaceWhenTwoEmptyLines() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("\r");

            Assert.AreEqual(2, tv.GetTextLinesCount());

            tv.RemoveText(Key.Back);

            Assert.AreEqual(0, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void DeleteWhenTwoEmptyLines() {
            var tv = new TextEditor.Views.TextView.View();

            tv.EnterText("\r");
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 0));

            Assert.AreEqual(2, tv.GetTextLinesCount());

            tv.RemoveText(Key.Delete);

            Assert.AreEqual(0, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void DeleteWhenTwoNonEmptyLines() {
            var tv = new TextEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(4, 0));
            tv.RemoveText(Key.Delete);

            Assert.AreEqual(1, tv.GetTextLinesCount());
            Assert.AreEqual((text1 + text2).Length, tv.GetTextLineLength(0));
            Assert.AreEqual((text1 + text2), tv.GetTextLine(0));
            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void BackspaceWhenTwoNonEmptyLines() {
            var tv = new TextEditor.Views.TextView.View();
            string text1 = "asdf";
            string text2 = "zxcv";

            tv.EnterText(text1);
            tv.EnterText("\r");
            tv.EnterText(text2);
            tv.HandleCaretMove(this, CreateCaretMovedEventArgs(0, 1));
            tv.RemoveText(Key.Back);

            Assert.AreEqual(1, tv.GetTextLinesCount());
            Assert.AreEqual((text1 + text2).Length, tv.GetTextLineLength(0));
            Assert.AreEqual((text1 + text2), tv.GetTextLine(0));
            Assert.AreEqual(4, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
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
