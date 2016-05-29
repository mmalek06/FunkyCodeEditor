using CodeEditor.DataStructures;

namespace CodeEditor.Algorithms.Selection {
    internal class SelectionInfo {

        #region properties

        public TextPosition StartPosition { get; set; }

        public TextPosition EndPosition { get; set; }

        public TextPosition CursorPosition { get; set; }

        #endregion

    }
}
