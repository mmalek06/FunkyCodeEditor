using System.Windows;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Extensions {
    public static class PointExtensions {
        public static TextPosition GetDocumentPosition(this Point point) {
            var charSize = StringExtensions.GetCharSize();
            int column = (int)(point.X / charSize.Width);
            int line = (int)(point.Y / charSize.Height);

            return new TextPosition(column, line);
        }
        
        public static Point AlignToVisualLineTop(this Point point) {
            var docPosition = point.GetDocumentPosition();

            return point.AlignToVisualLineTop(docPosition);
        }

        public static Point AlignToVisualLineTop(this Point point, TextPosition docPosition) {
            var charSize = StringExtensions.GetCharSize();
            int y = (int)(docPosition.Line * charSize.Height);
            int x = (int)(docPosition.Column * charSize.Width);

            return new Point(x, y);
        }

        public static Point AlignToVisualLineBottom(this Point point) {
            var docPosition = point.GetDocumentPosition();

            return point.AlignToVisualLineBottom(docPosition);
        }

        public static Point AlignToVisualLineBottom(this Point point, TextPosition docPosition) {
            var charSize = StringExtensions.GetCharSize();
            int y = (int)((docPosition.Line + 1) * charSize.Height);
            int x = (int)(docPosition.Column * charSize.Width);

            return new Point(x, y);
        }
    }
}
