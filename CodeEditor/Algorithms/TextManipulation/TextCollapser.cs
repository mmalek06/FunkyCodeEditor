using System.Collections.Generic;
using System.Linq;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Visuals;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class TextCollapser {

        #region public methods

        public VisualTextLine CollapseTextRange(TextPositionsPair area, IReadOnlyList<string> lines, int index) {
            string precedingText = new string(lines[area.StartPosition.Line].Take(area.StartPosition.Column).ToArray());
            string followingText = new string(lines[area.EndPosition.Line].Skip(area.EndPosition.Column + 1).ToArray());
            var linesToStartFrom = lines.Skip(area.StartPosition.Line);
            var middlePart = new List<string>();
            int start = area.StartPosition.Line;
            int end = area.EndPosition.Line;

            for (int i = start; i <= end; i++) {
                string currentLine = lines[i];

                if (i == start) {
                    currentLine = string.Join("", currentLine.Skip(area.StartPosition.Column));
                } else if (i == end) {
                    currentLine = string.Join("", currentLine.Take(area.EndPosition.Column + 1));
                }

                middlePart.Add(currentLine);
            }

            return VisualTextLine.Create(middlePart, precedingText, followingText, index, GetCollapseRepresentation());
        }

        public IEnumerable<VisualTextLine> ExpandTextRange(TextPositionsPair area, IEnumerable<string> lines, int index) {
            return null;
        }

        #endregion

        #region methods

        private string GetCollapseRepresentation() {
            if (EditorConfiguration.GetFormattingType() == Enums.LanguageFormattingType.BRACKETS) {
                return "{...}";
            }

            return string.Empty;
        }

        #endregion

    }
}
