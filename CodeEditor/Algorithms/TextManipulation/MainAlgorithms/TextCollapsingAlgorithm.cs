using System.Collections.Generic;
using System.Linq;
using CodeEditor.DataStructures;
using CodeEditor.Visuals;

namespace CodeEditor.Algorithms.TextManipulation {
    internal class TextCollapsingAlgorithm {

        #region public methods

        public IEnumerable<VisualTextLine> GetLinesToRedrawAfterCollapse(IReadOnlyList<VisualTextLine> visuals, VisualTextLine collapsedLine, TextRange range) {
            var linesToRedraw = new List<VisualTextLine>();

            if (range.StartPosition.Line == range.EndPosition.Line) {
                return linesToRedraw;
            }

            int collapseEndLine = range.EndPosition.Line;

            for (int i = collapseEndLine + 1, newIndex = collapsedLine.Index + 1; i < visuals.Count; i++, newIndex++) {
                var line = visuals[i].CloneWithIndexChange(newIndex);

                linesToRedraw.Add(line);
            }

            return linesToRedraw;
        }

        public IEnumerable<VisualTextLine> GetLinesToRedrawAfterExpand(IEnumerable<VisualTextLine> lines, int nextLineIndex) {
            var linesToRedraw = new List<VisualTextLine>();

            foreach (var line in lines) {
                var clonedLine = line.CloneWithIndexChange(line.Index + nextLineIndex);

                linesToRedraw.Add(clonedLine);
            }

            return linesToRedraw;
        }

        public VisualTextLine CollapseTextRange(TextRange area, IReadOnlyList<string> lines, ICollapseRepresentation collapseRepresentationAlgorithm) {
            string precedingText = new string(lines[area.StartPosition.Line].Take(area.StartPosition.Column).ToArray());
            string followingText = new string(lines[area.EndPosition.Line].Skip(area.EndPosition.Column + 1).ToArray());
            int collapsedLineIndex = area.StartPosition.Line;
            var linesToStartFrom = lines.Skip(area.StartPosition.Line);
            var middlePart = new List<string>();
            int start = area.StartPosition.Line;
            int end = area.EndPosition.Line;

            if (start != end) {
                for (int i = start; i <= end; i++) {
                    string currentLine = lines[i];

                    if (i == start) {
                        currentLine = string.Join("", currentLine.Skip(area.StartPosition.Column));
                    } else if (i == end) {
                        currentLine = string.Join("", currentLine.Take(area.EndPosition.Column + 1));
                    }

                    middlePart.Add(currentLine);
                }
            } else {
                middlePart.Add(string.Join("", lines[start].Skip(area.StartPosition.Column).Take((1 + area.EndPosition.Column) - area.StartPosition.Column)));
            }

            return VisualTextLine.Create(middlePart, precedingText, followingText, collapsedLineIndex, collapseRepresentationAlgorithm.GetCollapseRepresentation());
        }

        public IEnumerable<VisualTextLine> ExpandTextRange(TextRange area, IEnumerable<string> lines) =>
            lines.Skip(area.StartPosition.Line).Take(1 + area.EndPosition.Line - area.StartPosition.Line).Select((line, index) => VisualTextLine.Create(line, area.StartPosition.Line + index));

        #endregion

    }
}
