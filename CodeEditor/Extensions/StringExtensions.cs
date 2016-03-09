using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CodeEditor.Configuration;

namespace CodeEditor.Extensions {
    public static class StringExtensions {

        #region fields

        private static Size? charSize;

        #endregion

        #region public methods

        public static Size GetScreenSize(this string text, FontFamily fontFamily, double fontSize, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch) {
            fontFamily = fontFamily ?? new TextBlock().FontFamily;
            fontSize = fontSize > 0 ? fontSize : new TextBlock().FontSize;

            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            var ft = new FormattedText(text ?? string.Empty, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black);

            return new Size(ft.Width, ft.Height);
        }

        public static Size GetCharSize() {
            if (charSize == null) {
                var s = "X".GetScreenSize(
                    TextConfiguration.GetFontFamily(),
                    TextConfiguration.GetFontSize(),
                    TextConfiguration.GetFontStyle(),
                    TextConfiguration.GetFontWeight(),
                    TextConfiguration.GetFontStretch());

                charSize = new Size(s.Width, s.Height);
            }

            return charSize.Value;
        }

        #endregion

    }
}
