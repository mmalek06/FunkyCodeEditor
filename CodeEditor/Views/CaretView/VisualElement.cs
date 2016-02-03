using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using CodeEditor.TextProperties;

namespace CodeEditor.Views.CaretView {
    internal class VisualElement : DrawingVisual {

        #region fields

        private static double charWidth;

        #endregion

        #region properties

        public TextPosition Position { get; set; }
        
        public static string Symbol { get; private set; }

        #endregion

        #region constructor

        static VisualElement() {
            charWidth = StringExtensions.GetCharWidth();
            Symbol = GetSymbol();
        }

        #endregion

        #region public methods

        public void Draw(TextRunProperties runProperties) {
            var textSource = new SimpleTextSource(Symbol, runProperties);
            double textHeight = 0;

            using (TextLine textLine = TextFormatter.Create().FormatLine(
                                textSource,
                                Position.Column,
                                96 * 6,
                                new SimpleParagraphProperties { defaultTextRunProperties = runProperties },
                                null)) {
                textHeight = textLine.Height;
            }

            var formattedText = new FormattedText(
                Symbol, 
                CultureInfo.CurrentUICulture, 
                FlowDirection.LeftToRight, 
                runProperties.Typeface, 
                runProperties.FontRenderingEmSize, 
                Brushes.Black);
            var textLocation = new Point(Position.Column * charWidth - charWidth / 2, Position.Line * textHeight);

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
