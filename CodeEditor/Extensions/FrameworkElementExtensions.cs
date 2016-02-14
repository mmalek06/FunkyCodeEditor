using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using CodeEditor.TextProperties;

namespace CodeEditor.Extensions {
    public static class FrameworkElementExtensions {
        public static Typeface CreateTypeface(this FrameworkElement fe) {
            return new Typeface(Configuration.TextConfiguration.GetFontFamily(),
                                Configuration.TextConfiguration.GetFontStyle(),
                                Configuration.TextConfiguration.GetFontWeight(),
                                Configuration.TextConfiguration.GetFontStretch());
        }

        public static TextRunProperties CreateGlobalTextRunProperties(this FrameworkElement fe) {
            var p = new GlobalTextRunProperties();
            p.typeface = fe.CreateTypeface();
            p.fontRenderingEmSize = Configuration.TextConfiguration.GetFontSize();
            p.foregroundBrush = (Brush)fe.GetValue(Control.ForegroundProperty);
            p.cultureInfo = CultureInfo.CurrentCulture;

            return p;
        }
    }
}
