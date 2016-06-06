using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CodeEditor.Configuration;

namespace CodeEditor.Views.Lines {
    internal class VisualElement : DrawingVisual {

        #region constructor

        public VisualElement(int num) {
            Redraw(num);
        }

        #endregion

        #region public methods

        public void Redraw(int num) {
            var fontColor = SharedEditorConfiguration.GetLinesColumnFontColor();
            var typeface = SharedEditorConfiguration.GetTypeface();
            var fontHeight = TextConfiguration.GetCharSize().Height;

            using (var drawingContext = RenderOpen()) {
                drawingContext.DrawText(
                    new FormattedText(num.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontHeight, fontColor),
                    new Point(0, fontHeight * (num - 1)));
            }
        }

        #endregion

        #region methods

        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters) {
            return null;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters) {
            return null;
        }

        #endregion

    }
}
