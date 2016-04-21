using CodeEditor.Core.DataStructures;

namespace CodeEditor.Messaging {
    internal class TextAddedMessage {

        #region properties

        public string Text { get; set; }

        public TextPosition PrevCaretPosition { get; set; }

        public TextPosition NewCaretPosition { get; set; }

        #endregion

    }
}
