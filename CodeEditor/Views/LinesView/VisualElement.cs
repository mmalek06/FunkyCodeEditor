using System.Globalization;
using System.Windows;
using System.Windows.Media;
using CodeEditor.Configuration;

namespace CodeEditor.Views.LinesView {
    internal class VisualElement : DrawingVisual {

        #region constructor

        public VisualElement(int num) {
            Redraw(num);
        }

        #endregion

        #region public methods

        public void Redraw(int num) {
            var fontColor = EditorConfiguration.GetLinesColumnFontColor();
            double fontHeight = EditorConfiguration.GetFontHeight();
            var typeface = EditorConfiguration.GetTypeface();

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
