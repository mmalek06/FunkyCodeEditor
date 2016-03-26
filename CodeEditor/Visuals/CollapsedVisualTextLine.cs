using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.TextFormatting;

namespace CodeEditor.Visuals {
    internal class CollapsedVisualTextLine : VisualTextLine {

        #region fields

        private string textBeforeCollapse;

        private string textAfterCollapse;

        private List<string> collapsedContent;

        #endregion

        #region constructor

        public CollapsedVisualTextLine(IEnumerable<SimpleTextSource> textSourcesToCollapse, SimpleTextSource precedingSource, SimpleTextSource followingSource, int index) {
            collapsedContent = new List<string>(textSourcesToCollapse.Select(source => source.Text));
            textBeforeCollapse = precedingSource.Text;
            textAfterCollapse = followingSource.Text;
            Index = index;

            Redraw();
        }

        #endregion

        #region public methods

        public override void Redraw() {
            var runProperties = Configuration.TextConfiguration.GetGlobalTextRunProperties();

            using (TextLine textLine = Formatter.FormatLine(new SimpleTextSource("{...}", runProperties), 0, 96 * 6, ParagraphProperties, null)) {
                double top = Index * textLine.Height;

                using (var drawingContext = RenderOpen()) {
                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
            }
        }

        public override IEnumerable<SimpleTextSource> GetTextSources() {
            var textSources = new List<SimpleTextSource>();
            var runProperties = Configuration.TextConfiguration.GetGlobalTextRunProperties();

            textSources.Add(new SimpleTextSource(textBeforeCollapse + collapsedContent[0], runProperties));
            textSources.AddRange(from text in collapsedContent.Skip(1).Take(collapsedContent.Count - 2)
                                 select new SimpleTextSource(text, runProperties));
            textSources.Add(new SimpleTextSource(collapsedContent.Last() + textAfterCollapse, runProperties));

            return textSources;
        }

        #endregion

    }
}
