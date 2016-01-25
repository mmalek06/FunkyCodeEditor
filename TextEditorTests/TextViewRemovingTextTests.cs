using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Input;
using System.Windows.Interop;
using TextEditor.Commands;

namespace TextEditorTests {
    [TestClass]
    public class TextViewRemovingTextTests {
        [TestMethod]
        public void BackspaceWhenNoLines() {
            var tv = new TextEditor.Views.TextView.View();
            var removeCmd = new RemoveTextCommand(tv);

            Assert.AreEqual(false, removeCmd.CanExecute(MockKeyEventArgs(Key.Back)));
        }

        [TestMethod]
        public void DeleteWhenNoLines() {
            var tv = new TextEditor.Views.TextView.View();
            var removeCmd = new RemoveTextCommand(tv);

            Assert.AreEqual(false, removeCmd.CanExecute(MockKeyEventArgs(Key.Delete)));
        }

        [TestMethod]
        public void BackspaceWhenOneCharacter() {
            var tv = new TextEditor.Views.TextView.View();
            var enterTextCmd = new EnterTextCommand(tv);
            var removeTextCmd = new RemoveTextCommand(tv);
            var argument = MockTextCompositionEventArgs("a");

            enterTextCmd.Execute(argument);

            Assert.AreEqual(true, removeTextCmd.CanExecute(MockKeyEventArgs(Key.Back)));
        }

        private KeyEventArgs MockKeyEventArgs(Key key) {
            return new KeyEventArgs(Keyboard.PrimaryDevice, new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero), 0, key) {
                RoutedEvent = Keyboard.KeyDownEvent
            };
        }

        private TextCompositionEventArgs MockTextCompositionEventArgs(string text) {
            return new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, Keyboard.FocusedElement, text));
        }
    }
}
