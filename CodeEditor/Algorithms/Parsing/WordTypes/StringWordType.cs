namespace CodeEditor.Algorithms.Parsing.WordTypes {
    internal class StringWordType : IWordType {

        #region properties

        public TextType Type => TextType.STRING;

        #endregion

        #region public methods

        public bool IsMatch(string word) => false;

        #endregion

    }
}
