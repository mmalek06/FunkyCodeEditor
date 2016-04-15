namespace CodeEditor.Algorithms.Parsing.WordTypes {
    internal class KeywordWordType : IWordType {

        #region properties

        public TextType Type => TextType.KEYWORD;

        #endregion

        #region public methods

        public bool IsMatch(string word) => false;

        #endregion

    }
}
