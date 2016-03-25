using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.TextFormatting;

namespace CodeEditor.Visuals {
    internal class SingleVisualTextLine : VisualTextLine {

        #region fields

        private SimpleTextSource textSource;

        #endregion

        #region properties

        public SimpleTextSource TextSource {
            get { return textSource; }
            protected set { textSource = value; }
        }

        #endregion

        #region constructor

        public SingleVisualTextLine(SimpleTextSource textSource, int index) {
            this.textSource = textSource;
            Index = index;
            
            Redraw();
        }

        #endregion

        #region public methods

        public VisualTextLine Collapse(IEnumerable<SingleVisualTextLine> textLines, int collapseColumnStart, int collapseColumnEnd) {
            var precedingText = textLines.First().TextSource.Text.Take(collapseColumnStart).ToString();
            var followingText = textLines.Last().TextSource.Text.Skip(collapseColumnEnd).ToString();
            var line = Create(textLines.Select(l => l.TextSource), precedingText, followingText, Index);

            return line;
        }

        public override void Redraw() {
            using (TextLine textLine = Formatter.FormatLine(textSource, 0, 96 * 6, ParagraphProperties, null)) {
                double top = Index * textLine.Height;

                using (var drawingContext = RenderOpen()) {
                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
            }
        }

        #endregion

    }
}
