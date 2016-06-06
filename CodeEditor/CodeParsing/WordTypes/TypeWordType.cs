using CodeEditor.Enums;

namespace CodeEditor.CodeParsing.WordTypes {
    internal class TypeWordType : IWordType {

        #region properties

        public TextType Type => TextType.TYPE;

        public SupportedLanguages Language { get; set; }

        public IDefinitionLoader DefinitionLoader { get; set; }

        #endregion

        #region public methods

        public bool IsMatch(string word) => false;

        #endregion

    }
}
