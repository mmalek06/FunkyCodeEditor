using System.Collections.Generic;
using System.Linq;
using CodeEditor.Visuals;

namespace CodeEditor.Views.Text {
    internal partial class TextView {

        #region methods

        private void RedrawCollapsedLine(VisualTextLine collapsedLine, int line) {
            if (line >= visuals.Count) {
                visuals.Add(null);
            } else {
                visuals[line] = null;
            }

            visuals[line] = collapsedLine;

            collapsedLine.Draw();
        }

        private void AddLines(IEnumerable<VisualTextLine> linesToDraw) {
            foreach (var line in linesToDraw) {
                visuals.Add(line);
                line.Draw();
            }
        }

        private void DrawLines(IEnumerable<VisualTextLine> linesToDraw) {
            foreach (var line in linesToDraw) {
                DrawLine(line);
            }
        }

        private void RemoveLines(IReadOnlyCollection<int> indices) {
            var visualsToRemove = new List<VisualTextLine>();

            foreach (var visual in visuals) {
                var line = (VisualTextLine)visual;

                if (indices.Contains(line.Index)) {
                    visualsToRemove.Add(line);
                }
            }
            foreach (var line in visualsToRemove) {
                visuals.Remove(line);
            }
        }

        private void DrawLine(VisualTextLine line) {
            if (line.Index < visuals.Count) {
                visuals.Insert(line.Index, line);
                visuals.RemoveAt(line.Index + 1);
                line.Draw();
            } else {
                visuals.Add(line);
                line.Draw();
            }
        }

        #endregion

    }
}
