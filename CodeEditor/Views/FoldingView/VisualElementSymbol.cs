using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Algorithms.Folding;
using CodeEditor.TextProperties;

namespace CodeEditor.Views.FoldingView {
    internal class VisualElementSymbol : VisualElementSymbolBase {

        public void DrawFolding(TextRunProperties runProperties, FoldingStates state, int top) {
            string symbol = state == FoldingStates.EXPANDED ? "-" : "+";
            var textSource = new SimpleTextSource(symbol, runProperties);
            double textHeight = 0;

            using (TextLine textLine = GetLine(runProperties, textSource)) {
                textHeight = textLine.Height;
            }

            var formattedText = GetFormattedText(symbol, runProperties);
            var textLocation = new Point(0, top * textHeight);

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
