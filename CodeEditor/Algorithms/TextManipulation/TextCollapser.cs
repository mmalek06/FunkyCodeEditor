using System.Collections.Generic;
using System.Linq;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Visuals;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class TextCollapser {

        #region public methods

        public VisualTextLine CollapseTextRange(TextPositionsPair area, IEnumerable<string> lines, int index) {
            var middlePart = lines.Skip(area.StartPosition.Line).Take(area.EndPosition.Line - area.StartPosition.Line);
            string precedingText = new string(lines.ElementAt(area.StartPosition.Line).Take(area.StartPosition.Column).ToArray());
            string followingText = new string(lines.ElementAt(area.EndPosition.Line).Skip(area.EndPosition.Column + 1).ToArray());

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
