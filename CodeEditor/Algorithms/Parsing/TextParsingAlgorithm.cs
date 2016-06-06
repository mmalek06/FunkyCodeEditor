using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeEditor.Algorithms.Parsing.WordTypes;
using CodeEditor.Configuration;
using ParseResultType = System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<CodeEditor.Algorithms.Parsing.ParsingInfo>>;
using ParseResultContents = System.Collections.Generic.List<System.Collections.Generic.List<CodeEditor.Algorithms.Parsing.ParsingInfo>>;

namespace CodeEditor.Algorithms.Parsing {
    internal class TextParsingAlgorithm {

        #region fields

        private static readonly IEnumerable<IWordType> WordParsers = SharedEditorConfiguration.GetWordParsers();

        #endregion

        #region public methods

        public ParseResultType Parse(IEnumerable<string> lines) {
            var result = new ParseResultContents();
            var lineIndex = 0;

            foreach (var line in lines) {
                var parts = Regex.Matches(line, "(?<match>[^\\s\"]+)|(?<match>\"[^\"]*\")")
                                 .Cast<Match>()
                                 .Select(m => m.Groups["match"].Value)
                                 .ToArray();
                var parsingInfos = from parser in WordParsers
                                   from word in parts
                                   where parser.IsMatch(word)
                                   select new ParsingInfo(type: parser.Type, text: word);

                result[lineIndex] = new List<ParsingInfo>(parsingInfos);

                lineIndex++;
            }

            return result;
        }

        #endregion

    }
}
