using System.Windows.Input;
using CodeEditor.DataStructures;

namespace CodeEditor.Messaging {
    internal class LineRemovedMessage {

        #region properties

        public Key Key { get; set; }

        public TextPosition Position { get; set; }

        public int LineLength { get; set; }

        #endregion

    }
}