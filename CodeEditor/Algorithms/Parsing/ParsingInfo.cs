using CodeEditor.CodeParsing.WordTypes;

namespace CodeEditor.Algorithms.Parsing {
    internal class ParsingInfo {

        #region properties

        public TextType Type { get; }

        public string Text { get; }

        #endregion

        #region constructor

        public ParsingInfo(TextType type, string text) {
            Type = type;
            Text = text;
        }

        #endregion

    }
}
