using CodeEditor.Core.DataStructures;

namespace CodeEditor.Messaging {
    internal class TextAddedMessage {

        #region properties

        public string Text { get; set; }

        public TextPosition Position { get; set; }

        #endregion

    }
}
