using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.TextFormatting;

namespace CodeEditor.Visuals {
    internal class SingleVisualTextLine : VisualTextLine {

        #region fields

        private SimpleTextSource textSource;

        #endregion

        #region constructor

        public SingleVisualTextLine(SimpleTextSource textSource, int index) {
            Text = textSource.Text;
            Index = index;
            this.textSource = textSource;
            
            Redraw();
        }

        #endregion

        #region public methods

        public override void Redraw() {
            using (TextLine textLine = Formatter.FormatLine(textSource, 0, 96 * 6, ParagraphProperties, null)) {
                double top = Index * textLine.Height;

                using (var drawingContext = RenderOpen()) {
                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
            }
        }

        public override IEnumerable<SimpleTextSource> GetTextSources() => new[] { textSource };

        #endregion

    }
}
