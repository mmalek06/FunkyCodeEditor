namespace CodeEditor.Algorithms.Parsing.WordTypes {
    internal class CollapseWordType : IWordType {

        #region properties

        public TextType Type => TextType.COLLAPSE;

        #endregion

        #region public methods

        public bool IsMatch(string word) => false;

        #endregion

    }
}
