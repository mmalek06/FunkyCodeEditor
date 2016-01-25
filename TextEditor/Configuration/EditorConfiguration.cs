using System.Windows.Media;
using TextEditor.Extensions;

namespace TextEditor.Configuration {
    public static class EditorConfiguration {

        internal static int GetLinesColumnWidth() {
            return ((int)StringExtensions.GetCharWidth()) + 10;
        }

        internal static Brush GetLinesColumnBrush() {
            return Brushes.Black;
        }

        internal static Color GetLinesColumnFontColor() {
            return Colors.White;
        }

    }
}
