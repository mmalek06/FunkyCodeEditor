using System;
using TextEditor.DataStructures;

namespace TextEditor.Events {
    public class CaretMovedEventArgs : EventArgs {
        public TextPosition NewPosition { get; set; }
        public TextPosition OldPosition { get; set; }
    }
}
