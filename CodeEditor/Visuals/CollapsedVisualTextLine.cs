using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.TextFormatting;

namespace CodeEditor.Visuals {
    internal class CollapsedVisualTextLine : VisualTextLine {

        #region fields

        private SimpleTextSource textSourceBeforeCollapse;

        private SimpleTextSource textSourceAfterCollapse;

        private List<SimpleTextSource> collapsedContent;

        #endregion

        #region constructor

        public CollapsedVisualTextLine(IEnumerable<SimpleTextSource> textSourcesToCollapse, SimpleTextSource precedingSource, SimpleTextSource followingSource, int index) {
            collapsedContent = new List<SimpleTextSource>(textSourcesToCollapse);
            textSourceBeforeCollapse = precedingSource;
            textSourceAfterCollapse = followingSource;
            Index = index;

            Redraw();
        }

        #endregion

        #region public methods

        public IEnumerable<VisualTextLine> Expand() => collapsedContent.Select(source => Create(source, Index));

        public override void Redraw() {
            /*using (TextLine textLine = Formatter.FormatLine(textSource, 0, 96 * 6, ParagraphProperties, null)) {
                double top = Index * textLine.Height;

                using (var drawingContext = RenderOpen()) {
                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
            }*/
        }

        #endregion

    }
}
