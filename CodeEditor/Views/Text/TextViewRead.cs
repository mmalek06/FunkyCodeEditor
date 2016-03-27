using System.Collections.Generic;
using System.Linq;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Views.Text {
    internal partial class TextView {

        #region public methods

        public char GetCharAt(TextPosition position) => VisibleTextLines.ElementAt(position.Line)[position.Column];

        public int GetLinesCount() => VisibleTextLines.Count();

        public int GetLineLength(int index) => index < VisibleTextLines.Count() ? VisibleTextLines.ElementAt(index).Length : 0;

        public string GetLine(int index) => index >= VisibleTextLines.Count() ? string.Empty : VisibleTextLines.ElementAt(index);

        public IEnumerable<string> GetTextPartsBetweenPositions(TextPosition startPosition, TextPosition endPosition) {
            var parts = new string[(endPosition.Line - startPosition.Line) + 1];
            int middleIndex = 0;

            if (startPosition.Line == endPosition.Line) {
                parts[0] = VisibleTextLines.ElementAt(startPosition.Line).Substring(startPosition.Column, endPosition.Column - startPosition.Column);
            } else {
                parts[0] = VisibleTextLines.ElementAt(startPosition.Line).Substring(startPosition.Column);
                parts[parts.Length - 1] = VisibleTextLines.ElementAt(endPosition.Line).Substring(0, endPosition.Column);
                middleIndex = 1;
            }
            for (int i = startPosition.Line + 1; i < endPosition.Line; i++, middleIndex++) {
                parts[middleIndex] = VisibleTextLines.ElementAt(i);
            }

            return parts;
        }

        public bool IsInTextRange(TextPosition position) {
            if (position.Column < 0 || position.Line < 0) {
                return false;
            }
            if (position.Line >= GetLinesCount()) {
                return false;
            }
            if (position.Column > GetLineLength(position.Line)) {
                return false;
            }

            return true;
        }

        #endregion

    }
}
