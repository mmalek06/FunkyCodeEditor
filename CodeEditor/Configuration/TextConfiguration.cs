using System.Windows;
using System.Windows.Media;

namespace CodeEditor.Configuration {
    public static class TextConfiguration {

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

    }
}
