using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace TextEditor.Views.TextView {
    internal class VisualTextLine : DrawingVisual {
        public VisualTextLine(TextLine textLine, double top) {
            Redraw(textLine, top);
        }

        public void Redraw(TextLine textLine, double top) {
            using (var drawingContext = RenderOpen()) {
                textLine.Draw(drawingContext, new Point(0, top), InvertAxes.None);
            }
        }

        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters) {
            return null;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters) {
            return null;
        }
    }
}
