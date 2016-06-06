using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.TextFormatting;
using CodeEditor.TextProperties;
using CodeEditor.Visuals.Base;

namespace CodeEditor.Visuals {
    internal class SingleVisualTextLine : VisualTextLine {

        #region fields

        private SimpleTextSource textSource;

        #endregion

        #region properties

        public override int Length => textSource.Text.Length;

        public override string RenderedText => textSource.Text;

        #endregion

        #region constructor

        public SingleVisualTextLine(SimpleTextSource textSource, int index) {
            this.textSource = textSource;
            Index = index;
        }

        #endregion

        #region public methods

        public override void Draw() {
            using (var textLine = Formatter.FormatLine(textSource, 0, 96 * 6, ParagraphProperties, null)) {
                var top = Index * textLine.Height;

                using (var drawingContext = RenderOpen()) {
                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
            }
        }

        public override IReadOnlyList<string> GetStringContents() => new[] { RenderedText };

        public override CharInfo GetCharInfoAt(int column) => new CharInfo { IsCharacter = true, Text = RenderedText[column].ToString() };

        public override VisualTextLine CloneWithIndexChange(int index) => Create(RenderedText, index);

        public override CachedVisualTextLine ToCachedLine() =>
            new CachedSingleVisualTextLine(RenderedText, Index);

        #endregion

    }
}
