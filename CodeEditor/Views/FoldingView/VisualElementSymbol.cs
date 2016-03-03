using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Visuals;

namespace CodeEditor.Views.FoldingView {
    internal class VisualElementSymbol : VisualElementSymbolBase {

        public void DrawFolding(TextRunProperties runProperties, FoldingStates state, int top) {
            string symbol = state == FoldingStates.EXPANDED ? "-" : "+";
            var formattedText = GetFormattedText(symbol, runProperties);
            var textLocation = new Point(0, top);

            using (var drawingContext = RenderOpen()) {
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
