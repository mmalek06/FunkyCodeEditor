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
            int closingBracketNum = 0;
            var linesArray = textLines.ToArray();
            int linesCount = textLines.Count();

            for (int i = linesCount - 1; i >= startPosition.Line; i--) {
                for (int j = linesArray[i].Length - 1; j >= 0; j--) {
                    char character = linesArray[i][j];

                    if (character == '}') {
                        closingBracketNum++;
                    }
                    if (closingBracketNum == openingBracketNum && character == '}') {
                        return new TextPosition { Column = j + 1, Line = i };
                    }
                }
            }

            return null;
        }

        #endregion

    }
}
