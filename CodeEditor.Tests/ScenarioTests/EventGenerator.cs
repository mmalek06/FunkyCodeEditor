using System;
using System.Windows.Input;
using System.Windows.Interop;
using CodeEditor.DataStructures;
using CodeEditor.Events;

namespace CodeEditor.Tests.ScenarioTests {
    public static class EventGenerator {

        #region public methods

        public static KeyEventArgs CreateKeyEventArgs(Key key) =>
            new KeyEventArgs(Keyboard.PrimaryDevice, new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero), 0, key) {
                RoutedEvent = Keyboard.KeyDownEvent
            };

        public static TextCompositionEventArgs CreateTextCompositionEventArgs(string text) =>
            new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, Keyboard.FocusedElement, text));

        public static CaretMovedEventArgs CreateCaretMovedEventArgs(int col, int line) =>
            new CaretMovedEventArgs { NewPosition = new TextPosition(column: col, line: line) };

        #endregion

    }
}
