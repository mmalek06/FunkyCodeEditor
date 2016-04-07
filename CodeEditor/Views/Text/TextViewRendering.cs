using System.Collections.Generic;
using CodeEditor.Visuals;

namespace CodeEditor.Views.Text {
    internal partial class TextView {

        #region methods

        private void RedrawCollapsedLine(VisualTextLine collapsedLine, int line) {
            visuals[line] = null;
            visuals[line] = collapsedLine;

            collapsedLine.Draw();
        }

        private void DrawLines(IEnumerable<VisualTextLine> linesToDraw) {
            foreach (var line in linesToDraw) {
                visuals.Add(line);
                line.Draw();
            }
        }

        private void DrawLines(IReadOnlyDictionary<int, string> linesData) {
            foreach (var pair in linesData) {
                DrawLine(pair.Key, pair.Value);
            }
        }

        private void DrawLine(int index, string newText) {
            VisualTextLine line;

            if (index < visuals.Count) {
                line = (VisualTextLine)visuals[index];

                line.UpdateText(newText);
                line.Draw();
            } else {
                line = VisualTextLine.Create(newText, index);
                line.Draw();

                visuals.Add(line);
            }
        }

        #endregion

    }
}
