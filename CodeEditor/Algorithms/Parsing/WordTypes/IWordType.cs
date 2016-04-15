namespace CodeEditor.Algorithms.Parsing.WordTypes {
    internal interface IWordType {

        #region properties

        TextType Type { get; }

        #endregion

        #region public methods

        bool IsMatch(string word);

        #endregion

    }
}
