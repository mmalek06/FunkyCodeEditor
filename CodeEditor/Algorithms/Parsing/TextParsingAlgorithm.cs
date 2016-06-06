using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeEditor.CodeParsing;
using CodeEditor.CodeParsing.WordTypes;
using CodeEditor.Configuration;
using CodeEditor.Enums;
using ParseResultType = System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<CodeEditor.Algorithms.Parsing.ParsingInfo>>;

namespace CodeEditor.Algorithms.Parsing {
    internal class TextParsingAlgorithm {

        #region fields

        private IEnumerable<IWordType> wordParsers;

        private readonly SupportedLanguages language;

        private readonly IDefinitionLoader definitionLoader;

        #endregion

        #region constructor

        public TextParsingAlgorithm(SupportedLanguages language, IDefinitionLoader definitionLoader) {
            this.language = language;
            this.definitionLoader = definitionLoader;
        }

        #endregion

        #region public methods

        public ParseResultType Parse(IEnumerable<string> lines) {
            if (wordParsers == null) {
                wordParsers = SharedEditorConfiguration.GetWordParsers(language, definitionLoader);
            }

            return (from line in lines
                    select Regex.Matches(line, "(?<match>[^\\s\"]+)|(?<match>\"[^\"]*\")")
                                .Cast<Match>()
                                .Select(m => m.Groups["match"].Value)
                                .ToArray() into parts
                    select 
                        (from parser in wordParsers
                         from word in parts
                         where parser.IsMatch(word)
                         select new ParsingInfo(type: parser.Type, text: word)) 
                    into parsingInfos
                    select new List<ParsingInfo>(parsingInfos)).ToList();
        }

        #endregion

    }
}
