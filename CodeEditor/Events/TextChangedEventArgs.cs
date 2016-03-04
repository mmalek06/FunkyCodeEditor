using System;

namespace CodeEditor.Events {
    public class TextChangedEventArgs : EventArgs {
        public string Text { get; set; }
        public int CurrentColumn { get; set; }
        public int CurrentLine { get; set; }
    }
}
