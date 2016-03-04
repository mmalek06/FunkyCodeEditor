using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using CodeEditor.TextProperties;

namespace CodeEditor.Views.Lines {
    internal class LinesView : HelperViewBase {

        #region fields

        private int linesCount;
        private TextFormatter formatter;
        private SimpleParagraphProperties paragraphProperties;

        #endregion

        #region constructor

        public LinesView() : base() {
            bgBrush = EditorConfiguration.GetLinesColumnBrush();
            linesCount = 1;
            formatter = TextFormatter.Create();
            paragraphProperties = new SimpleParagraphProperties { defaultTextRunProperties = Configuration.TextConfiguration.GetGlobalTextRunProperties() };
        }

        #endregion

        #region event handlers

        public override void HandleTextInput(KeyEventArgs e, TextPosition activePosition) {
            if (activePosition.Line == 0 && activePosition.Column == 0 && e.Key == Key.Back) {
                return;
            }
            if (e.Key == Key.Back && activePosition.Column == 0) {
                linesCount--;
                RedrawLines();
            } else if (e.Key == Key.Delete) {
                linesCount--;
                RedrawLines();
            }
        }

        public override void HandleTextInput(TextCompositionEventArgs e, TextPosition activePosition) {
            if (e.Text == TextProperties.Properties.NEWLINE) {
                linesCount++;
                RedrawLines();
            }
        }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            RedrawLines();
        }

        protected override double GetWidth() => EditorConfiguration.GetLinesColumnWidth();

        #endregion

        #region methods

        private void RedrawLines() {
            var lineNumbers = Enumerable.Range(1, linesCount).ToArray();

            visuals.Clear();

            foreach (int num in lineNumbers) {
                var el = new VisualElement(num);

                visuals.Add(el);
            }
        }

        #endregion

    }
}
