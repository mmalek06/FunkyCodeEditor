using System.Windows.Media;
using TextEditor.Extensions;

namespace TextEditor.Configuration {
    public static class EditorConfiguration {

        internal static int GetLinesColumnWidth() => ((int)StringExtensions.GetCharWidth()) + 10;

        internal static Brush GetLinesColumnBrush() {
            return Brushes.Black;
        }

        internal static Brush GetLinesColumnFontColor() {
            return Brushes.White;
        }

        internal static double GetFontHeight() => StringExtensions.GetCharSize().Height;

        internal static Typeface GetTypeface() => 
            new Typeface(TextConfiguration.GetFontFamily(), TextConfiguration.GetFontStyle(), TextConfiguration.GetFontWeight(), TextConfiguration.GetFontStretch());
    }
}
