using System.Collections.Generic;
using CodeEditor.DataStructures;

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

        public char GetCharAt(TextPosition position) => textView.Lines[position.Line][position.Column];

        public int GetTextLinesCount() => textView.Lines.Count;

        public int GetTextLineLength(int index) => index < textView.Lines.Count ? textView.Lines[index].Length : 0;

        public string GetTextLine(int index) => index >= textView.Lines.Count ? string.Empty : textView.Lines[index];

        public IEnumerable<string> GetAllTextLines() => textView.Lines;

        public IEnumerable<string> GetTextPartsBetweenPositions(TextPosition startPosition, TextPosition endPosition) {
            var parts = new string[(endPosition.Line - startPosition.Line) + 1];
            int middleIndex = 0;

            if (startPosition.Line == endPosition.Line) {
                parts[0] = textView.Lines[startPosition.Line].Substring(startPosition.Column, endPosition.Column - startPosition.Column);
            } else {
                parts[0] = textView.Lines[startPosition.Line].Substring(startPosition.Column);
                parts[parts.Length - 1] = textView.Lines[endPosition.Line].Substring(0, endPosition.Column);
                middleIndex = 1;
            }
            for (int i = startPosition.Line + 1; i < endPosition.Line; i++, middleIndex++) {
                parts[middleIndex] = textView.Lines[i];
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
