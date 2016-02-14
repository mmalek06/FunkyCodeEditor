using System.Collections.Generic;
using System.Linq;
using CodeEditor.DataStructures;
using CodeEditor.Views.TextView;

namespace CodeEditor.Algorithms.Folding {
    internal class BracketsFoldingAlgorithm : IFoldingAlgorithm {

        #region public methods

        public IList<string> GetCollapsedLines(IEnumerable<string> textLines, TextPosition startPosition, TextInfo textInfo) {
            int openingBracketNum = GetOpeningBracketNumber(textLines, startPosition);
            var endPosition = GetClosingBracketPosition(textLines, startPosition, openingBracketNum);

            if (endPosition == null) {
                return new string[0];
            }
            if (startPosition.Line == endPosition.Line) {
                return new string[0];
            }

            return textInfo.GetTextPartsBetweenPositions(startPosition, endPosition).ToArray();
        }

        #endregion

        #region methods

        private int GetOpeningBracketNumber(IEnumerable<string> textLines, TextPosition startPosition) => 
            textLines.Where((line, idx) => idx <= startPosition.Line)
                     .Select(line => line.Where((character, idx) => idx < startPosition.Column && character == '{').Count())
                     .Sum() + 1;

        private TextPosition GetClosingBracketPosition(IEnumerable<string> textLines, TextPosition startPosition, int openingBracketNum) {
            bool closingBracketFound = false;
            int closingBracketNum = 0;

            return textLines.Where((line, idx) => idx >= startPosition.Line)
                            .Select((line, idx) => {
                                if (closingBracketFound) {
                                    return null;
                                }

                                var indexes = line.Select((character, charIdx) => {
                                    if (character == '}') {
                                        closingBracketNum++;
                                    }
                                    if (closingBracketNum == openingBracketNum && character == '}') {
                                        closingBracketFound = true;

                                        return charIdx;
                                    }

                                    return -1;
                                }).ToArray();

                                if (!indexes.Any(number => number > -1)) {
                                    return null;
                                }

                                return new TextPosition { Column = indexes.First(index => index > -1) + 1, Line = idx };
                            })
                            .FirstOrDefault(position => position != null);
        }

        #endregion

    }
}
