using CodeEditor.Core.DataStructures;

namespace CodeEditor.TextProperties {
    internal class CharInfo {

        #region properties

        public bool IsCharacter { get; set; }

        public char Character { get; set; }

        public TextPosition NextCharPosition { get; set; }

        public TextPosition PrevCharPosition { get; set; }

        #endregion

    }
}
