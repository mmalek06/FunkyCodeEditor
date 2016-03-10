using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Configuration;
using CodeEditor.DataStructures;
using CodeEditor.Extensions;
using CodeEditor.TextProperties;
using CodeEditor.Visuals;

namespace CodeEditor.Views.Caret {
    internal class VisualElement : VisualElementSymbolBase {

        #region fields

        private static Size charSize;
        private static TextRunProperties runProperties;
        private static SimpleTextSource textSource;

        #endregion

        #region properties

        public static string Symbol { get; private set; }

        #endregion

        #region constructor

        static VisualElement() {
            Symbol = GetSymbol();
            runProperties = TextConfiguration.GetGlobalTextRunProperties(); 
            textSource = new SimpleTextSource(Symbol, runProperties);
            charSize = StringExtensions.GetCharSize();
        }

        #endregion

        #region public methods

        public void Draw(TextPosition position) {
            var formattedText = GetFormattedText(Symbol, runProperties);
            var textLocation = new Point(position.Column * charSize.Width - charSize.Width / 2, position.Line * charSize.Height);

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
