using System.Collections.Generic;
using System.Windows;
using TextEditor.DataStructures;

namespace TextEditor.Extensions {
    public static class PointExtensions {
        public static TextPosition GetDocumentPosition(this Point point) {
            var charSize = StringExtensions.GetCharSize();
            int column = (int)(point.X / charSize.Width);
            int line = (int)(point.Y / charSize.Height);

            return new TextPosition(column, line);
        }

        public static Point AlignToVisualLineTop(this Point point) {
            var docPosition = point.GetDocumentPosition();
            var charSize = StringExtensions.GetCharSize();
            int y = (int)(docPosition.Line * charSize.Height);
            int x = (int)(docPosition.Column * charSize.Width);

            return new Point(x, y);
        }

        public static Point AlignToVisualLineBottom(this Point point) {
            var docPosition = point.GetDocumentPosition();
            var charSize = StringExtensions.GetCharSize();
            int y = (int)((docPosition.Line + 1) * charSize.Height);
            int x = (int)(docPosition.Column * charSize.Width);

            return new Point(x, y);
        }

        public static IEnumerable<Point[]> GetBoundingBoxesInbetween(this Point point, Point other) {
            return null;
        }
    }
}
