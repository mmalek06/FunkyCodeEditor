namespace CodeEditor.Core.DataStructures {
    public class TextRange {

        #region properties

        public TextPosition StartPosition { get; set; }

        public TextPosition EndPosition { get; set; }

        #endregion

        #region public methods

        public bool Contains(TextPosition position) => position >= StartPosition && position <= EndPosition;

        #endregion

    }
}
