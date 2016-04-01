using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.TextFormatting;
using CodeEditor.TextProperties;

namespace CodeEditor.Visuals {
    internal class SingleVisualTextLine : VisualTextLine {

        #region fields

        private SimpleTextSource textSource;

        #endregion

        #region properties

        public override int Length => textSource.Text.Length;

        public override string Text => textSource.Text;

        #endregion

        #region constructor

        public SingleVisualTextLine(SimpleTextSource textSource, int index) {
            this.textSource = textSource;
            Index = index;
        }

        #endregion

        #region public methods

        public override void UpdateText(string text) => textSource.Text = text;

        public override void Draw() {
            using (TextLine textLine = Formatter.FormatLine(textSource, 0, 96 * 6, ParagraphProperties, null)) {
                double top = Index * textLine.Height;

                using (var drawingContext = RenderOpen()) {
                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
            }
        }

        public override IReadOnlyCollection<string> GetStringContents() => new[] { Text };

        public override IReadOnlyCollection<SimpleTextSource> GetTextSources() => new[] { textSource };

        public override CharInfo GetCharInfoAt(int column) => new CharInfo { IsCharacter = true, Character = Text[column] };

        #endregion

    }
}
