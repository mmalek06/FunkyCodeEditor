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

        private IEnumerable<IWordType> wordTypes;

        private readonly SupportedLanguages language;

        private readonly IDefinitionLoader definitionLoader;

        #endregion

        #region properties

        private IEnumerable<IWordType> WordTypes => wordTypes ?? (wordTypes = SharedEditorConfiguration.GetWordParsers(language, definitionLoader));

        #endregion

        #region constructor

        public TextParsingAlgorithm(SupportedLanguages language, IDefinitionLoader definitionLoader) {
            this.language = language;
            this.definitionLoader = definitionLoader;
        }

        #endregion

        #region public methods

        public ParseResultType Parse(IEnumerable<string> lines) {
            var initialParsingResult = ParseInitial(lines);
            var filteredParsingResult = Filter(initialParsingResult);

            return filteredParsingResult;
        }
        
        #endregion

        #region methods

        private ParseResultType ParseInitial(IEnumerable<string> lines) =>
            from line in lines
            select Regex.Matches(line, "(?<match>[^\\s\"]+)|(?<match>\"[^\"]*\")")
                        .Cast<Match>()
                        .Select(m => m.Groups["match"].Value)
                        .ToArray() 
            into parts
            select
                (from type in WordTypes
                 from text in parts
                 where type.IsMatch(text)
                 select new ParsingInfo(type.Type, text)) 
            into parsingInfos
            select parsingInfos;

        private ParseResultType Filter(ParseResultType initialParsingResult) {
            var result = new List<List<ParsingInfo>>();

            foreach (var row in initialParsingResult) {
                foreach (var info in row) {
                    if (info.Type != TextType.STANDARD) {
                        continue;
                    }
                }
            }

            return result;
        }

        #endregion

    }
}
