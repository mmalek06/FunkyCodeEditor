using System.Windows.Input;
using CodeEditor.DataStructures;

namespace CodeEditor.Messaging {
    internal class TextRemovedMessage {

        #region properties

        public Key Key { get; set; }

        public TextPosition Position { get; set; }

        #endregion

    }
}
