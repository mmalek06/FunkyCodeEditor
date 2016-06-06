using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;
using CodeEditor.TextProperties;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Visuals {
    internal class CollapsedVisualTextLine : VisualTextLine {

        #region fields

        private string collapseRepresentation;

        private string textBeforeCollapse;

        private string textAfterCollapse;

        private List<string> collapsedContent;

        #endregion

        #region properties

        public override string RenderedText => $"{textBeforeCollapse}{collapseRepresentation}{textAfterCollapse}";

        public override int Length => RenderedText.Length;

        #endregion

        #region constructor

        public CollapsedVisualTextLine(IEnumerable<SimpleTextSource> textSourcesToCollapse, SimpleTextSource precedingSource, SimpleTextSource followingSource, int index, string collapseRepresentation) {
            collapsedContent = new List<string>(textSourcesToCollapse.Select(source => source.Text));
            textBeforeCollapse = precedingSource.Text;
            textAfterCollapse = followingSource.Text;
            Index = index;
            this.collapseRepresentation = collapseRepresentation;
        }

        #endregion

        #region public methods

        public override void Draw() {
            var runProperties = TextConfiguration.GetGlobalTextRunProperties();

            using (var drawingContext = RenderOpen()) {
                double top;

                using (var textLine = Formatter.FormatLine(new SimpleTextSource(textBeforeCollapse, runProperties), 0, 96 * 6, ParagraphProperties, null)) {
                    top = Index * textLine.Height;

                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
                using (var textLine = Formatter.FormatLine(new SimpleTextSource(collapseRepresentation, runProperties), 0, 96 * 6, ParagraphProperties, null)) {    
                    textLine.Draw(drawingContext, new Point(TextConfiguration.GetCharSize().Width * textBeforeCollapse.Length, top), InvertAxes.None);
                }
                using (var textLine = Formatter.FormatLine(new SimpleTextSource(textAfterCollapse, runProperties), 0, 96 * 6, ParagraphProperties, null)) {
                    textLine.Draw(drawingContext, 
                        new Point(TextConfiguration.GetCharSize().Width * textBeforeCollapse.Length + TextConfiguration.GetCharSize().Width * collapseRepresentation.Length, top), 
                        InvertAxes.None);
                }
            }
        }

        public override IReadOnlyList<string> GetStringContents() =>
            CollapseLineTraits.GetStringContents(textBeforeCollapse, textAfterCollapse, collapsedContent);

        public override CharInfo GetCharInfoAt(int column) =>
            CollapseLineTraits.GetCharInfoAt(column, textBeforeCollapse, textAfterCollapse, RenderedText, Index, collapseRepresentation);

        public override VisualTextLine CloneWithIndexChange(int index) => Create(collapsedContent, textBeforeCollapse, textAfterCollapse, index, collapseRepresentation);

        public override CachedVisualTextLine ToCachedLine() =>
            new CachedCollapsedVisualTextLine(collapsedContent, textBeforeCollapse, textAfterCollapse, Index, collapseRepresentation);

        #endregion

    }
}
