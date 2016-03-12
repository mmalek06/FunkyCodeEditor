using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Configuration;
using CodeEditor.Visuals;

namespace CodeEditor.Views.Folding {
    internal class VisualElementSymbol : VisualElementSymbolBase {

        public void DrawFolding(FoldingStates state, int top) {
            string symbol = state == FoldingStates.EXPANDED ? "-" : "+";
            var runProperties = TextConfiguration.GetGlobalTextRunProperties();
            var formattedText = GetFormattedText(symbol, runProperties);
            var textLocation = new Point(5, top);

            using (var drawingContext = RenderOpen()) {
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Black, 1), new Rect(4, top + 2, 10, 9));
                drawingContext.DrawText(formattedText, textLocation);
            }
        }

        public void DrawLine(TextRunProperties runProperties, FoldingStates state, int top, int lineSpan) {

        }

        protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters) {
            return null;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters) {
            return null;
        }

    }
}
