using CodeEditor.TextProperties;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace CodeEditor.Visuals {
    internal abstract class VisualElementSymbolBase : DrawingVisual {
        protected TextLine GetLine(TextRunProperties runProperties, TextSource textSource, int column = 0) {
            return TextFormatter.Create().FormatLine(
                textSource,
                column,
                96 * 6,
                new SimpleParagraphProperties { defaultTextRunProperties = runProperties },
                null);
        }

        protected FormattedText GetFormattedText(string text, TextRunProperties runProperties) {
            return new FormattedText(
                text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                runProperties.Typeface,
                runProperties.FontRenderingEmSize,
                Brushes.Black);
        }
    }
}
