using CodeEditor.Enums;

namespace CodeEditor.CodeParsing.WordTypes {
    internal class StdWordType : IWordType {

        #region properties

        public TextType Type => TextType.STANDARD;

        public SupportedLanguages Language { get; set; }

        public IDefinitionLoader DefinitionLoader { get; set; }

        #endregion

        #region public methods

        public bool IsMatch(string word) => true;

        #endregion

    }
}
