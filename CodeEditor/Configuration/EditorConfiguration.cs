using System.Windows.Media;
using CodeEditor.Algorithms.Folding;
using CodeEditor.Extensions;

namespace CodeEditor.Configuration {
    internal static class EditorConfiguration {

        public static int GetLinesColumnWidth() => ((int)StringExtensions.GetCharSize().Width) * 5;

        public static int GetFoldingColumnWidth() => ((int)StringExtensions.GetCharSize().Width) * 3;

        public static int GetTextAreaLeftMargin() => GetLinesColumnWidth() + GetFoldingColumnWidth();

        public static Brush GetLinesColumnBrush() {
            return Brushes.Black;
        }

        public static Brush GetLinesColumnFontColor() {
            return Brushes.White;
        }

        public static Brush GetFoldingColumnBrush() {
            return GetEditorBrush();
        }

        public static Brush GetFoldingColumnFontColor() {
            return Brushes.LightBlue;
        }

        public static Brush GetEditorBrush() {
            return Brushes.LightGray;
        }

        public static Typeface GetTypeface() => 
            new Typeface(TextConfiguration.GetFontFamily(), TextConfiguration.GetFontStyle(), TextConfiguration.GetFontWeight(), TextConfiguration.GetFontStretch());

        public static IFoldingAlgorithm GetFoldingAlgorithm() => new BracketsFoldingAlgorithm();

    }
}
