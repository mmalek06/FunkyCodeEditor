using System.Collections.Generic;
using System.Linq;
using CodeEditor.Core.DataStructures;
using CodeEditor.Visuals;

namespace CodeEditor.Views.Text {
    /// <summary>
    /// provides 'readonly' methods and properties that could be used by other views, controls or tests
    /// </summary>
    internal partial class TextView {

        #region properties

        public int LinesCount => visuals.Count;

        public IReadOnlyList<string> Lines => GetTextPartsBetweenPositions(new TextPosition(column: 0, line: 0), new TextPosition(column: GetLineLength(LinesCount - 1), line: LinesCount - 1)).ToList();

        #endregion

        #region public methods

        public char GetCharAt(TextPosition position) => ((VisualTextLine)visuals[position.Line]).GetCharAt(position.Column);

        public int GetLineLength(int index) => visuals.Count == 0 ? 0 : ((VisualTextLine)visuals[index]).Length;

        public string GetLine(int index) => index >= visuals.Count ? string.Empty : ((VisualTextLine)visuals[index]).Text;

        public IEnumerable<string> GetTextPartsBetweenPositions(TextPosition startPosition, TextPosition endPosition) {
            var parts = new List<string>();
            string lastPart = null;

            if (startPosition.Line == endPosition.Line) {
                parts.Add(((VisualTextLine)visuals[startPosition.Line]).Text.Substring(startPosition.Column, endPosition.Column - startPosition.Column));
            } else {
                parts.Add(((VisualTextLine)visuals[startPosition.Line]).Text.Substring(startPosition.Column));

                lastPart = ((VisualTextLine)visuals[endPosition.Line]).Text.Substring(0, endPosition.Column);
            }
            for (int i = startPosition.Line + 1; i < endPosition.Line; i++) {
                var contents = ((VisualTextLine)visuals[i]).GetStringContents();

                parts.AddRange(contents);
            }
            if (lastPart != null) {
                parts.Add(lastPart);
            }

            return parts;
        }

        public bool IsInTextRange(TextPosition position) {
            if (position.Column < 0 || position.Line < 0) {
                return false;
            }
            if (position.Line >= LinesCount) {
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
