namespace CodeEditor.Algorithms.Parsing.WordTypes {
    internal class DefinitionWordType : IWordType {

        #region properties

        public TextType Type => TextType.DEFINITION;

        #endregion

        #region public methods

        public bool IsMatch(string word) => false;

        #endregion

    }
}
