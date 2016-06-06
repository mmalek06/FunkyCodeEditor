using System.Linq;
using CodeEditor.Enums;

namespace CodeEditor.CodeParsing.WordTypes {
    internal class KeywordWordType : IWordType {

        #region properties

        public TextType Type => TextType.KEYWORD;

        public SupportedLanguages Language { get; set; }

        public IDefinitionLoader DefinitionLoader { get; set; }

        #endregion

        #region public methods

        public bool IsMatch(string word) {
            var words = DefinitionLoader.GetWords(Type, Language);

            return words.Contains(word);
        }

        #endregion

    }
}
