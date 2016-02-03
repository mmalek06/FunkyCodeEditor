using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using TextEditor.Configuration;
using TextEditor.TextProperties;

namespace TextEditor.Extensions {
    public static class FrameworkElementExtensions {
        public static Typeface CreateTypeface(this FrameworkElement fe) {
            return new Typeface(TextConfiguration.GetFontFamily(),
                                TextConfiguration.GetFontStyle(),
                                TextConfiguration.GetFontWeight(),
                                TextConfiguration.GetFontStretch());
        }

        public static TextRunProperties CreateGlobalTextRunProperties(this FrameworkElement fe) {
            var p = new GlobalTextRunProperties();
            p.typeface = fe.CreateTypeface();
            p.fontRenderingEmSize = TextConfiguration.GetFontSize();
            p.foregroundBrush = (Brush)fe.GetValue(Control.ForegroundProperty);
            p.cultureInfo = CultureInfo.CurrentCulture;

            return p;
        }
    }
}
