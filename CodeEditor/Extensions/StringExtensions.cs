using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CodeEditor.Configuration;

namespace CodeEditor.Extensions {
    public static class StringExtensions {
        public static Size GetScreenSize(this string text, FontFamily fontFamily, double fontSize, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch) {
            fontFamily = fontFamily ?? new TextBlock().FontFamily;
            fontSize = fontSize > 0 ? fontSize : new TextBlock().FontSize;

            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            var ft = new FormattedText(text ?? string.Empty, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);

            return new Size(ft.Width, ft.Height);
        }

        public static double GetCharWidth() {
            return GetCharSize().Width;
        }

        public static Size GetCharSize() {
            return "X".GetScreenSize(
                TextConfiguration.GetFontFamily(),
                TextConfiguration.GetFontSize(),
                TextConfiguration.GetFontStyle(),
                TextConfiguration.GetFontWeight(),
                TextConfiguration.GetFontStretch());
        }
    }
}
