using System.Collections.Generic;
using System.Linq;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Views.Text {
    internal class TextInfo {

        #region fields

        private TextView textView;

        #endregion

        #region constructor

        public TextInfo(TextView textView) {
            this.textView = textView;
        }

        #endregion

        #region public methods

        public char GetCharAt(TextPosition position) => textView.VisibleTextLines.ElementAt(position.Line)[position.Column];

        public int GetTextLinesCount() => textView.VisibleTextLines.Count();

        public int GetTextLineLength(int index) => index < textView.VisibleTextLines.Count() ? textView.VisibleTextLines.ElementAt(index).Length : 0;

        public string GetTextLine(int index) => index >= textView.VisibleTextLines.Count() ? string.Empty : textView.VisibleTextLines.ElementAt(index);

        public IEnumerable<string> GetAllTextLines() => textView.VisibleTextLines;

        public IEnumerable<string> GetTextPartsBetweenPositions(TextPosition startPosition, TextPosition endPosition) {
            var parts = new string[(endPosition.Line - startPosition.Line) + 1];
            int middleIndex = 0;

            if (startPosition.Line == endPosition.Line) {
                parts[0] = textView.VisibleTextLines.ElementAt(startPosition.Line).Substring(startPosition.Column, endPosition.Column - startPosition.Column);
            } else {
                parts[0] = textView.VisibleTextLines.ElementAt(startPosition.Line).Substring(startPosition.Column);
                parts[parts.Length - 1] = textView.VisibleTextLines.ElementAt(endPosition.Line).Substring(0, endPosition.Column);
                middleIndex = 1;
            }
            for (int i = startPosition.Line + 1; i < endPosition.Line; i++, middleIndex++) {
                parts[middleIndex] = textView.VisibleTextLines.ElementAt(i);
            }

            return parts;
        }

        public bool IsInTextRange(TextPosition position) {
            if (position.Column < 0 || position.Line < 0) {
                return false;
            }
            if (position.Line >= GetTextLinesCount()) {
                return false;
            }
            if (position.Column > GetTextLineLength(position.Line)) {
                return false;
            }

            return true;
        }

        #endregion

    }
}
