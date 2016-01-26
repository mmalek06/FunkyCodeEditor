using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Input;
using System.Windows.Interop;
using TextEditor.Commands;

namespace TextEditorTests {
    [TestClass]
    public class TextViewRemovingTextTests {
        [TestMethod]
        public void BackspaceWhenOneCharacter() {
            var tv = new TextEditor.Views.TextView.View();
            var enterTextCmd = new EnterTextCommand(tv);
            var removeTextCmd = new RemoveTextCommand(tv);
            var argument = CreateTextCompositionEventArgs("a");

            enterTextCmd.Execute(argument);

            Assert.AreEqual(true, removeTextCmd.CanExecute(CreateKeyEventArgs(Key.Back)));

            removeTextCmd.Execute(CreateKeyEventArgs(Key.Back));

            Assert.AreEqual(0, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
        }

        [TestMethod]
        public void BackspaceWhenTwoLines() {
            var tv = new TextEditor.Views.TextView.View();
            var enterTextCmd = new EnterTextCommand(tv);
            var removeTextCmd = new RemoveTextCommand(tv);

            enterTextCmd.Execute(CreateTextCompositionEventArgs("\r"));

            Assert.AreEqual(2, tv.GetTextLinesCount());
            Assert.AreEqual(true, removeTextCmd.CanExecute(CreateKeyEventArgs(Key.Back)));

            removeTextCmd.Execute(CreateKeyEventArgs(Key.Back));

            Assert.AreEqual(0, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        [TestMethod]
        public void DeleteWhenTwoLines() {
            var tv = new TextEditor.Views.TextView.View();
            var enterTextCmd = new EnterTextCommand(tv);
            var removeTextCmd = new RemoveTextCommand(tv);

            enterTextCmd.Execute(CreateTextCompositionEventArgs("\r"));

            tv.HandleCaretMove(this, new TextEditor.Events.CaretMovedEventArgs { NewPosition = new TextEditor.DataStructures.TextPosition { Column = 0, Line = 0 } });

            Assert.AreEqual(2, tv.GetTextLinesCount());
            Assert.AreEqual(true, removeTextCmd.CanExecute(CreateKeyEventArgs(Key.Delete)));

            removeTextCmd.Execute(CreateKeyEventArgs(Key.Delete));

            Assert.AreEqual(0, tv.ActiveColumnIndex);
            Assert.AreEqual(0, tv.ActiveLineIndex);
            Assert.AreEqual(1, tv.GetTextLinesCount());
        }

        private KeyEventArgs CreateKeyEventArgs(Key key) {
            return new KeyEventArgs(Keyboard.PrimaryDevice, new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero), 0, key) {
                RoutedEvent = Keyboard.KeyDownEvent
            };
        }

        private TextCompositionEventArgs CreateTextCompositionEventArgs(string text) {
            return new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, Keyboard.FocusedElement, text));
        }
    }
}
