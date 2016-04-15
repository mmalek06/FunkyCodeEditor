namespace CodeEditor.Algorithms.Parsing.WordTypes {
    internal class StdWordType : IWordType {

        #region properties

        public TextType Type => TextType.STANDARD;

        #endregion

        #region public methods

        public bool IsMatch(string word) => true;

        #endregion

    }
}
