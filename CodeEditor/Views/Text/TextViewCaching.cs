using CodeEditor.Messaging;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Views.Text {
    internal partial class TextView {

        #region event handlers

        public void HandleScrolling(ScrollChangedMessage message) =>
            UpdateCache(message.LinesScrolled, message.FirstVisibleLineIndex, message.LastVisibleLineIndex);

        #endregion

        #region methods

        private void UpdateCache(int linesScrolled, int firstVisibleLineIndex, int lastVisibleLineIndex) {
            if (linesScrolled == 0) {
                return;
            }

            int startForTop = firstVisibleLineIndex - linesScrolled < 0 ? 0 : firstVisibleLineIndex - linesScrolled;

            ConvertVisualLinesToCachedLines(0, firstVisibleLineIndex);
            ConvertVisualLinesToCachedLines(lastVisibleLineIndex, visuals.Count - lastVisibleLineIndex);
            ConvertCachedLinesToVisualLines(firstVisibleLineIndex, lastVisibleLineIndex - firstVisibleLineIndex);
        }

        private void ConvertVisualLinesToCachedLines(int start, int count) {
            for (int i = start; i < start + count; i++) {
                if (visuals[i] is CachedVisualTextLine) {
                    continue;
                }

                var cachedLine = ((VisualTextLine)visuals[i]).ToCachedLine();

                visuals.RemoveAt(i);
                visuals.Insert(i, cachedLine);
            }
        }

        private void ConvertCachedLinesToVisualLines(int start, int count) {
            for (int i = start; i < start + count; i++) {
                if (!(visuals[i] is CachedVisualTextLine)) {
                    continue;
                }

                var visualLine = ((CachedVisualTextLine)visuals[i]).ToVisualTextLine();

                visuals.RemoveAt(i);
                visuals.Insert(i, visualLine);
                visualLine.Draw();
            }
        }

        #endregion

    }
}
