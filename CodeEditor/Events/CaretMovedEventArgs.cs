using System;
using CodeEditor.DataStructures;

namespace CodeEditor.Events {
    public class CaretMovedEventArgs : EventArgs {
        public TextPosition NewPosition { get; set; }
        public TextPosition OldPosition { get; set; }
    }
}
