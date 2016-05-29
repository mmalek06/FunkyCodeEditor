using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.Extensions;
using CodeEditor.TextProperties;

namespace CodeEditor.Configuration {
    internal static class TextConfiguration {

        #region fields

        private static Typeface typeface;

        private static TextRunProperties textRunProperties;
        
        private static Size? charSize;

        #endregion

        #region public methods

        public static int GetFontSize() {
            return 12;
        }

        public static FontFamily GetFontFamily() {
            return new FontFamily("Courier New");
        }

        public static FontStyle GetFontStyle() {
            return FontStyles.Normal;
        }

        public static FontWeight GetFontWeight() {
            return FontWeights.Normal;
        }

        public static FontStretch GetFontStretch() {
            return FontStretches.Normal;
        }

        public static Typeface GetTypeface() {
            if (typeface == null) {
                typeface = CreateTypeface();
            }

            return typeface;
        }

        public static TextRunProperties GetGlobalTextRunProperties() {
            if (textRunProperties == null) {
                textRunProperties = CreateGlobalTextRunProperties();
            }

            return textRunProperties;
        }

        public static Typeface CreateTypeface() {
            return new Typeface(GetFontFamily(),
                                GetFontStyle(),
                                GetFontWeight(),
                                GetFontStretch());
        }

        public static TextRunProperties CreateGlobalTextRunProperties() {
            var fe = new FrameworkElement();
            var p = new GlobalTextRunProperties();

            p.typeface = CreateTypeface();
            p.fontRenderingEmSize = GetFontSize();
            p.foregroundBrush = (Brush)fe.GetValue(Control.ForegroundProperty);
            p.cultureInfo = CultureInfo.CurrentCulture;

            return p;
        }

        public static Size GetCharSize() {
            if (charSize == null) {
                var s = "X".GetScreenSize(
                    GetFontFamily(),
                    GetFontSize(),
                    GetFontStyle(),
                    GetFontWeight(),
                    GetFontStretch());

                charSize = new Size(s.Width, s.Height);
            }

            return charSize.Value;
        }

        #endregion

    }
}
