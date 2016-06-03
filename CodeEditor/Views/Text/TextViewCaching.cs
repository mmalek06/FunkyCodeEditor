using CodeEditor.Visuals.Base;

namespace CodeEditor.Views.Text {
    internal partial class TextView {

        #region methods

        private void UpdateCache(int changeInLines, int topmostLine, int bottommostLine) {
            // scrolled to bottom
            if (changeInLines > 0) {
                ConvertVisualLinesToCachedLines(topmostLine - changeInLines, changeInLines);
                ConvertCachedLinesToVisualLines(bottommostLine - changeInLines, changeInLines);
            }
            // scrolled to top
            else if (changeInLines < 0) {
                changeInLines = -changeInLines;

                ConvertCachedLinesToVisualLines(topmostLine, changeInLines);
                ConvertVisualLinesToCachedLines(bottommostLine, changeInLines);
            }
        }

        #endregion

        #region methods

        private void ConvertVisualLinesToCachedLines(int start, int count) {
            start -= 1;

            for (int i = start; i < start + count; i++) {
                var cachedVisualLine = ((VisualTextLine)visuals[i]).ToCachedLine();

                visuals.RemoveAt(i);
                visuals.Insert(i, cachedVisualLine);
            }
        }

        private void ConvertCachedLinesToVisualLines(int start, int count) {
            start -= 1;

            for (int i = start; i < start + count; i++) {
                var visualLine = visuals[i] as CachedVisualTextLine;

                if (visualLine == null) {
                    continue;
                }

                visuals.RemoveAt(i);
                visuals.Insert(i, visualLine.ToVisualTextLine());
            }
        }

        #endregion

    }
}
