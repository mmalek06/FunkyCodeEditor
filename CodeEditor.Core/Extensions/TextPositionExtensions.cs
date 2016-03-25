using System.Windows;
using CodeEditor.Core.DataStructures;

namespace CodeEditor.Core.Extensions {
    public static class TextPositionExtensions {
        public static Point GetPositionRelativeToParent(this TextPosition position, Size charSize) {
            var point = new Point {
                X = position.Column * charSize.Width,
                Y = position.Line * charSize.Height
            };

            return point;
        }
    }
}
