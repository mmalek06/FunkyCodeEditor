using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using CodeEditor.TextProperties;
using CodeEditor.Visuals;

namespace CodeEditor.Views.Caret {
    internal class VisualElement : VisualElementSymbolBase {

        #region fields

        private static double charWidth;

        #endregion

        #region properties
        
        public static string Symbol { get; private set; }

        #endregion

        #region constructor

        static VisualElement() {
            charWidth = StringExtensions.GetCharWidth();
            Symbol = GetSymbol();
        }

        #endregion

        #region public methods

        public void Draw(TextRunProperties runProperties, TextPosition position) {
            var textSource = new SimpleTextSource(Symbol, runProperties);
            double textHeight = 0;

            using (TextLine textLine = GetLine(runProperties, textSource, position.Column)) {
                textHeight = textLine.Height;
            }

            var formattedText = GetFormattedText(Symbol, runProperties);
            var textLocation = new Point(position.Column * charWidth - charWidth / 2, position.Line * textHeight);

            using (var drawingContext = RenderOpen()) {
                drawingContext.DrawText(formattedText, textLocation);
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

        private static string GetSymbol() {
            return "|";
        }

        #endregion

    }
}
