using System.Windows.Input;
using CodeEditor.DataStructures;

namespace CodeEditor.Messaging {
    public class TextRemovedMessage {

        #region properties

        public Key Key { get; set; }

        public TextPosition OldCaretPosition { get; set; }

        public TextPosition NewCaretPosition { get; set; }

        public string RemovedText { get; set; }

        #endregion

    }
}
