using CodeEditor.DataStructures;

namespace CodeEditor.TextProperties {
    internal class CharInfo {

        #region properties

        public bool IsCharacter { get; set; }

        public string Text { get; set; }

        public TextPosition NextCharPosition { get; set; }

        public TextPosition PrevCharPosition { get; set; }

        #endregion

    }
}
