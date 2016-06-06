using CodeEditor.Enums;

namespace CodeEditor.CodeParsing.WordTypes {
    internal interface IWordType {

        #region properties

        TextType Type { get; }

        SupportedLanguages Language { get; }

        IDefinitionLoader DefinitionLoader { get; }

        #endregion

        #region public methods

        bool IsMatch(string word);

        #endregion

    }
}
