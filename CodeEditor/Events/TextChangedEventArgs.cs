using System;

namespace CodeEditor.Events {
    public class TextChangedEventArgs : EventArgs {
        public int CurrentColumn { get; set; }
        public int CurrentLine { get; set; }
    }
}
