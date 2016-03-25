using System.Windows;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Core.Extensions {
    public static class PointExtensions {
        public static TextPosition GetDocumentPosition(this Point point, Size charSize) {
            int column = (int)(point.X / charSize.Width);
            int line = (int)(point.Y / charSize.Height);

            return new TextPosition(column, line);
        }
        
        public static Point AlignToVisualLineTop(this Point point, Size charSize) {
            var docPosition = point.GetDocumentPosition(charSize);

            return point.AlignToVisualLineTop(docPosition, charSize);
        }

        public static Point AlignToVisualLineTop(this Point point, TextPosition docPosition, Size charSize) {
            int y = (int)(docPosition.Line * charSize.Height);
            int x = (int)(docPosition.Column * charSize.Width);

            return new Point(x, y);
        }

        public static Point AlignToVisualLineBottom(this Point point, Size charSize) {
            var docPosition = point.GetDocumentPosition(charSize);

            return point.AlignToVisualLineBottom(docPosition, charSize);
        }

        public static Point AlignToVisualLineBottom(this Point point, TextPosition docPosition, Size charSize) {
            int y = (int)((docPosition.Line + 1) * charSize.Height);
            int x = (int)(docPosition.Column * charSize.Width);

            return new Point(x, y);
        }
    }
}
