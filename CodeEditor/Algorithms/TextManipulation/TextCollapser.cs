using System.Collections.Generic;
using System.Linq;
using CodeEditor.Core.DataStructures;
using CodeEditor.Visuals;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class TextCollapser {

        #region public methods

        public VisualTextLine CollapseTextRange(TextPositionsPair area, IEnumerable<SimpleTextSource> textSources, int index) {
            var middlePart = textSources.Skip(area.StartPosition.Line).Take(area.EndPosition.Line - area.StartPosition.Line);
            string precedingText = new string(textSources.ElementAt(area.StartPosition.Line).Text.Take(area.StartPosition.Column).ToArray());
            string followingText = new string(textSources.ElementAt(area.EndPosition.Line).Text.Skip(area.EndPosition.Column).ToArray());

            return VisualTextLine.Create(middlePart, precedingText, followingText, index);
        }

        public IEnumerable<VisualTextLine> ExpandTextRange(TextPositionsPair area, IEnumerable<SimpleTextSource> textSources, int index) {
            return null;
        }

        #endregion

    }
}
