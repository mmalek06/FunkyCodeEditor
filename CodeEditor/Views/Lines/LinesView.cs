using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;
using CodeEditor.Core.DataStructures;
using CodeEditor.Extensions;
using CodeEditor.TextProperties;
using CodeEditor.Views.BaseClasses;

namespace CodeEditor.Views.Lines {
    internal class LinesView : HelperViewBase {

        #region enums

        private enum TextInputType { ADD, REMOVE };

        #endregion

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
            paragraphProperties = new SimpleParagraphProperties { defaultTextRunProperties = TextConfiguration.GetGlobalTextRunProperties() };
        }

        #endregion

        #region event handlers

        public override void HandleLineRemove(Key key, TextPosition activePosition, int lineLen) {
            if (key == Key.Back && activePosition.Column == 0) {
                linesCount--;
                RedrawLines(TextInputType.REMOVE);
                UpdateSize();
            } else if (key == Key.Delete) {
                linesCount--;
                RedrawLines(TextInputType.REMOVE);
                UpdateSize();
            }
        }

        public override void HandleTextInput(string text, TextPosition activePosition) {
            if (text == TextProperties.Properties.NEWLINE) {
                linesCount++;
                RedrawLines(TextInputType.ADD);
                UpdateSize();
            }
        }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            RedrawLines(TextInputType.ADD);
        }

        protected override double GetWidth() => EditorConfiguration.GetLinesColumnWidth();

        #endregion

        #region methods

        private void UpdateSize() {
            double h = linesCount * StringExtensions.GetCharSize().Height;

            if (h > ActualHeight) {
                Height = h;
            }
        }

        private void RedrawLines(TextInputType inputType) {
            if (inputType == TextInputType.ADD) {
                visuals.Add(new VisualElement(linesCount));
            } else {
                var lastNum = visuals[visuals.Count - 1];

                visuals.Remove(lastNum);
            }
        }

        #endregion

    }
}
