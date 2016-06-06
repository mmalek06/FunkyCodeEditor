using System.Collections.Generic;
using CodeEditor.CodeParsing.WordTypes;
using CodeEditor.Enums;

namespace CodeEditor.CodeParsing {
    internal interface IDefinitionLoader {

        #region public methods

        IEnumerable<string> GetWords(TextType type, SupportedLanguages language);

        #endregion

    }
}
