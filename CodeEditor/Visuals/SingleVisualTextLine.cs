using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.TextFormatting;

namespace CodeEditor.Visuals {
    internal class SingleVisualTextLine : VisualTextLine {

        #region fields

        private SimpleTextSource textSource;

        #endregion

        #region properties

        public override int Length => textSource.Text.Length;

        public override string Text {
            get { return textSource.Text; }
            protected set { textSource.Text = value; }
        }

        #endregion

        #region constructor

        public SingleVisualTextLine(SimpleTextSource textSource, int index) {
            this.textSource = textSource;
            Text = textSource.Text;
            Index = index;
                        
            Redraw();
        }

        #endregion

        #region public methods

        public override void UpdateText(string text) => textSource.Text = text;

        public override void Redraw() {
            using (TextLine textLine = Formatter.FormatLine(textSource, 0, 96 * 6, ParagraphProperties, null)) {
                double top = Index * textLine.Height;

                using (var drawingContext = RenderOpen()) {
                    textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
                }
            }
        }

        public override IEnumerable<string> GetStringContents() => new[] { Text };

        public override IEnumerable<SimpleTextSource> GetTextSources() => new[] { textSource };

        #endregion

    }
}
